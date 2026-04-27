# Plane State Machine

Every `clsPlane` instance always holds exactly one `enumPlaneState` value. The state drives which physics, AI, and radar-visibility rules apply on each game tick.

---

## State Diagram

```mermaid
stateDiagram-v2
    [*] --> ground_atGate : spawn (departure)

    %% Departure flow
    ground_atGate --> ground_awaitingPushback : ATC: askForPushback
    ground_awaitingPushback --> ground_inPushback : ATC: pushbackApproved
    ground_inPushback --> ground_breaking : reached pushback endpoint
    ground_breaking --> ground_inTaxi : ATC: taxiTo / continueTaxi
    ground_breaking --> ground_holdingPosition : ATC: holdPosition
    ground_holdingPosition --> ground_inTaxi : ATC: continueTaxi
    ground_inTaxi --> tower_inLineUp : ATC: lineUpandWait / lineUpandTakeOff
    tower_inLineUp --> tower_linedupAndWaiting : reached lineup point
    tower_linedupAndWaiting --> tower_takingOff : ATC: clearForTakeOff
    tower_takingOff --> tower_Departed : airborne & past SID end

    %% Arrival flow
    [*] --> tower_freeFlight : spawn (arrival)
    tower_freeFlight --> tower_FinalApproach : ATC: airEnterFinal
    tower_FinalApproach --> tower_enteringTouchDown : crossed runway threshold
    tower_enteringTouchDown --> tower_inTouchDown : aligned & cleared
    tower_inTouchDown --> ground_breaking : decelerated below taxi speed
    ground_breaking --> ground_inTaxi : ATC: taxiTo
    ground_inTaxi --> ground_inParking : entered gate pathway
    ground_inParking --> ground_preparingGate : close to gate
    ground_preparingGate --> ground_atGate : docked

    %% Crash (any state)
    ground_atGate --> special_crashed : collision
    ground_inTaxi --> special_crashed : collision
    tower_inTouchDown --> special_crashed : collision
    tower_takingOff --> special_crashed : collision
    tower_freeFlight --> special_crashed : collision

    %% Go-around
    tower_FinalApproach --> tower_freeFlight : ATC: airGoAround
    tower_enteringTouchDown --> tower_freeFlight : ATC: airGoAround

    %% Lineup cancel
    tower_inLineUp --> ground_inTaxi : ATC: cancelLineUp
    tower_linedupAndWaiting --> ground_inTaxi : ATC: cancelLineUp
```

---

## State Reference

### Ground states

| State | Meaning |
|---|---|
| `ground_atGate` | Parked at gate; engines off |
| `ground_awaitingPushback` | Ready to push back; waiting for ATC approval |
| `ground_inPushback` | Tug reversing aircraft away from gate |
| `ground_breaking` | Decelerating to a stop at a path endpoint or hold position |
| `ground_holdingPosition` | Stopped on taxiway, awaiting further clearance |
| `ground_inTaxi` | Taxiing along the A* ground path |
| `ground_inParking` | Following gate-approach path toward stand |
| `ground_preparingGate` | Final metres — aligning angle, coming to full stop |

### Tower states

| State | Meaning |
|---|---|
| `tower_inLineUp` | Moving onto runway via lineup path |
| `tower_linedupAndWaiting` | Holding on runway threshold, awaiting takeoff clearance |
| `tower_takingOff` | Accelerating along takeoff roll |
| `tower_Departed` | Handed off to departure/en-route; removed from active list |
| `tower_freeFlight` | Inbound, navigating via STARs / waypoints |
| `tower_FinalApproach` | On final, following the FINAL path |
| `tower_enteringTouchDown` | Crossed threshold; selecting touchdown way |
| `tower_inTouchDown` | On runway, decelerating |

### Special state

| State | Meaning |
|---|---|
| `special_crashed` | Terminal — aircraft remains visible on radar |

---

## Radar Visibility Rules

| Radar | Visible states |
|---|---|
| Ground radar | All `ground_*` states, `tower_inLineUp`, `tower_linedupAndWaiting`, `tower_takingOff`, `tower_inTouchDown`, `special_crashed` |
| Tower radar | All `tower_*` states; also ground states while on runway-type paths |
| Approach/Departure | `tower_freeFlight`, `tower_FinalApproach`, or altitude ≥ 100 ft |

---

## ATC Commands Reference

```mermaid
mindmap
  root((ATC Commands))
    Ground
      continueTaxi
      holdPosition
      pushbackApproved
      askForPushback
      groundExpectRunway
      taxiTo
    Tower / Runway
      lineUpandWait
      lineUpandTakeOff
      clearForTakeOff
      cancelLineUp
      cancelTakeOff
      towerExitVia
    Air
      airHeadToDirection
      airHeadToNavPoint
      airExpectRunway
      airMakeShortApproach
      airEnterFinal
      airClearedForLanding
      airGoAround
      airEnterSTAR
      airEnterSTARviaNavPoint
      airEnterSID
      airAdjustSpeed
      airAdjustAltitude
    Frequency
      contactTower
      contactArrival
      contactDeparture
      contactArrDep
      contactTracon
      contactGround
```
