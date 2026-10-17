#!/usr/bin/env bash
# Backs up / restores / clears the Editor's save files, so a playtest doesn't leave its progress in the owner's saves.
#
# usage: saves.sh backup  <persistentDataPath>          copies every WritableData_*.FH into a new timestamped backup dir
#        saves.sh restore <persistentDataPath> <backup> puts the backup back exactly (files the playtest created are removed)
#        saves.sh fresh   <persistentDataPath>          deletes every WritableData_*.FH (first launch, empty slots); back up first
#        saves.sh list    <persistentDataPath>
#
# Only run restore/fresh while the Editor is NOT in Play mode: leaving Play mode saves (RunEntry.OnDestroy), which would
# overwrite whatever was just put back. Get <persistentDataPath> from preflight.cs; it changes with the version branch.
set -euo pipefail

cmd="${1:-}"; dir="${2:-}"
[[ -n "$cmd" && -n "$dir" ]] || { sed -n '4,8p' "$0" >&2; exit 2; }
[[ -d "$dir" ]] || { echo "no such dir: $dir" >&2; exit 2; }
backups="$dir/../playtest-backups/$(basename "$dir")"

case "$cmd" in
  backup)
    dest="$backups/$(date +%Y%m%d-%H%M%S)"
    mkdir -p "$dest"
    shopt -s nullglob
    files=("$dir"/WritableData_*.FH)
    (( ${#files[@]} )) && cp -p "${files[@]}" "$dest/"
    echo "$(cd "$dest" && pwd)"
    echo "backed up ${#files[@]} file(s)" >&2
    ;;
  restore)
    src="${3:-}"
    [[ -d "$src" ]] || { echo "no such backup: $src" >&2; exit 2; }
    rm -f "$dir"/WritableData_*.FH
    shopt -s nullglob
    files=("$src"/WritableData_*.FH)
    (( ${#files[@]} )) && cp -p "${files[@]}" "$dir/"
    echo "restored ${#files[@]} file(s) from $src"
    ;;
  fresh)
    rm -f "$dir"/WritableData_*.FH
    echo "cleared save files in $dir"
    ;;
  list)
    ls -la "$dir"/WritableData_*.FH 2>/dev/null || echo "no save files"
    ls -1 "$backups" 2>/dev/null | sed 's/^/backup: /' || true
    ;;
  *)
    echo "unknown command: $cmd" >&2; exit 2
    ;;
esac
