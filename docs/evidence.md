# Evidence: pokemon-legality

- `/v1/check` verified 2026-11-01 against a legal Chespin (a static XY encounter generated with
  PKHeX.Core) and the same file with illegal EVs: `testdata/legal.pk6` -> valid, `illegal.pk6` ->
  `PK6:EVs:Invalid`. Run `scripts/smoke.sh`.
- The Gen 6 `/validator/v1/*` protocol is upstream's reverse engineering of `DllGts.cro` and
  `DllRandomTrade.cro`; not yet exercised by a real 3DS from OpenPak (needs a console capture).
