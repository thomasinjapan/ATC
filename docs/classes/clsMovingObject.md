# clsMovingObject

**Inherits:** Object  
**Inherited By:** [clsPlane](clsPlane.md)

Base class for all entities that have a position, direction, speed, and can move in the simulation world.

## Properties

| Type | Name | Default |
|---|---|---|
| [clsDistanceCollection](clsDistanceCollection.md) | pos_X | — |
| [clsDistanceCollection](clsDistanceCollection.md) | pos_Y | — |
| [clsDistanceCollection](clsDistanceCollection.md) | pos_Altitude | — |
| Double | pos_direction | `0` |
| [clsSpeedCollection](clsSpeedCollection.md) | mov_speed_absolute | — |
| Double | mov_speed_rotation | — |
| [clsSpeedCollection](clsSpeedCollection.md) | target_speed | — |
| Double | target_direction | — |
| [clsDistanceCollection](clsDistanceCollection.md) | target_altitude | — |
| [clsDistanceCollection](clsDistanceCollection.md) | pointDetectionCircle | — |

## Properties (read-only)

| Type | Name | Description |
|---|---|---|
| Double | speed_X | Eastward velocity component in m/s |
| Double | speed_Y | Northward velocity component in m/s |

## Methods

| Returns | Signature |
|---|---|
| — | **New** () |
| — | **New** (positiondata As structPosition, movementData As structMovement, accelerationProperties As structAcceleration) |
| — | **move** (timespan As TimeSpan) |

## Structures

### structPosition

Snapshot of spatial state; used in the full constructor.

| Field | Type |
|---|---|
| pos_X | [clsDistanceCollection](clsDistanceCollection.md) |
| pos_Y | [clsDistanceCollection](clsDistanceCollection.md) |
| pos_Altitude | [clsDistanceCollection](clsDistanceCollection.md) |
| pos_direction | Double |

### structMovement

| Field | Type |
|---|---|
| speed_absolute | [clsSpeedCollection](clsSpeedCollection.md) |
| speed_rotation | Double |

### structAcceleration

| Field | Type |
|---|---|
| ground_accelleration_speed | Double |
| ground_accelleration_angle | Double |
| air_accelleration_speed | Double |
| air_accelleration_angle | Double |

## Property Descriptions

### Double pos_direction

Heading in degrees (0–360). Setter wraps automatically: values ≤ 0 have 360 added; values > 360 have 360 subtracted.

### Double speed_X

`mov_speed_absolute.inMetersPerSecond × sin(pos_direction × π / 180)`. Positive = east.

### Double speed_Y

`mov_speed_absolute.inMetersPerSecond × cos(pos_direction × π / 180)`. Positive = north. Note: Y-axis is inverted in screen space so `move()` subtracts this from pos_Y.

### clsDistanceCollection pointDetectionCircle

Tolerance radius within which the object is considered to have reached its current waypoint.

## Method Descriptions

### move(timespan As TimeSpan)

Advances pos_X and pos_Y by the velocity components over `timespan.TotalSeconds`.

```vb
pos_X.meters += speed_X * timespan.TotalSeconds
pos_Y.meters -= speed_Y * timespan.TotalSeconds
```
