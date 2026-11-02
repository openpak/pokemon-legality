# Protocol: pokemon-legality

## `POST /v1/check`

Request (JSON, header `X-Internal-Key` when `PKHAX_INTERNAL_KEY` is set):

```json
{"format": "pk6", "data": "<base64 of the stored, decrypted-format Pokemon file>"}
```

`format` is a hint (`pk3` to `pk9`, `pb7`, `pa8`, `pb8`); PKHeX also detects the generation from the
length. Response 200:

```json
{"valid": true, "parsed": true, "generation": 6, "species": 650, "form": 0, "level": 1,
 "nickname": "Chespin", "ot": "OPENPAK", "shiny": false, "gender": 0, "nature": 3,
 "ability": 65, "held_item": 0, "moves": [33, 45], "issues": [], "error": null}
```

`gender` is PKHeX numbering (0 male, 1 female, 2 genderless); ids are PKHeX game ids.
`valid=false` carries `issues` such as `PK6:EVs:Invalid`. `error` is set when the bytes are not a
Pokemon file. 400 bad JSON or base64, 401 wrong key.

The files must be the stored (box) form, 232 bytes for PK6, 136 for PK4/PK5 stored part, 260 PK7.
Party-form trailers are tolerated by PKHeX. Encrypted network forms must be decrypted by the caller.

## Gen 6 validator (`/validator/v1/*`)

Unchanged from upstream, see [upstream.md](upstream.md) and `server.cs`. The 3DS game POSTs to it;
the real service token check is still a TODO upstream and here.

## Evidence

- `/v1/check` is exercised by `scripts/smoke.sh` against a legal Chespin (a static XY encounter
  generated with PKHeX.Core, `testdata/legal.pk6`) and the same file with illegal EVs
  (`illegal.pk6`, verdict `PK6:EVs:Invalid`).
- The Gen 6 `/validator/v1/*` protocol is upstream's reverse engineering of `DllGts.cro` and
  `DllRandomTrade.cro`; a capture from a real 3DS is still needed to confirm it.
- PKHeX verdicts need the data files as stored. A PKHeX release bump can change verdicts; pin and
  bump deliberately.
- The same service answers Gen 4/5 (WFC), Gen 6 (NEX) and future Switch callers, hence `common/`.
