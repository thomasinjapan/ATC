# ATC — Air Traffic Control Simulation

A Windows desktop game where you manage aircraft arrivals, departures, and ground movement at real-world airports.

**Version**: 0.2.9.3 · **Platform**: Windows · **Language**: Visual Basic .NET (.NET Framework 4.7.2)

---

## Build

```
msbuild ATC.sln /p:Configuration=Debug /p:Platform="Any CPU"
```

Output: `ATC/bin/Debug/ATC.exe`

Requires .NET Framework 4.7.2 and Windows. Open `ATC.sln` in Visual Studio for the recommended IDE experience.

---

## Documentation

| Topic | Link |
|---|---|
| Architecture & class diagram | [docs/architecture.md](docs/architecture.md) |
| Game loop & timer lifecycle | [docs/game-loop.md](docs/game-loop.md) |
| Plane state machine | [docs/plane-states.md](docs/plane-states.md) |
| Multiplayer networking | [docs/networking.md](docs/networking.md) |
| Airport data format (.atc) | [docs/airport-format.md](docs/airport-format.md) |
| — clsGame | [docs/classes/clsGame.md](docs/classes/clsGame.md) |
| — clsPlane | [docs/classes/clsPlane.md](docs/classes/clsPlane.md) |
| — clsAirport | [docs/classes/clsAirport.md](docs/classes/clsAirport.md) |
| — clsAStarEngine | [docs/classes/clsAStarEngine.md](docs/classes/clsAStarEngine.md) |
| — Unit collections | [docs/classes/unit-collections.md](docs/classes/unit-collections.md) |

Full docs index: [docs/index.md](docs/index.md)

---

## Included Airports

| File | Airport |
|---|---|
| `haneda_2012.atc` | Tokyo Haneda (HND / RJTT) |
| `narita_2012.atc` | Tokyo Narita (NRT / RJAA) |
| `Tegel_2014.atc` | Berlin Tegel (TXL / EDDT) |
| `hiroshima.atc` | Hiroshima (HIJ / RJOA) |
| `Naha.atc` | Okinawa Naha (OKA / ROAH) |

---

## Multiplayer

One player creates a game (server). Others join by IP address from the main menu. The server runs all simulation logic and sends compressed keyframe snapshots to clients every ~100 ms over TCP.
