# ATC — Air Traffic Control Simulation

A Windows desktop game where you manage aircraft arrivals, departures, and ground movement at real-world airports.

**Version**: 0.2.9.3 · **Platform**: Windows · **Language**: Visual Basic .NET (.NET Framework 4.7.2)

---

## Build

**Requirements**: Windows, [.NET Framework 4.7.2](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net472)

**Visual Studio** (recommended): open `ATC.sln`, then press `Ctrl+Shift+B` to build or `F5` to build and run.

**Command line** (MSBuild):
```
msbuild ATC.sln /p:Configuration=Release /p:Platform="Any CPU"
```

Output: `ATC/bin/Release/ATC.exe`

> MSBuild ships with Visual Studio. On GitHub Actions the [`microsoft/setup-msbuild`](https://github.com/microsoft/setup-msbuild) action locates it automatically — see [`.github/workflows/build.yml`](.github/workflows/build.yml).

[![Build](https://github.com/thomasinjapan/ATC/actions/workflows/build.yml/badge.svg)](https://github.com/thomasinjapan/ATC/actions/workflows/build.yml)

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
