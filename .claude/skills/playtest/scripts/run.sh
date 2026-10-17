#!/usr/bin/env bash
# Runs one of this skill's C# snippets in the connected Unity Editor and prints its result.
#
# usage: run.sh <snippet> [NAME=VALUE ...]
#   <snippet>     a name from this folder (state, flip, ...) or a path to any .cs snippet
#   NAME=VALUE    replaces every {{NAME}} in the snippet before it runs
#
# Exit code: 0 on success, 1 if the snippet failed to compile or threw (the message is printed).
set -euo pipefail

here="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
project="$(cd "$here/../../../../Rock Flipper" && pwd)"

[[ $# -ge 1 ]] || { echo "usage: run.sh <snippet> [NAME=VALUE ...]" >&2; exit 2; }
src="$1"; shift
[[ -f "$src" ]] || src="$here/$src.cs"
[[ -f "$src" ]] || { echo "no snippet: $src" >&2; exit 2; }

tmp="$(mktemp --suffix .cs)"
trap 'rm -f "$tmp"' EXIT

# substitute {{NAME}} placeholders, and refuse to run with any left over
python - "$src" "$tmp" "$@" <<'PY'
import re, sys
src, dst, *pairs = sys.argv[1:]
code = open(src, encoding="utf-8").read()
for pair in pairs:
    name, _, value = pair.partition("=")
    code = code.replace("{{" + name + "}}", value)
left = sorted(set(re.findall(r"\{\{(\w+)\}\}", code)))
if left:
    sys.exit("missing values for: " + ", ".join(left))
open(dst, "w", encoding="utf-8").write(code)
PY

out="$(unity command eval_file --file "$(cygpath -w "$tmp")" --project-path "$project" --json --no-banner || true)"

python -c '
import json, sys
try:
    d = json.loads(sys.stdin.read())
except ValueError:
    sys.exit("unparseable output from unity command eval_file")
if not d.get("success"):
    for e in d.get("errors", []):
        print(e.get("message", e))
    sys.exit(1)
r = d["data"]["result"]
if r.get("output"):
    print(r["output"])
v = r.get("result")
print(v if isinstance(v, str) else json.dumps(v))
' <<<"$out"
