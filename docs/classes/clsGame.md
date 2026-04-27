# clsGame

**File**: `ATC/game/clsGame.vb`  
**Scope**: `Public Class` — `<Serializable>`

`clsGame` is the single source of truth for a running simulation. It owns all timers, all planes, the airport, and the network sockets. Forms hold a reference to one `clsGame` instance and subscribe to its events.

---

## Constructors

### Server mode

```vb
New clsGame(airportFilePath As String,
            Optional maxGroundPlanes As Long = Long.MaxValue,
            Optional maxPlanes As Long = Long.MaxValue)
```

1. Sets `isServer = True`
2. Loads `clsAirport` from XML (`prepareAirPort`)
3. Populates `planeTypes` (`preparePlaneTypes`)
4. Spawns initial gate aircraft (`preparePlanesAtGates`)
5. Creates all timers (disabled by default)

### Client mode

```vb
New clsGame()
```

Sets `isclient = True`. Airport data arrives over the wire; timers are created as needed.

---

## Key Fields

| Field | Type | Description |
|---|---|---|
| `isServer` | `Boolean` (ReadOnly) | True when this instance runs game logic |
| `isclient` | `Boolean` (ReadOnly) | True when this instance is a remote viewer |
| `AirPort` | `clsAirport` | Loaded airport model |
| `Planes` | `List(Of clsPlane)` | All currently active aircraft |
| `selectedPlane` | `clsPlane` | The plane currently selected in the UI |
| `maxPlanes` | `Long` | Hard cap on simultaneous aircraft |
| `isPaused` | `Boolean` | When True, `Universe` timer does not advance physics |
| `planeTypes` | `List(Of structPlaneTypeInfo)` | Available aircraft models |

---

## Events

| Event | Signature | When raised |
|---|---|---|
| `selectedPlaneStatusChanged` | `(plane As clsPlane)` | Selected plane changes state |
| `planeFrequencyChanged` | `(plane As clsPlane)` | Selected plane changes frequency |
| `spawnedPlane` | `(plane As clsPlane)` | New aircraft enters simulation |
| `despawnedPlane` | `(plane As clsPlane)` | Aircraft removed from simulation |
| `availableRunwaysArrivalChanged` | `()` | Active arrival runway set changes |
| `availableRunwaysDepartureChanged` | `()` | Active departure runway set changes |
| `usedRunwaysChanged` | `()` | Runway occupancy changes |
| `ticked` | `(milliseconds As Long)` | Each physics tick (10 ms) |
| `radioMessage` | `(frequency, message)` | ATC radio transmission |

---

## Timers

| Timer | Default Interval | Purpose |
|---|---|---|
| `Universe` | 10 ms | Main physics / AI tick |
| `timerSpawn` | 120 000 ms | Spawn new aircraft |
| `timerEndGate` | 120 000 ms | Remove idle gate aircraft |
| `timerWindChange` | 120 000 ms | Change wind |
| `timerHistory` | 1 000 ms | Record position history |
| `tmrServerSendKeyFrame` | ~100 ms | Broadcast keyframe to clients |
| `tmrServerListen` | 100 ms | Read commands from clients |
| `tmrClientListen` | 100 ms | Read keyframes from server |

---

## Statistics Fields

| Field | Counts |
|---|---|
| `crashedPlanes` | Collisions |
| `successfulLandings` | Aircraft that completed touchdown |
| `successfulGated` | Aircraft that reached any gate |
| `successfulArrival` | Aircraft that reached their assigned gate |
| `successfulTakeOffs` | Aircraft that lifted off |
| `successfulDeparted` | Aircraft that exited the sim after departure |

---

## Common Change Patterns

| Goal | Method to modify |
|---|---|
| Add a new aircraft type | `preparePlaneTypes()` — add a `structPlaneTypeInfo` entry |
| Change spawn timing | `timerSpawn_Tick` handler; adjust `minSpawnDelay` / `maxSpawnDelay` |
| Add a game-wide event | Declare `Friend Event` here; `RaiseEvent` in the tick or handler; `AddHandler` in forms |
| Modify wind behaviour | `timerWindChange_Tick` handler |
