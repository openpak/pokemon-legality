# Deploy: pokemon-legality

Quadlet `pokemon-legality.container` on the `openpak.network` network (pinned IP via `pin-ips.sh --apply`),
image `ghcr.io/openpak/pokemon-legality:latest`, `AutoUpdate=registry`, volume for `/data` (the RSA key).
Port block 20170 (HTTP). Internal only; callers send `X-Internal-Key` (`PKHAX_INTERNAL_KEY` in the shared env).
Tags `v*.*.*` build the image; no other trigger.
