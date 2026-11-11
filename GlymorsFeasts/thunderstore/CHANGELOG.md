## 1.0.0

- First release of **Glymor's Feasts**, a collection plugin (`com.benhough.valheim.GlymorsFeasts`, assembly `GlymorsFeasts.dll`) for placeable Valheim feasts.
- Includes the **Grizzled Platter**, formerly the standalone Grizzled Platter / Pulled Bear Feast 1.0.9 (`MrGlim-GrizzledPlatter`, GUID `com.benhough.valheim.PulledBearFeast`). The standalone package is superseded and must be removed to avoid duplicate prefabs.
- In-game name remains Grizzled Platter. Prefab IDs `FeastPulledBear` / `FeastPulledBear_Material` and item tokens are unchanged so existing worlds and saves keep working.
- Keeps the 1.0.9 standalone fixes: food item type forced to Consumable, `m_foodItem` pointed at this mod's material, interact collider covering the full board, inherited `m_appendToolTip` cleared so craft-panel stats are not doubled, and item drop / attach visuals using this platter.
- If the old standalone plugin is still loaded, Glymor's Feasts logs a warning and skips registering the Grizzled Platter.
- Default recipe, stats, servings, and steam behavior are unchanged from standalone 1.0.9 (35/35 health and stamina, 3 regen, 50 minutes, 10 servings; 3 Pulled Bear, 4 Carrot, 3 Turnip, 1 Thistle, 1 Woodland Herb Blend).