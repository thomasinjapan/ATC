# Multiplayer Networking

The game uses a raw TCP server/client model. One player hosts (server); others join (clients). All game logic runs on the server; clients receive read-only keyframe snapshots and send commands back.

---

## Roles

| Role | `clsGame` constructor | Key flag |
|---|---|---|
| Server | `New clsGame(filePath, ...)` | `isServer = True` |
| Client | `New clsGame()` | `isclient = True` |

---

## Connection Sequence

```mermaid
sequenceDiagram
    participant S as Server (clsGame)
    participant C as Client (frmMainMenu → clsGame)

    Note over S: TcpListener.Start()
    C->>S: TCP connect (IP : port)
    S->>C: serialize clsAirport (BinaryFormatter, length-prefixed)
    Note over C: Deserialize airport<br/>game.AirPort = airport
    C->>S: tmrClientListen.Enabled = True
    S->>C: tmrServerSendKeyFrame fires every ~100 ms
```

---

## Wire Protocol

All messages are **length-prefixed binary** frames:

```
┌──────────────────┬────────────────────────────────────┐
│  4 bytes (Int32) │  N bytes (BinaryFormatter payload)  │
│  payload length  │  structNetworkMessageFromServer     │
└──────────────────┴────────────────────────────────────┘
```

### Message types (`enumNetworkMessageType`)

| Value | Direction | Payload type |
|---|---|---|
| `keyframe` | Server → Client | `structNetworkKeyframeMessagefromServer` |
| `radioMessage` | Server → Client | `structRadioMessageNetwork` |
| _(commands)_ | Client → Server | `structCommandInfo` |

### Keyframe payload (`structNetworkKeyframeMessagefromServer`)

```vb
Structure structNetworkKeyframeMessagefromServer
    planeSkeletons         As List(Of clsPlane.structPlaneSkeleton)
    windDirectionTo        As Double
    openArrivalRunwayIDs   As List(Of String)
    openDepartureRunwayIDs As List(Of String)
    usedRunwayIDs          As List(Of String)
    radioMessage           As structRadioMessageNetwork
End Structure
```

### Plane skeleton (`structPlaneSkeleton`)

A lightweight snapshot of one aircraft — replaces the full `clsPlane` object over the wire:

```vb
Structure structPlaneSkeleton
    callsign, currentState, Frequency
    currentSpeedKnots, currentAltitudeFeet, currentDirection
    posXFeet, posYFeet
    targetAltitudeFeet, targetDirection, targetSpeedKnots
    tower_LineUpApproved, tower_clearToLand, tower_takeOffApproved
    air_currentAirPathName, air_flightPathIDs, air_goalWayPointID …
    ground_CurrentTaxiWayID, ground_taxiPathIDs, ground_goalWayPointID …
    modelInfo As structPlaneTypeInfo
End Structure
```

### Command payload (`structCommandInfo`)

```vb
Structure structCommandInfo
    plane                           As String           ' callsign
    command                         As enumCommands
    groundTaxiRunwayCommandParameter As clsRunWay
    groundTaxiGoalPointCommandParameter As clsConnectionPoint
    airDirectionCommandParameter    As Integer
    airNavPointCommandParameter     As clsNavigationPoint
    airAltitudeCommandParameter     As Double
    towerRunwayID                   As String
    ' … additional per-command fields
End Structure
```

---

## Keyframe Broadcast Sequence

```mermaid
sequenceDiagram
    participant KT as tmrServerSendKeyFrame
    participant G  as clsGame (server)
    participant NW as mdlNetworkhandling
    participant C1 as Client 1
    participant C2 as Client 2

    KT->>G: tmrServerSendKeyFrame_Tick
    G->>NW: serverSendUpdateToClients(game, keyframe)
    NW->>NW: build structNetworkKeyframeMessagefromServer
    NW->>NW: BinaryFormatter.Serialize → MemoryStream
    NW->>NW: prepend 4-byte length header
    NW->>C1: Socket.Send(byteArray)
    NW->>C2: Socket.Send(byteArray)
```

---

## Client Receive Sequence

```mermaid
sequenceDiagram
    participant CL as tmrClientListen
    participant G  as clsGame (client)
    participant NW as mdlNetworkhandling
    participant UI as Forms

    CL->>G: tmrClientListen_Tick
    G->>NW: clientReceive(game)
    NW->>NW: read 4-byte length prefix
    NW->>NW: read N bytes from NetworkStream
    NW->>NW: BinaryFormatter.Deserialize → structNetworkMessageFromServer
    alt keyframe
        NW->>G: apply skeleton to matching plane (by callsign)
        NW->>G: update openArrivalRunwayIDs, wind, etc.
        G->>UI: availableRunwaysArrivalChanged / Departure
    else radioMessage
        G->>UI: radioMessage(frequency, text)
    end
```

---

## Serialization Note

All types transmitted over TCP carry `<Serializable>` and use `BinaryFormatter`. **Do not replace with JSON or XML serialization** — the binary format is the live wire protocol between running instances.
