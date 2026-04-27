# Airport Data Format (.atc)

Airport files live in `ATC/data/` and are XML documents with a `.atc` extension. They are parsed by `clsAirport` on startup.

---

## Coordinate System

All `x` / `y` attributes are in **metres** relative to the Airport Reference Point (ARP). The ARP is the geographic origin (`lat` / `lng`) stored in the file header.

- **x > 0** → east of ARP  
- **x < 0** → west of ARP  
- **y > 0** → south of ARP  
- **y < 0** → north of ARP  

Altitude attributes are in **feet**.

Geographic coordinates (`lat`, `lng`) elsewhere in the file are converted to (x, y) metres at parse time using `System.Device.Location.GeoCoordinate.GetDistanceTo`.

---

## Top-level Structure

```xml
<airport name="Tokyo Haneda" IATA="HND" ICAO="RJTT" date="2012/01/01">

  <!-- Radar view boundaries -->
  <groundradar>  <coordinate lat="35.549" lng="139.779"/> </groundradar>
  <tower>        <coordinate lat="..." lng="..."/>        </tower>
  <appdep>       <coordinate lat="..." lng="..."/>        </appdep>
  <tracon>       <coordinate lat="..." lng="..."/>        </tracon>

  <!-- Airport boundary polygon (for rendering) -->
  <landscape>
    <point x="..." y="..."/>
    ...
  </landscape>

  <!-- Navigation graph used by A* pathfinder -->
  <navpoints> ... </navpoints>

  <!-- Ramps / terminals / gates -->
  <ramp name="Terminal 1"> ... </ramp>

  <!-- Runways (arrival + departure infrastructure) -->
  <runway id="RWY34R" heading="..." ...> ... </runway>

  <!-- En-route waypoints for air-side navigation -->
  <airnavpoints>
    <airnavpoint id="DEITY" name="DEITY" lnglat="139.8,35.6,3000"/>
  </airnavpoints>

  <!-- Published arrival and departure routes -->
  <stars> <star name="STAR1"> ... </star> </stars>
  <sids>  <sid  name="SID1">  ... </sid>  </sids>

</airport>
```

---

## Navigation Points (`<navpoints>`)

Used for ground-side taxiway graph nodes (`clsConnectionPoint`):

```xml
<navpoints>
  <navpoint id="A1" name="Alpha 1" x="120.5" y="-340.2"
            isholdpoint="true" poiground="true" radarname="A1"/>
  ...
</navpoints>
```

| Attribute | Type | Description |
|---|---|---|
| `id` | String | Unique object ID |
| `name` | String | Display name |
| `x` | Double | Metres east of ARP |
| `y` | Double | Metres south of ARP (negative = north) |
| `lnglat` | `lng,lat,alt` | Alternative to x/y; converted on parse |
| `alt` | Double | Altitude in metres |
| `isholdpoint` | Boolean | Is this a runway hold-short point? |
| `poiground` | Boolean | Shown as POI on ground radar |
| `poitower` | Boolean | Shown as POI on tower radar |
| `radarname` | String | Short label shown on radar |
| `uiname` | String | Label used in UI panels |
| `stripename` | String | Label used on flight strips |

---

## Taxiway Paths (`<taxiways>`)

Edges in the ground navigation graph (`clsNavigationPath`):

```xml
<taxiways>
  <taxiway id="TW-A1-A2" from="A1" to="A2" name="A"
           type="taxiWay" maxspeed="20" taxipath="Alpha"/>
</taxiways>
```

### Path types (`enumPathWayType`)

| Value | Use |
|---|---|
| `taxiWay` | Normal taxiway |
| `runwayTaxiWay` | Crossing or parallel taxiway on runway |
| `exitWay` | Runway exit (high-speed turnoff) |
| `lineUpWay` | Runway entry path during line-up |
| `takeOffWay` | Takeoff roll segment |
| `touchDownWay` | Landing roll segment |
| `gateWay` | Gate approach path |
| `AirWay` | En-route air segment |

---

## Ramps and Gates

```xml
<ramp name="Terminal 1">
  <gate name="101" x="..." y="..." angle="180"/>
  <gate name="102" x="..." y="..." angle="180"/>
</ramp>
```

`angle` is the nose-in heading of a docked aircraft (degrees, 0–360).

---

## Runways

```xml
<runway id="RWY34R" heading="338.5">

  <!-- Arrival infrastructure -->
  <arrivalpoint id="AP-34R" .../>
  <touchdownways>
    <touchdownway id="TDW1" .../>
  </touchdownways>
  <exitways>
    <exitway id="EW-34R-A" .../>
  </exitways>
  <finals>
    <final name="ILS34R"> <finalpoint id="..."/> ... </final>
  </finals>

  <!-- Departure infrastructure -->
  <takeoffpoints>
    <takeoffpoint id="TOP-34R" .../>
  </takeoffpoints>
  <lineupways>
    <lineupway id="LU-34R" .../>
  </lineupways>
  <takeoffpaths>
    <takeoffpath id="TOP-34R-path" entry="..." exit="..."/>
  </takeoffpaths>

</runway>
```

---

## STARs and SIDs

```xml
<stars>
  <star name="DEITY2A">
    <starpoint id="DEITY" alt="3000"/>
    <starpoint id="FF34R" alt="2000"/>
  </star>
</stars>

<sids>
  <sid name="LAXO1">
    <sidpoint id="RWY34R-dep" alt="0"/>
    <sidpoint id="KAIHO" alt="5000"/>
  </sid>
</sids>
```

Each `<starpoint>` / `<sidpoint>` references a `navpoint` `id`. Altitude is the crossing altitude in feet.

---

## Adding a New Airport

1. Create `ATC/data/<name>.atc` following the structure above.
2. Open `ATC/Forms/frmMainMenu.Designer.vb` (or `frmMainMenu.vb`) and add the airport to `cboAirport`:

```vb
Me.cboAirport.Items.Add("My Airport (ICAO)")
Me.cboAirport.Tag(Me.cboAirport.Items.Count - 1) = "data\myairport.atc"
```
