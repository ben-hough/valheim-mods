## 1.2.0

New dishes:

- Adds eight new feasts, all crafted at the Food Preparation Table and placed from the Serving Tray, Feasts tab:
  - **Hearthmorning Skillet**: fried eggs, sausage and hash. 55/55, 4 regen, with steam.
  - **Shieldmaiden's Spread**: eggs, honey, bread and berries on a kite shield. 35/55, 3 regen.
  - **Lox Benedict Board**: lox, poached eggs and honey glaze on toast. 55/45, 4 regen.
  - **Huntsman's Basket**: wolf jerky, eggs, bread and root hash. 40/40, 3 regen.
  - **The Amber Roost**: honey-glazed roast chicken with honey, carrots and bread. 70/50, 5 regen.
  - **Sea-God's Tray**: maki, nigiri and fish with a swimmer's mead. 40/60, 3 regen, plus Sea-God's Favor.
  - **Rán's Longsmoke**: smoked fish on a cedar rack. 45/50, 3 regen.
  - **Hall-Teinn**: wolf, neck, lox and boar skewers on flatbread. 60/45, 4 regen.
- All eight new dishes:
  - Last 50 minutes, have 10 servings, weigh 10, and stack to 5.
  - Show full, half, leftovers, and bare stages.
  - Have their own inventory icon.
  - Have their own config sections for stat multiplier, servings, and recipe.
- New buff, **Sea-God's Favor**, from the Sea-God's Tray. It lasts 5 minutes and reduces swim stamina use by up to half. It is based on the Draught of Vananidir effect, without its swim speed bonus.

Placement and visuals:

- The new dishes sit flush and level when placed. Each placement collider's bottom now matches the bottom of the visual, so the dish no longer floats about 65 mm above the surface (about 95 mm for the skillet).
- Shieldmaiden's Spread lies flat. The shield is no longer tilted or bowed, and its bowls and berries rest on it at every stage.
- The Amber Roost's loaves are set at varied angles instead of a rectangular grid.
- Dropped feasts show a correctly sized model of the full dish. Only the full stage is shown, scaled to match a vanilla feast drop.

Grizzled Platter and Serpent's Hollow:

- New inventory icons for both. The Grizzled Platter now has its own icon instead of the package icon.
- The Grizzled Platter now stacks to 5, like the other feasts. Before this, it inherited the vanilla stack size.

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