#!/bin/sh
# Auth test against a running service started with PKHAX_INTERNAL_KEY=$2:
#   ./scripts/auth-test.sh http://127.0.0.1:20170 the-key
base=${1:-http://127.0.0.1:20170}; key=$2
fail=0
code() { curl -s -o /dev/null -w '%{http_code}' "$@"; }
chk() { [ "$2" = "$3" ] && echo "ok   $1" || { echo "FAIL $1: got $2 want $3"; fail=1; }; }
body='{"format":"pk6","data":"AAAA"}'
chk "check without key"       "$(code -XPOST "$base/v1/check" -d "$body")" 401
chk "check with wrong key"    "$(code -XPOST -H 'X-Internal-Key: nope' "$base/v1/check" -d "$body")" 401
chk "check with key"          "$(code -XPOST -H "X-Internal-Key: $key" "$base/v1/check" -d "$body")" 200
chk "public_key without key"  "$(code -XPOST "$base/validator/v1/public_key" -d x)" 401
chk "validate without key"    "$(code -XPOST "$base/validator/v1/validate" -d x)" 401
chk "public_key with key"     "$(code -XPOST -H "X-Internal-Key: $key" "$base/validator/v1/public_key" -d x)" 200
chk "healthz stays open"      "$(code "$base/healthz")" 200
exit $fail
