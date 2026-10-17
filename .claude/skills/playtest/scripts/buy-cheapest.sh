#!/usr/bin/env bash
# Progression run: keeps buying the cheapest visible upgrade (Shop button or skill node) through the real UI buttons,
# while something else earns the cash (usually AutoMouse). Stops when nothing is buyable, when the next CASH upgrade
# needs >= MAX_WAIT gameplay seconds of income, when a purchase doesn't go through, or after MAX_STEPS polls.
#
# usage: buy-cheapest.sh [MAX_WAIT=60] [MAX_STEPS=200]
#
# Rules: a STAR upgrade is bought as soon as it's affordable (stars come from player levels, far apart); otherwise the
# target is the cheapest CASH upgrade. Income is measured per gameplay second (Time.time) since the last purchase, so
# the wait estimate means "at 1x speed" whatever the time scale. Each poll takes ~1-5 s real time; no sleeping needed.
# Output: one line per purchase, then a STOP line and the last upgrades.cs listing.
set -uo pipefail

here="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
max_wait="${1:-60}"; max_steps="${2:-200}"
run() { bash "$here/run.sh" "$@"; }

# field <listing-first-line> <NAME> -> value after NAME
field() { awk -v k="$2" '{for (i = 1; i < NF; i++) if ($i == k) print $(i + 1)}' <<<"$1"; }
ge() { python -c "import sys; sys.exit(0 if float(sys.argv[1]) >= float(sys.argv[2]) else 1)" "$1" "$2"; }

base_t=""; base_cash=""
for ((step = 0; step < max_steps; step++)); do
  out="$(run upgrades)" || { echo "STOP: upgrades.cs failed"; echo "$out"; exit 1; }
  head1="$(head -1 <<<"$out")"
  [[ "$head1" == T* ]] || { echo "STOP: $head1"; exit 1; }
  t="$(field "$head1" T)"; cash="$(field "$head1" CASH)"; star="$(field "$head1" STAR)"
  [[ -z "$base_t" ]] && { base_t="$t"; base_cash="$cash"; }

  target="$(grep '^BUY .* STAR affordable' <<<"$out" | head -1)"
  [[ -z "$target" ]] && target="$(grep '^BUY .* CASH ' <<<"$out" | head -1)"
  if [[ -z "$target" ]]; then
    if grep -q '^BUY .* STAR short' <<<"$out"; then
      echo "STOP: only STAR upgrades left and no stars (T=$t, level exp $(field "$head1" EXP))"
    else
      echo "STOP: nothing buyable (T=$t cash=$cash star=$star)"
    fi
    echo "$out"; exit 0
  fi
  read -r _ cost currency status _ <<<"$target"
  path="${target##*PATH=}"

  if [[ "$status" == affordable ]]; then
    balance="$cash"; [[ "$currency" == STAR ]] && balance="$star"
    if [[ "$path" != Shop/* ]]; then
      run click PATH="Side bar/Skill tree button" FORCE=false >/dev/null
      r="$(run click PATH="$path" FORCE=false)"
      run click PATH="SkillTree Screen/Wrapper/Back Button UI Variant" FORCE=false >/dev/null
    else
      r="$(run click PATH="$path" FORCE=false)"
    fi
    # "clicked" only means onClick ran: skill-node buttons stay interactable and ignore clicks they can't honor,
    # so confirm the item's level went up (or it's gone from the list because it maxed out)
    level_of() { grep -F "PATH=$path" <<<"$1" | grep -o ' L[0-9]*/' | head -1 | tr -dc '0-9'; }
    before_level="$(level_of "$out")"
    after_out="$(run upgrades)"
    after_level="$(level_of "$after_out")"
    if [[ -n "$after_level" && "$after_level" == "$before_level" ]]; then
      echo "STOP: bought nothing. T=$t clicked $path for $cost $currency (had $balance), still L$after_level ($r)"; echo "$after_out"; exit 1
    fi
    echo "T=$t BUY $cost $currency $path (L$before_level -> L${after_level:-max}, $currency before $balance)"
    base_t=""; base_cash=""
    continue
  fi

  eta="$(python -c "
import sys
t, bt, cash, bc, cost = map(float, sys.argv[1:])
dt, earned = t - bt, cash - bc
print('measuring' if dt < 10 or earned <= 0 else '%.0f %.2f' % ((cost - cash) / (earned / dt), earned / dt))" "$t" "$base_t" "$cash" "$base_cash" "$cost")"
  if [[ "$eta" != measuring ]]; then
    read -r secs rate <<<"$eta"
    if (( secs >= max_wait )); then
      echo "STOP: next is $path for $cost CASH, have $cash at T=$t; income $rate/gameplay-s -> ~${secs}s"
      echo "$out"; exit 0
    fi
  fi
done
echo "STOP: step limit $max_steps (T=$t cash=$cash)"; echo "$out"
