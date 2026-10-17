#!/usr/bin/env bash
# Captures the Game view (UI canvases included) and writes it to a PNG outside the project.
#
# usage: shot.sh <output.png>     e.g. shot.sh "$SCRATCHPAD/playtest-01-buy-p1.png"
#
# Play mode only. Don't use capture_game_view's own save_path: it confines every path to Assets/, so the PNG gets
# imported as an asset (with a .meta). This asks for the image inline and decodes it instead.
set -euo pipefail

here="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
project="$(cd "$here/../../../../Rock Flipper" && pwd)"
dest="${1:?usage: shot.sh <output.png>}"
mkdir -p "$(dirname "$dest")"

unity command capture_game_view --source screen --project-path "$project" --json --no-banner \
  | python -c '
import base64, json, sys
d = json.loads(sys.stdin.read())
if not d.get("success"):
    for e in d.get("errors", []):
        print(e.get("message", e))
    sys.exit(1)
r = d["data"]["result"]
data = next((v for k, v in r.items() if isinstance(v, str) and len(v) > 1000), None)
if data is None:
    sys.exit("no inline image in the result; keys: " + ", ".join(r))
open(sys.argv[1], "wb").write(base64.b64decode(data.split(",", 1)[-1]))
print("%s (%sx%s)" % (sys.argv[1], r.get("width"), r.get("height")))
' "$(cygpath -w "$dest")"
