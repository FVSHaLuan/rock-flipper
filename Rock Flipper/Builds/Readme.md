# Builds

Unity builds go into `Output\<Variant>\<Platform>\`, for example `Output\Demo\Windows\`. The variants are `Demo`, `Release` and `Playtest`, and the platforms are `Windows`, `Linux` and `Mac`.

`Archive.bat` turns a variant's builds into zips, archives them, and clears the platform folders for the next build.

## Files

| File | What it is |
| --- | --- |
| `Archive.bat` | The main script. All the logic lives here. |
| `Archive_stub.bat` | Template for a variant folder. It calls `..\..\Archive.bat` with its own folder. |
| `Output\<Variant>\Archive.bat` | A copy of the stub. Double-click it to archive that variant. |

Because each variant's stub only calls the main script, changes to `Archive.bat` apply to every variant without recopying anything.

## Archiving a variant

Double-click `Output\<Variant>\Archive.bat`. The script then does the following:

1. **Checks the platforms.** A platform folder that is missing or has no files counts as missing. If any are missing, it lists them and asks whether to skip them (`S`) or abort (`A`, the default).
2. **Reads the game name and version** from the `title` and `version` lines of `buildCfg.ini`, which Unity writes after each build. It uses the first file it finds, trying `Windows`, then `Linux`, then `Mac\Mac.app` (where Unity puts the Mac one), then `Mac`. If either line is missing, it stops without changing anything.
3. **Asks for confirmation** (`Y`/`N`, default `N`) after showing the game name, the version and the platforms it will pack.
4. **Packs** each platform with WinRAR into `<title> - <Platform> - <version> - release.zip`. Files and folders whose names contain `DoNotShip` or `DontShip` are left out, including Unity's `..._BackUpThisFolder_ButDontShipItWithYourGame` folder.
5. **Deletes the builds** and recreates empty `Linux`, `Mac` (with `Mac\Mac.app`) and `Windows` folders. If any pack fails, it stops before this step and nothing is deleted.
6. **Archives** every `.zip` in the variant folder into `Output\<Variant>\Archives\<version>\`. If that folder already exists, it uses `<version>_1`, `<version>_2` and so on.

## Adding a variant

Copy `Archive_stub.bat` into the new variant folder, e.g. `Output\Playtest\`, and rename it to `Archive.bat` if you like. The stub only works two levels below `Builds\`, so don't run it from `Builds\` itself.

## Settings

These are at the top of `Archive.bat`:

- `archiveRoot`: the archive folder's name inside each variant (`Archives`).
- `dupFormat`: the suffix added when a version folder already exists. `#` is replaced by the number. The default is `_#`.

## Requirements

WinRAR must be installed at `C:\Program Files\WinRAR\WinRAR.exe`.

`Output\` is gitignored, so the stubs inside it aren't tracked. If the folder is ever wiped, copy `Archive_stub.bat` back in.
