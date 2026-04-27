# Architecture

## Technology Stack

| Layer | Technology |
|---|---|
| Language | Visual Basic .NET |
| Framework | .NET Framework 4.7.2 |
| UI | Windows Forms + GDI+ |
| Serialization | `BinaryFormatter` (binary wire protocol) |
| Pathfinding | Custom A* (`clsAStarEngine`) |
| Networking | Raw TCP (`TcpListener` / `TcpClient`) |

---

## Class Diagram

```mermaid
classDiagram
    direction TB

    class clsGame {
        +isServer : Boolean
        +isclient : Boolean
        +isPaused : Boolean
        +AirPort : clsAirport
        +Planes : List~clsPlane~
        +maxPlanes : Long
        +crashedPlanes : Long
        +successfulLandings : Long
        +Universe : Timer
        +timerSpawn : Timer
        +timerWindChange : Timer
        +timerHistory : Timer
        +New(airportFilePath) «server»
        +New() «client»
        +prepareAirPort(filename)
        +preparePlaneTypes()
        +preparePlanesAtGates(max)
        --events--
        +selectedPlaneStatusChanged(plane)
        +planeFrequencyChanged(plane)
        +spawnedPlane(plane)
        +despawnedPlane(plane)
        +availableRunwaysArrivalChanged()
        +availableRunwaysDepartureChanged()
        +ticked(milliseconds)
        +radioMessage(frequency, message)
    }

    class clsPlane {
        +callsign : String
        +currentState : enumPlaneState
        +Frequency : enumFrequency
        +modelInfo : structPlaneTypeInfo
        +isGroundRadarRelevant : Boolean
        +isTowerRadarRelevant : Boolean
        +isArrDepRadarRelevant : Boolean
        +skeleton : structPlaneSkeleton
        --events--
        +statusChanged(plane)
        +frequencyChanged(plane)
        +cardFound(card)
    }

    class clsMovingObject {
        +pos_X : clsDistanceCollection
        +pos_Y : clsDistanceCollection
        +pos_Altitude : clsDistanceCollection
        +pos_direction : Double
        +mov_speed_absolute : clsSpeedCollection
        +target_speed : clsSpeedCollection
        +target_direction : Double
        +target_altitude : clsDistanceCollection
        +speed_X : Double
        +speed_Y : Double
        +move(timespan)
    }

    class clsAirport {
        +meta : structAirPortMeta
        +Ramps : clsRamp[]
        +runWays : clsRunWay[]
        +STARs : clsAirPath[]
        +SIDs : clsAirPath[]
        +airSpaceNavPoints : List~clsConnectionPoint~
        +gates : List~clsGate~
        +ARP : clsNavigationPoint
        +windDirectionTo : Double
        +allNavigationPoints : List~clsNavigationPoint~
        +allNavigationPaths : List~clsNavigationPath~
    }

    class clsRunWay {
        +objectID : String
        +name : String
        +isAvailableForArrival : Boolean
        +isAvailableForDeparture : Boolean
        +canHandleArrivals : Boolean
        +canHandleDepartures : Boolean
        +landingAngle : Double
        +takeoffAngle : Double
        +touchDownWays : clsTouchDownWay[]
        +exitPaths : clsExitWay[]
        +lineUpPaths : clsLineUpWay[]
        +takeOffPaths : clsTakeOffPath[]
        +FINALs : List~clsAirPath~
        +STARs : clsAirPath[]
        +SIDs : clsAirPath[]
    }

    class clsRamp {
        +Gates : clsGate[]
    }

    class clsGate {
        +name : String
        +pos_X : Double
        +pos_Y : Double
        +angle : Double
    }

    class clsConnectionPoint {
        +taxiWays : clsNavigationPath[]
        +isHoldPoint : Boolean
        +isRunwayPoint : Boolean
        +isLineUpPoint : Boolean
        +isGate : Boolean
        +addTaxiWay(taxiWay)
        +getTakeOffPath() List~structPathStep~
    }

    class clsNavigationPoint {
        +pos_X : Double
        +pos_Y : Double
        +altitude : clsDistanceCollection
        +objectID : String
        +name : String
        +isPOIGround : Boolean
        +isPOITower : Boolean
    }

    class clsNavigationPath {
        +ObjectID : String
        +name : String
        +type : enumPathWayType
        +maxSpeed : clsSpeedCollection
        +taxiWayPoint1 : clsConnectionPoint
        +taxiWayPoint2 : clsConnectionPoint
        +length : Double
        +oppositeTaxiWayPoint(point) clsConnectionPoint
        +directionFrom(point) Double
        +directionTo(point) Double
    }

    class clsAStarEngine {
        +Solution(start, end, startAngle, maxAngle, priorities) List~structPathStep~
        -findPath(start, end, startAngle, maxAngle) List~structPathStep~
    }

    class clsDistanceCollection {
        +feet : Double
        +meters : Double
        +nauticalMiles : Double
        +New(distance, unit)
    }

    class clsSpeedCollection {
        +knots : Double
        +inKmPerHour : Double
        +inMetersPerSecond : Double
        +inFeetPerSecond : Double
        +New(knots)
    }

    clsPlane --|> clsMovingObject
    clsConnectionPoint --|> clsNavigationPoint
    clsGate --|> clsConnectionPoint

    clsGame "1" --> "1" clsAirport
    clsGame "1" --> "*" clsPlane

    clsAirport "1" --> "*" clsRamp
    clsAirport "1" --> "*" clsRunWay
    clsRamp "1" --> "*" clsGate
    clsRunWay --> clsNavigationPoint
    clsRunWay --> clsNavigationPath

    clsNavigationPath --> clsConnectionPoint
    clsConnectionPoint --> clsNavigationPath

    clsPlane --> clsAStarEngine
    clsAStarEngine --> clsConnectionPoint
    clsAStarEngine --> clsNavigationPath

    clsMovingObject --> clsDistanceCollection
    clsMovingObject --> clsSpeedCollection
```

---

## Layer Overview

```
┌─────────────────────────────────────────────────────────┐
│                        Forms (UI)                       │
│  frmMainMenu  frmMenu  frmGroundRadar  frmTowerRadar    │
│  frmAppDepRadar  frmAllControl                          │
├─────────────────────────────────────────────────────────┤
│                    Game Engine                          │
│  clsGame  (timer loop, events, spawn, networking)       │
├──────────────────────┬──────────────────────────────────┤
│   Aircraft           │   Airport Model                  │
│   clsPlane           │   clsAirport                     │
│   clsMovingObject    │   clsRunWay, clsRamp, clsGate    │
│                      │   clsNavigationPoint/Path        │
├──────────────────────┴──────────────────────────────────┤
│               Pathfinding / Coordinates                 │
│   clsAStarEngine       clsEarth                         │
├─────────────────────────────────────────────────────────┤
│                  Shared Utilities                       │
│   clsDistanceCollection   clsSpeedCollection            │
│   mdlHelpers              mdlNetworkhandling            │
└─────────────────────────────────────────────────────────┘
```

---

## Directory Map

```
ATC/
├── Forms/              UI — Windows Forms + custom controls
├── game/               clsGame — core engine
├── moving_objects/     clsPlane, clsMovingObject, unit collections
├── airportclasses/     clsAirport + runway/ramp/gate/path hierarchy
│   ├── ramp/           clsRamp, clsGate, clsConnectionPoint
│   └── runway/
│       ├── arrival/    clsTouchDownWay, clsExitWay, clsAirPath
│       └── departure/  clsTakeOffPath, clsLineUpWay
├── pathfinder/         clsAStarEngine + support types
├── coordinates/        clsEarth (geo → meters)
├── modules/            mdlHelpers, mdlNetworkhandling
└── data/               Airport XML files (.atc)
```
