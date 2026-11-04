# Changelog — pokemon-legality

## Unreleased

## v0.2.0 — 2026-11-04

- Authorization (#111): `PKHAX_INTERNAL_KEY` is now required (the service refuses to start without it). `X-Internal-Key` is checked in constant time on `/v1/*` and on `/validator/v1/*`; before, an unset key let anyone call `/v1/*` and the validator endpoints had no check at all. Callers (pokemon-home `HOME_LEGALITY_KEY`, pokemon-global-link `PGL_LEGALITY_KEY`) already send the key. `scripts/auth-test.sh` checks it against a running service.
- The upstream `TODO - VERIFY SERVICE TOKEN` is replaced by this: the 3DS service token itself is not parsed here; the caller must be a front holding the key.

## v0.1.1 — 2026-11-01

- `/v1/check` also returns gender, nature, ability, held_item and moves (battle stats and showcase read them).

## v0.1.0 — 2026-11-01

- Fork of Pretendo PKHaX (`38d76d4`); PKHeX.Core 26.10.18.
- `POST /v1/check`: JSON legality verdict for any generation; `GET /healthz`.
- RSA key generated on first start when missing; default port 20170.
- Tag-only release workflow (ghcr.io/openpak/pokemon-legality).
