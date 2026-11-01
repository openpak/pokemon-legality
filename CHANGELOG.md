# Changelog — pokemon-legality

## Unreleased

## v0.1.1 — 2026-11-01

- `/v1/check` also returns gender, nature, ability, held_item and moves (battle stats and showcase read them).

## v0.1.0 — 2026-11-01

- Fork of Pretendo PKHaX (`38d76d4`); PKHeX.Core 26.10.18.
- `POST /v1/check`: JSON legality verdict for any generation; `GET /healthz`.
- RSA key generated on first start when missing; default port 20170.
- Tag-only release workflow (ghcr.io/openpak/pokemon-legality).
