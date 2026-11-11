# Releasing

This repository holds several mods, one per top-level folder. Each mod is built, versioned and released independently.

## Build

Build a mod from its own folder, e.g. `cd GlymorsFeasts && ./build.ps1` (or `dotnet build -c Release` after game / BepInEx / Jötunn refs are in that folder's `refs/` directory; see the mod's README). Build output (`bin/`, `obj/`, `*.dll`, `refs/`) and release zips are git-ignored and must never be committed. Do not commit Valheim game DLLs, tokens, or local machine paths.

## Tags

Release tags use the scheme `<ModFolder>-vX.Y.Z`, for example `GlymorsFeasts-v1.0.0`. The folder name is the mod's project name exactly as it appears at the repository root.

```
git tag -a GlymorsFeasts-v1.0.0 -m "GlymorsFeasts 1.0.0"
git push origin GlymorsFeasts-v1.0.0
```

Bump the version in the mod's own manifest / project files first, commit, then tag that commit. A GitHub release, if made, uses the same tag name and attaches that mod's Thunderstore package only.

Thunderstore uploads are done separately from the GitHub release and should only happen after in-game testing.