# Game Loop

The simulation runs entirely on `System.Windows.Forms.Timer` instances owned by `clsGame`. There is no background thread; all logic executes on the UI thread.

---

## Timers

| Field | Interval | Role |
|---|---|---|
| `Universe` | 10 ms | Main physics + AI tick |
| `timerSpawn` | configurable | Spawns new aircraft |
| `timerEndGate` | configurable | Removes aircraft that have sat at gate too long |
| `timerWindChange` | configurable | Randomises wind direction/speed |
| `timerHistory` | 1 000 ms | Appends position snapshot to each plane's history |
| `tmrServerSendKeyFrame` | ~100 ms | Server → clients game-state broadcast |
| `tmrServerListen` | 100 ms | Server reads commands from clients |
| `tmrClientListen` | 100 ms | Client reads keyframes from server |

---

## Main Tick Sequence (`Universe`, every 10 ms)

```mermaid
sequenceDiagram
    participant T as Universe Timer
    participant G as clsGame
    participant P as clsPlane (each)
    participant A as clsAStarEngine
    participant UI as Forms (event handlers)

    T->>G: Universe_Tick
    G->>G: compute elapsed ms since last tick
    loop For each clsPlane
        G->>P: tick(elapsed)
        P->>P: advance position (move)
        P->>P: update speed toward target
        P->>P: update direction toward target
        P->>P: update altitude toward target
        alt Needs new ground path
            P->>A: Solution(start, goal, angle, maxAngle)
            A-->>P: List(Of structPathStep)
        end
        P->>P: evaluate state transitions
        alt State changed
            P->>G: statusChanged event
            G->>UI: selectedPlaneStatusChanged(plane)
        end
        alt Frequency changed
            P->>G: frequencyChanged event
            G->>UI: planeFrequencyChanged(plane)
        end
    end
    G->>G: check collisions between planes
    G->>UI: ticked(milliseconds)
```

---

## Spawn Cycle

```mermaid
sequenceDiagram
    participant S as timerSpawn
    participant G as clsGame
    participant A as clsAirport
    participant P as clsPlane (new)
    participant UI as Forms

    S->>G: timerSpawn_Tick
    G->>G: check allowSpawnUntil
    G->>G: check maxPlanes
    G->>A: pick random gate / runway
    G->>P: New clsPlane(...)
    G->>G: Planes.Add(plane)
    G->>UI: spawnedPlane(plane)
    G->>S: reset interval (random in [min, max])
```

---

## Despawn Cycle

```mermaid
sequenceDiagram
    participant G as clsGame
    participant P as clsPlane
    participant UI as Forms

    Note over P: state = tower_Departed<br/>or ground_atGate (timeout)
    G->>G: detect despawn condition in Universe_Tick
    G->>G: Planes.Remove(plane)
    G->>G: update stats (successfulLandings, etc.)
    G->>UI: despawnedPlane(plane)
```

---

## Event Flow (game → UI)

```mermaid
flowchart LR
    subgraph clsGame
        E1[selectedPlaneStatusChanged]
        E2[planeFrequencyChanged]
        E3[spawnedPlane]
        E4[despawnedPlane]
        E5[radioMessage]
        E6[ticked]
        E7[availableRunwaysArrivalChanged]
        E8[availableRunwaysDepartureChanged]
    end

    subgraph Forms
        F1[frmAllControl]
        F2[frmGroundRadar]
        F3[frmTowerRadar]
        F4[frmAppDepRadar]
        F5[frmMenu]
    end

    E1 --> F1
    E2 --> F1
    E3 --> F1 & F2 & F3
    E4 --> F1
    E5 --> F1
    E6 --> F2 & F3 & F4
    E7 --> F1 & F5
    E8 --> F1 & F5
```

---

## History Recording (`timerHistory`, every 1 s)

Each plane holds a circular buffer of the last `PLANE_HISTORY` (20) position snapshots. `timerHistory_Tick` iterates all planes and appends the current position. Forms use this to draw flight-path trails on radar displays.
