# Glymor's Feasts

A collection of placeable Valheim feasts by Ben Hough (MrGlim). Each dish is crafted at the Food Preparation Table, set out with the Serving Tray, and eaten by anyone at the table until the servings are gone.

**This 1.0.0 release includes one dish: the Grizzled Platter** (the finished standalone Grizzled Platter / Pulled Bear Feast 1.0.9, now part of this collection). Further dishes will be added in later versions when they are ready.

## Standalone package (read this if you used Grizzled Platter before)

The standalone Thunderstore package **MrGlim-GrizzledPlatter** (plugin GUID `com.benhough.valheim.PulledBearFeast`, assembly `PulledBearFeast.dll`) is superseded by this mod and **must be removed** from your profile. If both plugins load, they would try to register the same prefabs (`FeastPulledBear` / `FeastPulledBear_Material`). Glymor's Feasts detects the old GUID, logs a warning, and skips registering the Grizzled Platter so existing worlds do not clash. You will not get the collection plugin's dish until the standalone package is removed and the game is restarted.

Prefab names, localization token keys, and the in-game name **Grizzled Platter** are unchanged, so platters already placed in a world keep working.

The old config file was `BepInEx/config/com.benhough.valheim.PulledBearFeast.cfg`. This plugin uses `BepInEx/config/com.benhough.valheim.GlymorsFeasts.cfg`. Re-apply any custom recipe, stat, servings, or steam settings there if you had changed them.

## Features

- A placeable, steaming **Grizzled Platter** of shredded pulled bear with roasted carrot and turnip chunks and woodland herb sprigs, on the Black Forest buffet board.
- Food stats matched to vanilla **Swamp Dweller's Delight**.
- Ten servings, with the same visual depletion stages as vanilla feasts (full, half, leftovers, then the bare board).
- Crafted at the Food Preparation Table and placed from the Serving Tray, Feasts tab.
- Vanilla feast steam (`fx_FoodSteam`) while at least one serving remains.
- Configurable recipe, stat multiplier, servings, and steam.

## Food stats (Grizzled Platter)

Same as Swamp Dweller's Delight (`FeastSwamps_Material`) at the default multiplier of 1.0:

| Health | Stamina | Eitr | Regen | Duration | Servings | Weight | Stack |
| --- | --- | --- | --- | --- | --- | --- | --- |
| 35 | 35 | 0 | 3 hp/tick | 50 min | 10 | 10 | 5 |

Pulled Bear itself is a vanilla cauldron dish (37 health, 16 stamina, 3 regen, 30 min) and is not a feast in vanilla. This mod adds the feast version and keeps it on the same stat tier as Swamp Dweller's Delight and the Black Forest Buffet Platter.

## Recipe (Grizzled Platter)

Crafted at the **Food Preparation Table**:

- 3× Pulled Bear
- 4× Carrot
- 3× Turnip
- 1× Thistle
- 1× Woodland Herb Blend (`SpiceForests`)

Place it from the Serving Tray, Feasts tab, like any other feast. The hover prompt and empty platter pickup use the name **Grizzled Platter**. The empty platter can be removed with the Serving Tray after the servings are gone.

The Food Preparation Table ingredient row pages, so the fifth ingredient (Woodland Herb Blend) appears on the next page of the ingredient list. That is vanilla UI behavior, not a missing ingredient.

## How it works

The placed piece uses Valheim's vanilla `Feast` component: servings are stored on the piece's ZDO, eat requests go through the usual feast RPCs, and the food visual is parented under the board's `New` transform at the same pose as other feasts. The wooden board stays visible at every serving stage. Steam is the vanilla `fx_FoodSteam` effect, looping while at least one serving remains and shutting off when the platter is empty.

A few fixes from the standalone 1.0.9 release are kept here:

- The food item's `m_itemType` is forced to `Consumable`, and `m_foodItem` always points at this mod's material item, so `Humanoid.CanConsumeItem` can succeed when you use Eat.
- The interact collider covers the full board (including the meat bed).
- The cloned vanilla feast's `m_appendToolTip` is cleared so health, stamina, duration, and healing are not printed twice in the craft panel tooltip.
- The item drop and item-stand / attach visual use this platter, not the cloned Swamp Dweller's board.

Internal prefab IDs remain `FeastPulledBear` and `FeastPulledBear_Material`. Localization tokens remain `$item_feastpulledbear` and `$item_feastpulledbear_desc`.

## Requirements

- Valheim (current live version)
- [BepInExPack Valheim](https://thunderstore.io/c/valheim/p/denikson/BepInExPack_Valheim/) 5.4.2360 or later
- [Jötunn](https://thunderstore.io/c/valheim/p/ValheimModding/Jotunn/) 2.30.2 or later

## Install

1. Install BepInExPack Valheim and Jötunn in your profile (a Thunderstore Mod Manager or r2modman profile is the usual way).
2. **Remove** any standalone `MrGlim-GrizzledPlatter` or older `PulledBearFeast` plugin folder so the old GUID is not loaded.
3. Drop this package's folder (or just `GlymorsFeasts.dll` plus `icon.png`) into the profile's `BepInEx/plugins` folder.
4. Launch the game through that profile.

## Multiplayer

**Every player, including the host and every client, needs this mod.** It adds custom item and piece prefabs. Players without it will not have the Grizzled Platter (or will see a missing prefab for platters already placed in the world).

Config is read locally when the plugin loads and requires a restart to apply. This version does not sync config from the host to clients. If you change the recipe, stats, or servings, use the same values on every player who should see the same numbers.

## Config

`BepInEx/config/com.benhough.valheim.GlymorsFeasts.cfg` (restart after changes):

| Section | Key | Default | Notes |
| --- | --- | --- | --- |
| GrizzledPlatter - Food | `StatMultiplier` | 1.0 | Applied to health, stamina, eitr and regen copied from Swamp Dweller's Delight |
| GrizzledPlatter - Food | `Servings` | 10 | Portions on a placed feast |
| GrizzledPlatter - Crafting | `Recipe` | `PulledBear:3,Carrot:4,Turnip:3,Thistle:1,SpiceForests:1` | `PrefabName:Amount` pairs, comma-separated |
| GrizzledPlatter - Visuals | `EnableSteam` | true | Vanilla feast steam (`fx_FoodSteam`) while the platter still has food |

If a leftover standalone config still has an earlier default recipe (Queen's Jam or 4 Raspberries) and you paste it into this file, those old defaults are migrated to the Thistle recipe on the next launch. Custom recipes are left alone.

## Known issues

- The Food Preparation Table shows five recipe ingredients across two pages. That is the vanilla craft-panel layout.
- If the retired standalone Grizzled Platter plugin is still installed, this mod skips the platter and logs a warning. Remove the standalone package to use the dish from Glymor's Feasts.
- Changing recipe or stats while a world is running requires a restart; already-placed platters keep the servings stored in that piece's ZDO.

## Credits

- Valheim is by Iron Gate. This mod does not include game files.
- [BepInEx](https://thunderstore.io/c/valheim/p/denikson/BepInExPack_Valheim/) and [Jötunn](https://thunderstore.io/c/valheim/p/ValheimModding/Jotunn/) are required libraries.
- Pulled Bear, Swamp Dweller's Delight, and the Black Forest buffet board are vanilla content; this mod adds a feast version of Pulled Bear and uses those as stat and visual references.

## Source

Source, issues, and releases: [github.com/ben-hough/valheim-mods/tree/main/GlymorsFeasts](https://github.com/ben-hough/valheim-mods/tree/main/GlymorsFeasts)

## AI disclosure

This mod was made with the help of generative AI. The code and this README were produced with an AI coding agent, directed by the author (MrGlim / Ben Hough). The assembly records `AI_Assisted_Creation` and `AI_Model_Vendor` metadata. The Thunderstore package is listed in the **AI Generated** category.

## License

MIT, see [LICENSE](LICENSE). The license covers this mod's own code and assets, not Valheim game assets or other authors' mods.