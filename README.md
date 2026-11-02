# Pokemon Legality Check Server

OpenPak's Pokemon legality service. Every Pokemon that enters an OpenPak GTS, battle record or
showcase passes through it first.

| | |
| --- | --- |
| Network | Any (Gen 3 to 9 files; the 3DS Gen 6 validator protocol is served natively) |
| Role | Legality verdicts for raw Pokemon files, built on PKHeX.Core |
| Depends on | Nothing; stateless apart from one RSA key |
| Image | `ghcr.io/openpak/pokemon-legality` |
| Based on | [Pretendo `PKHaX`](https://github.com/PretendoNetwork/PKHaX) (AGPL-3.0) over [PKHeX.Core](https://github.com/kwsch/PKHeX) (GPL-3.0) |
| Licence | AGPL-3.0-only |

## What it serves

| Endpoint | What |
| --- | --- |
| `POST /v1/check` | JSON verdict for one Pokemon file, any generation (OpenPak addition) |
| `POST /validator/v1/validate` | Gen 6 3DS validator protocol, as upstream (Wonder Trade, GTS, Battle, Battle Video) |
| `POST /validator/v1/public_key` | Gen 6 3DS signing key, as upstream |
| `GET /healthz` | `ok` |

## Run

```sh
dotnet publish -c Release -o out
PKHAX_PORT=20170 PKHAX_PRIVATE_KEY_PATH=./private.key ./out/PKHaX
./scripts/smoke.sh http://127.0.0.1:20170
```

| Variable | Meaning |
| --- | --- |
| `PKHAX_PORT` | Listen port (image default 20170) |
| `PKHAX_PRIVATE_KEY_PATH` | RSA 2048 PEM; generated on first start if missing, keep it on a volume |
| `PKHAX_CERTIFICATE_ID` | Gen 6 certificate id (default 3) |
| `PKHAX_INTERNAL_KEY` | When set, `/v1/*` requires header `X-Internal-Key` |

## Docs

- [Protocol](docs/protocol.md)
- [Deploy](docs/deploy.md)
- [Provenance](docs/provenance.md)
- [Upstream README](docs/upstream.md)

## Licence

AGPL-3.0-only (see [LICENSE](LICENSE)). Links PKHeX.Core (GPL-3.0); both permit combination under
AGPL-3.0 section 13. Source stays public at github.com/openpak/pokemon-legality.
