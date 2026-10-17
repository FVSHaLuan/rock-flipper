"""Pacing report for a buy-cheapest.sh log (playtest skill), checked against the demo's pacing targets.

usage: python pace.py <log file> [--early-end MIN] [--late-start MIN]

Reads the purchase lines buy-cheapest.sh prints:
    T=<gameplay s> BUY <cost> <CASH|STAR> <path> (L<a> -> L<b|max>, ...)
Several logs (one per buy-cheapest call in the same Play session) can be concatenated into one file: T is
gameplay time since Play started, so it keeps counting across calls.

A purchase is "major" when it takes something from level 0 (an unlock, or the first level of an upgrade).
Phases: early = before --early-end minutes (default 15), late = from --late-start minutes (default 45), mid between.
Targets (median gap between purchases, gameplay seconds): early 8-20, mid 20-60, late 40-150; total ~60 min.
"""
import re
import statistics
import sys

TARGETS = {"early": (8, 20), "mid": (20, 60), "late": (40, 150)}
LINE = re.compile(r"^T=([\d.]+) BUY ([\d.]+) (CASH|STAR) (.+?) \(L(\d*) -> L(\w*)")


def main():
    args = sys.argv[1:]
    if not args:
        sys.exit(__doc__)
    early_end, late_start = 15.0, 45.0
    if "--early-end" in args:
        i = args.index("--early-end"); early_end = float(args[i + 1]); del args[i:i + 2]
    if "--late-start" in args:
        i = args.index("--late-start"); late_start = float(args[i + 1]); del args[i:i + 2]

    buys = []
    with open(args[0], encoding="utf-8") as f:
        for line in f:
            m = LINE.match(line.strip())
            if m:
                t, cost, cur, path, before, _after = m.groups()
                name = path.rsplit("/", 1)[-1] if not path.startswith("Shop/") else path.split("/")[1]
                buys.append((float(t), float(cost), cur, name, before == "0"))
    if not buys:
        sys.exit("no purchase lines found")
    buys.sort()

    def phase(t):
        return "early" if t < early_end * 60 else "late" if t >= late_start * 60 else "mid"

    print(f"{'T (m:s)':>8} {'gap s':>6}  {'phase':5}  purchase")
    gaps = {"early": [], "mid": [], "late": []}
    majors = []
    prev = None
    for t, cost, cur, name, major in buys:
        gap = t - prev if prev is not None else t
        prev = t
        ph = phase(t)
        gaps[ph].append(gap)
        lo, hi = TARGETS[ph]
        flag = "  << long" if gap > hi * 2 else ""
        if major:
            majors.append(t)
        print(f"{int(t // 60):>5}:{int(t % 60):02d} {gap:6.0f}  {ph:5}  {'*' if major else ' '} {name} {cost:.0f} {cur}{flag}")

    last = buys[-1][0]
    print(f"\n{len(buys)} purchases ({len(majors)} major, marked *) over {last / 60:.1f} gameplay min (target ~60)")
    for ph in ("early", "mid", "late"):
        g = gaps[ph]
        lo, hi = TARGETS[ph]
        if not g:
            print(f"  {ph:5}: no purchases")
            continue
        med = statistics.median(g)
        verdict = "OK" if lo <= med <= hi else "TOO FAST" if med < lo else "TOO SLOW"
        print(f"  {ph:5}: {len(g):3} buys, median gap {med:5.0f}s, max {max(g):5.0f}s  (target {lo}-{hi}s)  {verdict}")
    if len(majors) > 1:
        mg = [b - a for a, b in zip(majors, majors[1:])]
        print(f"  major upgrades: median gap {statistics.median(mg):.0f}s, longest {max(mg):.0f}s")


if __name__ == "__main__":
    main()
