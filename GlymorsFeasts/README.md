# Glymor's Feasts

A collection of placeable Valheim feasts by Ben Hough (MrGlim). Each dish is crafted at the Food Preparation Table, set out with the Serving Tray, and eaten by anyone at the table until the servings are gone.

**This 1.1.0 release includes two dishes:** the **Grizzled Platter** and **Serpent's Hollow**. Further dishes will be added in later versions when they are ready.

## Standalone packages (read this if you used either dish before)

Glymor's Feasts supersedes the standalone Thunderstore packages. **Remove them from your profile** so the same prefabs are not registered twice:

- **MrGlim-GrizzledPlatter** (plugin GUID `com.benhough.valheim.PulledBearFeast`, assembly `PulledBearFeast.dll`). Prefabs: `FeastPulledBear` / `FeastPulledBear_Material`.
- **MrGlim-SerpentsHollow** (plugin GUID `com.benhough.valheim.SerpentBreadBowlFeast`, assembly `SerpentBreadBowlFeast.dll`; also any older `SerpentBreadBowlFeast` folder). Prefabs: `FeastSerpentBreadBowl` / `FeastSerpentBreadBowl_Material`.

If a standalone plugin is still loaded, Glymor's Feasts detects that GUID, logs a warning, and skips registering that dish so existing worlds do not clash. You will not get that dish from the collection until the standalone package is removed and the game is restarted. The other dish still registers normally.

Prefab names, localization token keys, and the in-game names **Grizzled Platter** and **Serpent's Hollow** are unchanged, so feasts already placed in a world keep working after you switch to this package.

The old config files were `BepInEx/config/com.benhough.valheim.PulledBearFeast.cfg` and `BepInEx/config/com.benhough.valheim.SerpentBreadBowlFeast.cfg`. This plugin uses `BepInEx/config/com.benhough.valheim.GlymorsFeasts.cfg`, with a section per dish. Re-apply any custom recipe, stat, servings, or steam settings there if you had changed them.

## Features

- Two placeable, steaming feasts on the Serving Tray, Feasts tab.
- Ten servings each, with the same visual depletion stages as vanilla feasts (full at 70% or more of servings remaining, half between 40% and 70%, leftovers between 10% and 40%, then the bare board below 10%). With the default 10 servings, the look changes at 6, 3, and 0 remaining — eating one portion from a full platter (10 → 9) stays at the full stage, which is vanilla feast behavior.
- Crafted at the Food Preparation Table.
- Vanilla feast steam (`fx_FoodSteam`) while servings remain.
- Configurable recipe, stat multiplier, servings, and steam per dish.
- Eat, drop, and item-stand visuals use this mod's dishes, not the cloned vanilla boards.

### Grizzled Platter

A platter of shredded pulled bear with roasted carrot and turnip chunks and woodland herb sprigs, on the Black Forest buffet board. Food stats match vanilla **Swamp Dweller's Delight**. Steam loops while at least one serving remains and shuts off when the platter is empty. The wooden board stays visible at every stage.

### Serpent's Hollow

A slightly wider pot of Serpent Stew in the center of the vanilla Plains picnic board, with meat chunks and a ladle, jam-topped halved rolls by the handle, Queen's Jam filling the three small dishes built into the board, and whole rolls plus a stew-filled bread bowl and a hollowed bowl with its lid on the other side. Food stats match vanilla **Sailor's Bounty**. The stew lowers and the bread disappears at each stage; the ladle is on the full and half stages only. Steam rises from the pot while at least one serving remains, and from the filled bread bowl while that bowl is still on the tray. The board, empty pot, and dish jam stay visible on the bare board.

## Food stats

### Grizzled Platter

Same as Swamp Dweller's Delight (`FeastSwamps_Material`) at the default multiplier of 1.0:

| Health | Stamina | Eitr | Regen | Duration | Servings | Weight | Stack |
| --- | --- | --- | --- | --- | --- | --- | --- |
| 35 | 35 | 0 | 3 hp/tick | 50 min | 10 | 10 | 5 |

Pulled Bear itself is a vanilla cauldron dish (37 health, 16 stamina, 3 regen, 30 min) and is not a feast in vanilla. This mod adds the feast version and keeps it on the same stat tier as Swamp Dweller's Delight and the Black Forest Buffet Platter.

### Serpent's Hollow

Same as Sailor's Bounty (`FeastOceans_Material`, sometimes called the Seafarer's Herring Platter) at the default multiplier of 1.0. The live values are read from the hidden Consumable referenced by `FeastOceans_Material.m_appendToolTip` (the craftable material itself has `m_food = 0`):

| Health | Stamina | Eitr | Regen | Duration | Servings | Weight | Stack |
| --- | --- | --- | --- | --- | --- | --- | --- |
| 45 | 45 | 0 | 3 hp/tick | 50 min | 10 | 10 | 5 |

## Recipes

Both dishes are crafted at the **Food Preparation Table** and placed from the Serving Tray, Feasts tab. The empty platter can be removed with the Serving Tray after the servings are gone. The hover prompt and empty pickup use each dish's in-game name.

### Grizzled Platter

- 3× Pulled Bear
- 4× Carrot
- 3× Turnip
- 1× Thistle
- 1× Woodland Herb Blend (`SpiceForests`)

The Food Preparation Table ingredient row pages, so the fifth ingredient (Woodland Herb Blend) appears on the next page of the ingredient list. That is vanilla UI behavior, not a missing ingredient.

### Serpent's Hollow

- 2× Serpent Stew
- 3× Bread
- 2× Queen's Jam
- 1× Seafarer's Herbs (`SpiceOceans`)

This is meant to be comparable in cost to Sailor's Bounty (which uses 2 cooked serpent meat, 5 cooked fish, 4 thistle and 1 Seafarer's Herbs). Two stews are the same two cooked serpent meat, plus the stew's mushroom and honey; three loaves cover the bread-bowl presentation; two jars of Queen's Jam cover the dish fills and the jam slices.

## How it works

Each placed piece uses Valheim's vanilla `Feast` component: servings are stored on the piece's ZDO, eat requests go through the usual feast RPCs, and the food visual is parented under the board's `New` transform at the same pose as other feasts. Steam is the vanilla `fx_FoodSteam` effect.

Fixes kept from the standalone releases:

- The food item's `m_itemType` is forced to `Consumable`, and `m_foodItem` always points at this mod's material item, so `Humanoid.CanConsumeItem` can succeed when you use Eat.
- The interact collider covers the full board (including the food).
- The cloned vanilla feast's `m_appendToolTip` is cleared so health, stamina, duration, and healing are not printed twice in the craft panel tooltip.
- The item drop and item-stand / attach visual use this dish, not the cloned vanilla board.
- Visual edits are made on clones only, so vanilla feast prefabs are not leaked or mutated.

Internal prefab IDs remain `FeastPulledBear` / `FeastPulledBear_Material` and `FeastSerpentBreadBowl` / `FeastSerpentBreadBowl_Material`. Localization tokens remain `$item_feastpulledbear` / `$item_feastpulledbear_desc` and `$item_feastserpentbreadbowl` / `$item_feastserpentbreadbowl_desc`.

## Requirements

- Valheim (current live version)
- [BepInExPack Valheim](https://thunderstore.io/c/valheim/p/denikson/BepInExPack_Valheim/) 5.4.2360 or later
- [Jötunn](https://thunderstore.io/c/valheim/p/ValheimModding/Jotunn/) 2.30.2 or later

## Install

1. Install BepInExPack Valheim and Jötunn in your profile (a Thunderstore Mod Manager or r2modman profile is the usual way).
2. **Remove** any standalone `MrGlim-GrizzledPlatter`, older `PulledBearFeast`, `MrGlim-SerpentsHollow`, or `SerpentBreadBowlFeast` plugin folder so those old GUIDs are not loaded.
3. Drop this package's folder (or just `GlymorsFeasts.dll` plus `icon.png`) into the profile's `BepInEx/plugins` folder.
4. Launch the game through that profile.

## Multiplayer

**Every player, including the host and every client, needs this mod.** It adds custom item and piece prefabs. Players without it will not have these dishes (or will see a missing prefab for feasts already placed in the world).

Config is read locally when the plugin loads and requires a restart to apply. This version does not sync config from the host to clients. If you change the recipe, stats, or servings, use the same values on every player who should see the same numbers.

## Config

`BepInEx/config/com.benhough.valheim.GlymorsFeasts.cfg` (restart after changes):

| Section | Key | Default | Notes |
| --- | --- | --- | --- |
| GrizzledPlatter - Food | `StatMultiplier` | 1.0 | Applied to health, stamina, eitr and regen copied from Swamp Dweller's Delight |
| GrizzledPlatter - Food | `Servings` | 10 | Portions on a placed feast |
| GrizzledPlatter - Crafting | `Recipe` | `PulledBear:3,Carrot:4,Turnip:3,Thistle:1,SpiceForests:1` | `PrefabName:Amount` pairs, comma-separated |
| GrizzledPlatter - Visuals | `EnableSteam` | true | Vanilla feast steam (`fx_FoodSteam`) while the platter still has food |
| SerpentsHollow - Food | `StatMultiplier` | 1.0 | Applied to health, stamina, eitr and regen copied from Sailor's Bounty |
| SerpentsHollow - Food | `Servings` | 10 | Portions on a placed feast |
| SerpentsHollow - Crafting | `Recipe` | `SerpentStew:2,Bread:3,QueensJam:2,SpiceOceans:1` | `PrefabName:Amount` pairs, comma-separated |
| SerpentsHollow - Visuals | `EnableSteam` | true | Vanilla feast steam (`fx_FoodSteam`) from the pot and the filled bowl |

If a leftover standalone Grizzled Platter config still has an earlier default recipe (Queen's Jam or 4 Raspberries) and you paste it into this file, those old defaults are migrated to the Thistle recipe on the next launch. Custom recipes are left alone.

## Compatibility

- Remove the standalone Grizzled Platter and Serpent's Hollow packages before running this mod, as described above.
- Prefab IDs are stable, so worlds that already have either dish placed keep those pieces when you migrate to Glymor's Feasts 1.1.0.
- Both dishes use vanilla feast serving sync (ZDO `s_value` and `RPC_OnEat`). Clients need the same prefabs to see the custom visuals.
- This plugin does not change vanilla feasts. Visual work is done on clones of the Black Forest and Plains boards.

## Known issues

- The Food Preparation Table shows five recipe ingredients across two pages. That is the vanilla craft-panel layout.
- If a retired standalone plugin is still installed, this mod skips that dish and logs a warning. Remove the standalone package to use the dish from Glymor's Feasts.
- Changing recipe or stats while a world is running requires a restart; already-placed feasts keep the servings stored in that piece's ZDO.

## Credits

- Valheim is by Iron Gate. This mod does not include game files.
- [BepInEx](https://thunderstore.io/c/valheim/p/denikson/BepInExPack_Valheim/) and [Jötunn](https://thunderstore.io/c/valheim/p/ValheimModding/Jotunn/) are required libraries.
- Pulled Bear, Swamp Dweller's Delight, the Black Forest buffet board, Serpent Stew, Sailor's Bounty, and the Plains picnic board are vanilla content; this mod adds feast versions and uses those as stat and visual references.

## Source

Source, issues, and releases: [github.com/ben-hough/valheim-mods/tree/main/GlymorsFeasts](https://github.com/ben-hough/valheim-mods/tree/main/GlymorsFeasts)

## AI disclosure

This mod was made with the help of generative AI. The code and this README were produced with an AI coding agent, directed by the author (MrGlim / Ben Hough). The assembly records `AI_Assisted_Creation` and `AI_Model_Vendor` metadata. The Thunderstore package is listed in the **AI Generated** category.

## License

MIT, see [LICENSE](LICENSE). The license covers this mod's own code and assets, not Valheim game assets or other authors' mods.
