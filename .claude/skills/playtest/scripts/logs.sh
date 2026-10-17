#!/usr/bin/env bash
# Prints the Editor console entries logged after a cursor, one line each (stack trace only for errors and exceptions),
# then the cursor to pass next time.
#
# usage: logs.sh [since-cursor] [min-level]
#   since-cursor  "seq" to start after; omit or 0 for the last 200 entries
#   min-level     log | warn | error (default log)
set -euo pipefail

here="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
project="$(cd "$here/../../../../Rock Flipper" && pwd)"
since="${1:-0}"; level="${2:-log}"

if [[ "$since" == "0" ]]; then
  out="$(unity command console --tail 200 --project-path "$project" --json --no-banner)"
else
  out="$(unity command console --since "$since" --project-path "$project" --json --no-banner)"
fi

python -c '
import json, sys
level = sys.argv[1]
rank = {"log": 0, "warn": 1, "warning": 1, "error": 2, "exception": 2, "assert": 2}
d = json.loads(sys.stdin.read())
if not d.get("success"):
    for e in d.get("errors", []):
        print(e.get("message", e))
    sys.exit(1)
r = d["data"]["result"]
shown = 0
for e in r["entries"]:
    lv = e.get("level", "log")
    if rank.get(lv, 0) < rank[level]:
        continue
    shown += 1
    print("[%s] #%s %s" % (lv.upper(), e.get("seq"), e.get("message", "").strip()))
    if rank.get(lv, 0) >= 2 and e.get("stackTrace"):
        for line in e["stackTrace"].strip().splitlines()[:6]:
            print("        " + line)
print("-- %d shown of %d; next cursor: %s%s" % (shown, len(r["entries"]), r.get("cursor"), " (OLDER ENTRIES DROPPED)" if r.get("dropped") else ""))
' "$level" <<<"$out"
