# Unit Collections

Two small value-wrapper classes eliminate explicit unit conversion throughout the codebase. Both are `<Serializable>` and live in `ATC/moving_objects/`.

---

## `clsDistanceCollection`

**File**: `ATC/moving_objects/clsDistanceCollection.vb`

Stores a single distance value and exposes it in three units. Internally stored as **feet**.

```vb
Public Class clsDistanceCollection
    Public Sub New(distance As Double, unit As enumDistanceUnits)

    Public Enum enumDistanceUnits
        meters
        feet
        nauticalMiles
    End Enum

    Friend Property feet          As Double   ' stored value
    Friend Property meters        As Double   ' feet × 0.3048 / 3.28084
    Friend Property nauticalMiles As Double   ' feet / 6076.12
End Class
```

### Conversion factors

| From → To | Factor |
|---|---|
| feet → metres | × 0.3048 |
| metres → feet | × 3.28084 |
| feet → NM | ÷ 6076.12 |
| NM → feet | × 6076.12 |

### Usage examples

```vb
Dim d As New clsDistanceCollection(500, clsDistanceCollection.enumDistanceUnits.meters)
Console.WriteLine(d.feet)          ' → 1640.4
Console.WriteLine(d.nauticalMiles) ' → 0.27

Dim alt As New clsDistanceCollection(35000, clsDistanceCollection.enumDistanceUnits.feet)
Console.WriteLine(alt.meters)      ' → 10668
```

---

## `clsSpeedCollection`

**File**: `ATC/moving_objects/clsSpeedCollection.vb`

Stores a single speed value and exposes it in six units. Internally stored as **knots**.

```vb
Public Class clsSpeedCollection
    Public Sub New(Optional knots As Double = 0)

    Friend Property knots            As Double   ' stored value
    Friend Property inKmPerHour      As Double   ' knots × 1.852
    Friend Property inMetersPerHour  As Double   ' knots × 1852
    Friend Property inMetersPerSecond As Double  ' inMetersPerHour / 3600
    Friend Property inMilesPerHour   As Double   ' knots × 1.15078
    Friend Property inFeetPerSecond  As Double   ' knots × 1.68781
    Friend Property inFeetPerHour    As Double   ' inFeetPerSecond / 3600
End Class
```

### Conversion factors

| From knots → | Factor |
|---|---|
| km/h | × 1.852 |
| m/h | × 1852 |
| m/s | × 1852 ÷ 3600 |
| mph | × 1.15078 |
| ft/s | × 1.68781 |

### Usage examples

```vb
Dim s As New clsSpeedCollection(250)   ' 250 knots
Console.WriteLine(s.inKmPerHour)       ' → 463
Console.WriteLine(s.inMetersPerSecond) ' → 128.6
Console.WriteLine(s.inFeetPerSecond)   ' → 421.9

Dim taxi As New clsSpeedCollection     ' 0 knots default
taxi.inKmPerHour = 30
Console.WriteLine(taxi.knots)          ' → 16.2
```

---

## Design Note

Setting any unit property recalculates and stores the canonical value (feet or knots). This means all conversions are always consistent:

```vb
Dim d As New clsDistanceCollection(0, clsDistanceCollection.enumDistanceUnits.meters)
d.feet = 1000
' d.meters → 304.8  (correct — recalculated from stored feet)
```
