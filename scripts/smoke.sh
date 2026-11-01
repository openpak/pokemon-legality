#!/bin/sh
# Smoke test against a running service: ./scripts/smoke.sh http://127.0.0.1:20170
base=${1:-http://127.0.0.1:20170}
dir=$(dirname "$0")/../testdata
for f in legal illegal; do
  printf '%s: ' "$f"
  printf '{"format":"pk6","data":"%s"}' "$(base64 -w0 "$dir/$f.pk6")" | curl -s -XPOST "$base/v1/check" -d @-
  echo
done
