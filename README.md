# Valheim mods

Valheim mods by Ben Hough (MrGlim). Each mod lives in its own folder with its own README, LICENSE and build files; loader: BepInEx 5 (Thunderstore) with Jötunn.

| Folder | Version | Description | Links |
|---|---|---|---|
| [GlymorsFeasts](GlymorsFeasts/) | 1.2.0 | A collection of placeable Valheim feasts. This release has ten dishes. | [Thunderstore](https://thunderstore.io/c/valheim/p/MrGlim/GlymorsFeasts/) |

The standalone `MrGlim-GrizzledPlatter` and `MrGlim-SerpentsHollow` (also any older `SerpentBreadBowlFeast`) packages are superseded by [GlymorsFeasts](GlymorsFeasts/) and should be removed so the plugins do not both try to register the same feast prefabs.

Dishes in Glymor's Feasts 1.2.0:

- **Grizzled Platter**: pulled bear on the Black Forest board; Swamp Dweller's Delight stats (35/35, 3 regen, 50 min, 10 servings).
- **Serpent's Hollow**: Serpent Stew with bread bowls and Queen's Jam on the Plains picnic board; Sailor's Bounty stats (45/45, 3 regen, 50 min, 10 servings).
- **Hearthmorning Skillet**: fried eggs, sausage and hash in a steaming skillet (55/55, 4 regen).
- **Shieldmaiden's Spread**: eggs, honey, bread and berries on a kite shield (35/55, 3 regen).
- **Lox Benedict Board**: lox and poached eggs on honey-glazed toast (55/45, 4 regen).
- **Huntsman's Basket**: wolf jerky, eggs, bread and root hash (40/40, 3 regen).
- **The Amber Roost**: honey-glazed roast chicken with honey, carrots and bread (70/50, 5 regen).
- **Sea-God's Tray**: maki, nigiri and fish with a swimmer's mead; grants Sea-God's Favor, which reduces swim stamina use (40/60, 3 regen).
- **Rán's Longsmoke**: smoked fish on a cedar rack (45/50, 3 regen).
- **Hall-Teinn**: wolf, neck, lox and boar skewers on flatbread (60/45, 4 regen).

The new dishes all last 50 minutes, have 10 servings, and stack to 5.

## Building and releasing

Each mod builds from its own folder (see that folder's README). Releases are tagged `<ModFolder>-vX.Y.Z`; see [RELEASING.md](RELEASING.md).

## AI disclosure

These mods were made with the help of generative AI (an AI coding agent directed by the author). Each mod's own README keeps its specific AI disclosure.

## License

MIT, see [LICENSE](LICENSE). Each mod folder also carries its own LICENSE and any third-party credits; the license covers the mods' own code and assets, not game assets or other authors' mods.
