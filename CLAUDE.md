# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## ATC Simulation Game

## Project Overview

Air Traffic Control (ATC) simulation game — a Windows Forms desktop application written in **Visual Basic .NET** (.NET Framework 4.7.2). Players manage aircraft arrivals, departures, and ground movement at real-world airports.

- **Version**: 0.2.9.3 (author: Thomas Stein)
- **Type**: WinExe — Windows-only desktop app (Windows Forms + GDI+)
- **Multiplayer**: TCP server/client architecture; one player hosts (server), others join (clients)
- **Airport data**: Real airport layouts stored as XML-based `.atc` files in `ATC/data/`
- **No automated tests, no CI/CD, no linting tools**

---

## Build & Run

**IDE (recommended)**: Visual Studio — open `ATC.sln`

**MSBuild (command line)**:
```
msbuild ATC.sln /p:Configuration=Debug /p:Platform="Any CPU"
msbuild ATC.sln /p:Configuration=Release /p:Platform="Any CPU"
```

**Output directories**:
- Debug: `ATC/bin/Debug/ATC.exe`
- Release: `ATC/bin/Release/ATC.exe`

**Requirements**: .NET Framework 4.7.2, Windows OS

**Entry point**: `ATC.My.MyApplication` (auto-generated) → launches `frmMainMenu`

---

## Directory Structure

```
ATC.sln                              # Visual Studio solution file
ATC/
├── ATC.vbproj                       # VB.NET project file (.NET 4.7.2, WinExe)
├── App.config                       # Runtime config (supported .NET version)
├── app.manifest                     # Windows UAC manifest
├── ApplicationEvents.vb             # Application startup/shutdown hooks
│
├── Forms/                           # All Windows Forms UI
│   ├── frmMainMenu.vb               # Entry point: airport selection, game/client creation
│   ├── frmMenu.vb                   # In-game controls: pause, runway management
│   ├── frmGroundRadar.vb            # Ground/taxiway radar display
│   ├── frmTowerRadar.vb             # Tower radar display
│   ├── frmAppDepRadar.vb            # Approach/Departure radar
│   └── frmAllControl.vb             # Comprehensive unified control panel (72KB)
│
├── game/
│   └── clsGame.vb                   # Core game engine (60KB): game loop, networking, events
│
├── moving_objects/
│   ├── clsPlane.vb                  # Aircraft behavior, state machine, physics (2255 lines)
│   ├── clsMovingObject.vb           # Base class for all moving entities
│   ├── clsDistanceCollection.vb     # Unit-aware distance: ft ↔ m ↔ nm
│   └── clsSpeedCollection.vb        # Unit-aware speed: knots ↔ km/h ↔ m/s ↔ ft/s
│
├── airportclasses/
│   ├── clsAirport.vb                # Airport model + XML parser (26KB)
│   ├── clsNavigationPoint.vb        # Waypoints: X/Y position + altitude + ID
│   ├── clsNavigationPath.vb         # Directed path connection between waypoints
│   ├── clsLandscape.vb              # Airport terrain/boundary polygon
│   ├── clsPathWay.vb                # Ordered list of navigation paths
│   ├── clsAirPath.vb                # STAR or SID air path definition
│   ├── ramp/
│   │   ├── clsRamp.vb               # Ramp area (contains gates)
│   │   ├── clsConnectionPoint.vb    # Node in the taxiway graph
│   │   └── gate/
│   │       ├── clsGate.vb           # Individual gate
│   │       ├── clsGatePath.vb       # Gate approach path
│   │       └── clsGateWaySection.vb # Gate pathway segment
│   └── runway/
│       ├── clsRunWay.vb             # Runway with arrival + departure paths
│       ├── arrival/                 # clsTouchDownWay, clsExitWay, clsTouchDownWayPoint, clsAirPath
│       └── departure/               # clsTakeOffPath, clsTakeOffWay, clsTakeOffPoint, clsLineUpWay
│
├── pathfinder/
│   └── clsAStarEngine.vb            # A* pathfinding for ground taxi routing
│
├── coordinates/
│   └── clsEarth.vb                  # Geographic coordinates → meters conversion
│
├── modules/
│   ├── mdlHelpers.vb                # Math utilities: angle diff, 2D/3D distance
│   └── mdlNetworkhandling.vb        # TCP protocol, BinaryFormatter serialization, server/client I/O
│
├── data/                            # Airport configuration files (XML)
│   ├── haneda_2012.atc              # Tokyo Haneda (2.6 MB)
│   ├── Haneda_2012_lowres.atc       # Tokyo Haneda low-resolution variant
│   ├── Haneda_2012_simplified.atc   # Tokyo Haneda simplified variant
│   ├── narita_2012.atc              # Tokyo Narita (2.3 MB)
│   ├── Tegel_2014.atc               # Berlin Tegel (415 KB)
│   ├── hiroshima.atc                # Hiroshima (261 KB)
│   └── Naha.atc                     # Okinawa Naha (828 KB)
│
└── Resources/                       # Embedded images and UI assets
```

---

## Architecture

### Timer-Based Game Loop

All game logic is driven by VB.NET `System.Windows.Forms.Timer` instances in `clsGame`:

| Timer | Interval | Purpose |
|-------|----------|---------|
| `Universe` | 10 ms | Main physics/AI tick — moves planes, checks collisions |
| `timerSpawn` | configurable | Spawns new arriving/departing aircraft |
| `timerWindChange` | configurable | Changes wind direction/speed |
| `timerHistory` | 1000 ms | Records flight path history for all planes |
| `tmrServerSendKeyFrame` | ~100 ms | Server broadcasts game state to clients |
| `tmrServerListen` / `tmrClientListen` | 100 ms | Network I/O timers |

### Event-Driven Design

VB.NET `WithEvents` / `RaiseEvent` / `AddHandler` are used throughout. Key events raised by `clsGame`:

| Event | When raised |
|-------|-------------|
| `selectedPlaneStatusChanged(plane)` | Plane state changes |
| `planeFrequencyChanged(plane)` | Plane changes radio frequency |
| `spawnedPlane(plane)` | New aircraft spawned |
| `despawnedPlane(plane)` | Aircraft removed from simulation |
| `radioMessage(frequency, message)` | ATC radio communication |
| `availableRunwaysArrivalChanged()` | Active arrival runways change |
| `availableRunwaysDepartureChanged()` | Active departure runways change |
| `ticked(milliseconds)` | Each game tick (for UI sync) |

Forms subscribe to `clsGame` events via `AddHandler` after creating the game instance.

### Client-Server Networking

- **Server mode**: `New clsGame(filePath)` → `isServer = True`; starts `TcpListener`, sends airport data on connect
- **Client mode**: `New clsGame()` → `isclient = True`; connects via `TcpClient`, receives airport data
- **Serialization**: `BinaryFormatter` — **do not replace or refactor**; every type sent over the wire is `<Serializable>` and the binary format is the live protocol between server and clients
- **Keyframes** (`enumNetworkMessageType.keyframe`): server → clients, contains `structNetworkKeyframeMessagefromServer` (plane skeletons, wind, open runway IDs)
- **Commands** (`structCommandInfo`): client → server, targets a plane by callsign string

### Airport Object Hierarchy

```
clsAirport
├── Ramps: clsRamp[]
│   └── Gates: clsGate[]
├── RunWays: clsRunWay[]
│   ├── Arrival paths (STARs, touchdown ways, exit ways)
│   └── Departure paths (SIDs, takeoff ways, lineup ways)
├── STARs: clsAirPath[]   (approach routes)
├── SIDs: clsAirPath[]    (departure routes)
├── airSpaceNavPoints     (en-route waypoints)
└── POIs                  (points of interest)
```

### A* Pathfinding (Ground)

`clsAStarEngine.Solution(start, end, startAngle, maxAngle, priorities?)` returns `List(Of structPathStep)`, where each step contains the next `clsConnectionPoint` and the `clsNavigationPath` leading to it. The taxiway graph is built from `clsConnectionPoint` nodes connected by `clsNavigationPath` edges.

---

## Naming Conventions

| Prefix | Applied To | Examples |
|--------|-----------|---------|
| `cls` | Classes | `clsGame`, `clsPlane`, `clsAirport`, `clsRunWay` |
| `frm` | Windows Forms | `frmMainMenu`, `frmTowerRadar`, `frmGroundRadar` |
| `mdl` | Modules (static) | `mdlHelpers`, `mdlNetworkhandling` |
| `enum` | Enumerations | `enumPlaneState`, `enumCommands`, `enumFrequency` |
| `struct` | Structures | `structPlaneSkeleton`, `structCommandInfo`, `structPosition` |
| `ctl` | Custom controls | `ctlStripe`, `ctlWindRose`, `ctlStripeList` |
| `tmr` | Timer fields | `tmrServerListen`, `tmrClientListen` |

Form control prefixes: `cmd` (Button), `cbo` (ComboBox), `txt` (TextBox), `trk` (TrackBar), `lbl` (Label), `pic` (PictureBox).

---

## VB.NET Code Style

Every source file begins with:
```vb
Option Explicit On      ' Variables must be declared before use
Option Strict Off       ' Implicit type conversions are allowed
Option Compare Binary   ' String comparison is case-sensitive
Option Infer On         ' Type inference: Dim x = 5 → x is Integer
```

Additional conventions:
- **`Friend` scope** for most class members (package-private equivalent — accessible within the assembly)
- **`Public`** only for types/enums needed across assembly boundaries
- **`ReadOnly Property`** for values computed from other state (never cached separately)
- **XML doc comments** (`''' <summary>...</summary>`) on utility functions in modules
- **`<Serializable>`** attribute required on all classes/structures transmitted over the network
- **`WithEvents`** on fields that raise events; event handlers named `ObjectName_EventName`

**`Option Strict Off` means implicit type coercions compile silently.** Be deliberate: e.g., assigning a `Double` to an `Integer` field truncates without warning. When in doubt, cast explicitly (`CInt`, `CDbl`, etc.).

---

## Plane State Machine

`clsPlane.enumPlaneState` defines all aircraft states. Typical flow:

**Departure**:
`ground_atGate` → `ground_awaitingPushback` → `ground_inPushback` → `ground_breaking` → `ground_inTaxi` → `tower_inLineUp` → `tower_linedupAndWaiting` → `tower_takingOff` → `tower_Departed`

**Arrival**:
`tower_freeFlight` → `tower_FinalApproach` → `tower_enteringTouchDown` → `tower_inTouchDown` → `ground_breaking` → `ground_inTaxi` → `ground_inParking` → `ground_preparingGate` → `ground_atGate`

**Special**: `special_crashed` (terminal state)

---

## Airport Data Format (.atc files)

XML files in `ATC/data/`. All X/Y coordinates are in **meters** relative to the Airport Reference Point (ARP). Altitude is in **feet**.

```xml
<airport name="Tokyo Haneda" IATA="HND" ICAO="RJTT" date="2012/01/01">
  <groundradar><coordinate lat="35.549" lng="139.779"/></groundradar>
  <tower><coordinate lat="..." lng="..."/></tower>
  <appdep><coordinate lat="..." lng="..."/></appdep>
  <tracon><coordinate lat="..." lng="..."/></tracon>
  <landscape>
    <point x="..." y="..."/>
    ...
  </landscape>
  <ramp name="Terminal 1">
    <gate name="101" x="..." y="..." angle="..."/>
    ...
  </ramp>
  <runway id="RWY34R" heading="..." ...>
    <!-- touchdown ways, exit ways, lineup ways, takeoff paths -->
  </runway>
  <airnavpoints>
    <airnavpoint id="DEITY" name="DEITY" x="..." y="..." alt="..."/>
  </airnavpoints>
  <stars>
    <star name="STAR1"><starpoint id="..." alt="..."/></star>
  </stars>
  <sids>
    <sid name="SID1"><sidpoint id="..." alt="..."/></sid>
  </sids>
</airport>
```

**To add a new airport**:
1. Create a new `.atc` XML file in `ATC/data/`
2. Open `frmMainMenu.vb` and add the airport to the `cboAirport` combo box items, with the file path as its tag value

---

## Utility Functions

**`mdlHelpers.vb`** — math utilities (no instance required):
```vb
diffBetweenAnglesAbs(angle1, angle2)          ' → Double, range [0, 180]
diffBetweenAngles(angleFrom, angleTo)          ' → Double, range [-180, 180]
diffBetweenPoints2D(x1, y1, x2, y2)           ' → Double, Euclidean distance
diffBetweenPoints3D(x1, y1, z1, x2, y2, z2)  ' → Double, 3D Euclidean distance
```

**Unit collections** — wrap a value and expose it in all units:
```vb
Dim d As New clsDistanceCollection(500)   ' 500 meters
d.feet   ' → 1640.4
d.nm     ' → 0.27

Dim s As New clsSpeedCollection(250)      ' 250 knots
s.kmh    ' → 463
s.ms     ' → 128.6
```

---

## Common Change Patterns

| Goal | Files to modify |
|------|----------------|
| Add a new ATC command | `clsPlane.vb` (`enumCommands` + handler), `mdlNetworkhandling.vb` (serialization) |
| Add a new airport | Create `ATC/data/<name>.atc`, register in `frmMainMenu.vb` |
| Change radar rendering | `frmGroundRadar.vb`, `frmTowerRadar.vb`, or `frmAppDepRadar.vb` |
| Modify pathfinding | `ATC/pathfinder/clsAStarEngine.vb` |
| Add a game-wide event | Declare `Friend Event` in `clsGame.vb`, `RaiseEvent` where needed, `AddHandler` in forms |
| Change aircraft physics | `clsPlane.vb` (movement logic in `Universe_Tick` or equivalent) |
| Add a new plane type | `clsPlane.structPlaneTypeInfo`, populate in `clsGame.preparePlaneTypes()` |
| Change spawn behavior | `clsGame.vb` (`timerSpawn_Tick` handler) |
| Modify network protocol | `mdlNetworkhandling.vb` + update `structNetworkKeyframeMessagefromServer` |

---

## Domain Terminology

| Term | Meaning |
|------|---------|
| **STAR** | Standard Terminal Arrival Route — published path guiding aircraft into airport airspace |
| **SID** | Standard Instrument Departure — published path routing departing aircraft out of airspace |
| **ARP** | Airport Reference Point — geographic origin; all X/Y coords in `.atc` files are relative to this |
| **Squawk** | 4-digit transponder code identifying an aircraft on radar |
| **Pushback** | Aircraft reversing from gate with tug assistance |
| **Line-up** | Aircraft taxiing onto and positioning at the runway threshold |
| **Final approach** | Last straight-in segment of approach, aligned with runway centerline |
| **Go-around** | Aborted landing: aircraft climbs and re-enters the approach pattern |
| **Frequency** | VHF radio band: Ground (taxi), Tower (takeoff/landing), Approach/Departure |
| **ILS** | Instrument Landing System — precision guidance beam for final approach |
| **FL** | Flight Level: pressure altitude in hundreds of feet (FL150 = ~15,000 ft) |
| **NM / nm** | Nautical mile (1 NM = 1,852 m) |
| **Keyframe** | Full game-state snapshot sent periodically from server to clients |
| **Skeleton** | Serialized, lightweight representation of a plane's state for network transmission (`structPlaneSkeleton`) |
| **Ramp** | Apron area adjacent to terminal where aircraft park at gates |
| **Taxiway graph** | Network of `clsConnectionPoint` nodes + `clsNavigationPath` edges used for A* routing |
