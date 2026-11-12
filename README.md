# Valheim mods

Valheim mods by Ben Hough (MrGlim). Each mod lives in its own folder with its own README, LICENSE and build files; loader: BepInEx 5 (Thunderstore) with Jötunn.

| Folder | Version | Description | Links |
|---|---|---|---|
| [GlymorsFeasts](GlymorsFeasts/) | 1.1.0 | A collection of placeable Valheim feasts. This release includes the Grizzled Platter and Serpent's Hollow. | [Thunderstore](https://thunderstore.io/c/valheim/p/MrGlim/GlymorsFeasts/) |

The standalone `MrGlim-GrizzledPlatter` and `MrGlim-SerpentsHollow` (also any older `SerpentBreadBowlFeast`) packages are superseded by [GlymorsFeasts](GlymorsFeasts/) and should be removed so the plugins do not both try to register the same feast prefabs.

Dishes in Glymor's Feasts 1.1.0:

- **Grizzled Platter** — pulled bear on the Black Forest board; Swamp Dweller's Delight stats (35/35, 3 regen, 50 min, 10 servings).
- **Serpent's Hollow** — Serpent Stew with bread bowls and Queen's Jam on the Plains picnic board; Sailor's Bounty stats (45/45, 3 regen, 50 min, 10 servings).

## Building and releasing

Each mod builds from its own folder (see that folder's README). Releases are tagged `<ModFolder>-vX.Y.Z`; see [RELEASING.md](RELEASING.md).

## AI disclosure

These mods were made with the help of generative AI (an AI coding agent directed by the author). Each mod's own README keeps its specific AI disclosure.

## License

MIT, see [LICENSE](LICENSE). Each mod folder also carries its own LICENSE and any third-party credits; the license covers the mods' own code and assets, not game assets or other authors' mods.
