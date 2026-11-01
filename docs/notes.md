# Notes: pokemon-legality

- Fork point: Pretendo PKHaX `38d76d4` (server.cs, csproj, Dockerfile). OpenPak adds `check.cs`
  (`/v1/check`), `/healthz`, key auto-generation, PKHeX.Core 26.10.18 and the tag-only release.
- Licences: PKHaX AGPL-3.0, PKHeX.Core GPL-3.0 (NuGet package; repo licence file is GPL-3.0 with
  additional notice terms). Combined work is AGPL-3.0-only; we publish source.
- PKHeX verdicts need the data files as stored. A PKHeX release bump can change verdicts; pin and bump deliberately.
- Why `common/`: the same service answers Gen 4/5 (WFC), Gen 6 (NEX) and future Switch callers.
