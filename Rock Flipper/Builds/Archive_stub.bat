@echo off
:: Stub for a variant folder: copy this file into Output\<Variant>\ (e.g. Output\Playtest\).
:: Packs, cleans and archives that folder's builds. The logic lives in Builds\Archive.bat.
call "%~dp0..\..\Archive.bat" "%~dp0."
