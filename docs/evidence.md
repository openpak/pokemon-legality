# Evidence: pokemon-legality

- `/v1/check` is exercised by `scripts/smoke.sh` against a legal Chespin (a static XY encounter
  generated with PKHeX.Core, `testdata/legal.pk6`) and the same file with illegal EVs
  (`illegal.pk6`, verdict `PK6:EVs:Invalid`).
- The Gen 6 `/validator/v1/*` protocol is upstream's reverse engineering of `DllGts.cro` and
  `DllRandomTrade.cro`; a capture from a real 3DS is still needed to confirm it.
