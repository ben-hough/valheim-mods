## 1.1.1

- Serpent's Hollow: bread no longer hovers. Every roll, jam half, bread bowl, and lid is dropped onto the real picnic-board mesh top at its position, or onto the pieces it is supposed to rest on, with a small tilt when that helps it settle. The 1.1.0 path snapped a loose bounding box to a flat deck height and then added a constant lift, which left gaps under the tilted and stacked rolls (visible from a low side angle). The approved layout is unchanged: round pot centered, jam crescent by the handle, jam in the built-in ramekins.
- Item drop and attach visuals copy the settled full-stage dish, so they match the placed feast.

## 1.1.0

- Adds **Serpent's Hollow** to the collection (the finished standalone Serpent's Hollow 1.1.6): a Plains-board feast of Serpent Stew with bread bowls and Queen's Jam. Stats match Sailor's Bounty (45/45 health and stamina, 3 regen, 50 minutes, 10 servings). Default recipe: 2 Serpent Stew, 3 Bread, 2 Queen's Jam, 1 Seafarer's Herbs.
- Prefab IDs stay `FeastSerpentBreadBowl` / `FeastSerpentBreadBowl_Material`, and item tokens are unchanged, so placed feasts from the standalone package keep working after you remove it.
- If the standalone Serpent's Hollow plugin (`com.benhough.valheim.SerpentBreadBowlFeast` / `SerpentBreadBowlFeast.dll`, package `MrGlim-SerpentsHollow`) is still loaded, Glymor's Feasts logs a warning and skips registering this dish. The Grizzled Platter still registers unless its own standalone package is also present.
- Keeps the standalone 1.1.6 look and fixes: vanilla feast stages (full / half / leftovers / bare), stew that lowers and bread that disappears at each stage, dish jam in the built-in ramekins, pot and bowl steam, Consumable item type, `m_foodItem` pointed at this mod's material, inherited `m_appendToolTip` cleared, drop / attach visuals using this dish, and clone-only visual edits.
- The standalone `MrGlim-SerpentsHollow` / `SerpentBreadBowlFeast` package is superseded and must be removed, the same way `MrGlim-GrizzledPlatter` was in 1.0.0. The Grizzled Platter itself is unchanged from 1.0.0.

## 1.0.0

- First release of **Glymor's Feasts**, a collection plugin (`com.benhough.valheim.GlymorsFeasts`, assembly `GlymorsFeasts.dll`) for placeable Valheim feasts.
- Includes the **Grizzled Platter**, formerly the standalone Grizzled Platter / Pulled Bear Feast 1.0.9 (`MrGlim-GrizzledPlatter`, GUID `com.benhough.valheim.PulledBearFeast`). The standalone package is superseded and must be removed to avoid duplicate prefabs.
- In-game name remains Grizzled Platter. Prefab IDs `FeastPulledBear` / `FeastPulledBear_Material` and item tokens are unchanged so existing worlds and saves keep working.
- Keeps the 1.0.9 standalone fixes: food item type forced to Consumable, `m_foodItem` pointed at this mod's material, interact collider covering the full board, inherited `m_appendToolTip` cleared so craft-panel stats are not doubled, and item drop / attach visuals using this platter.
- If the old standalone plugin is still loaded, Glymor's Feasts logs a warning and skips registering the Grizzled Platter.
- Default recipe, stats, servings, and steam behavior are unchanged from standalone 1.0.9 (35/35 health and stamina, 3 regen, 50 minutes, 10 servings; 3 Pulled Bear, 4 Carrot, 3 Turnip, 1 Thistle, 1 Woodland Herb Blend).