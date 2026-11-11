using BepInEx.Configuration;

namespace GlymorsFeasts
{
    internal static class GrizzledPlatterConfig
    {
        internal const string LegacyDefaultRecipeV1 = "PulledBear:3,Carrot:4,QueensJam:2,SpiceForests:1";
        internal const string LegacyDefaultRecipeV2 = "PulledBear:3,Carrot:4,Turnip:3,Raspberry:4,SpiceForests:1";
        internal const string DefaultRecipe = "PulledBear:3,Carrot:4,Turnip:3,Thistle:1,SpiceForests:1";

        internal static ConfigEntry<float> StatMultiplier;
        internal static ConfigEntry<string> Recipe;
        internal static ConfigEntry<bool> EnableSteam;
        internal static ConfigEntry<int> Servings;

        internal static void Bind(ConfigFile config)
        {
            StatMultiplier = config.Bind(
                "GrizzledPlatter - Food",
                "StatMultiplier",
                1.0f,
                "Multiplier applied to Swamp Dweller's Delight health, stamina, eitr and regen. 1.0 matches vanilla Swamp Dweller's Delight (35 / 35 / 0 / 3, 50 minutes, 10 servings). Restart required.");

            Recipe = config.Bind(
                "GrizzledPlatter - Crafting",
                "Recipe",
                DefaultRecipe,
                "Food Preparation Table ingredients as PrefabName:Amount pairs, comma-separated. Default is 3 Pulled Bear, 4 Carrot, 3 Turnip, 1 Thistle, 1 Woodland Herb Blend. Restart required.");

            if (Recipe.Value == LegacyDefaultRecipeV1 || Recipe.Value == LegacyDefaultRecipeV2)
            {
                Recipe.Value = DefaultRecipe;
            }

            Servings = config.Bind(
                "GrizzledPlatter - Food",
                "Servings",
                10,
                "Number of servings on a placed feast. Vanilla feasts use 10. Restart required.");

            EnableSteam = config.Bind(
                "GrizzledPlatter - Visuals",
                "EnableSteam",
                true,
                "Use the same fx_FoodSteam the vanilla feasts use, visible while at least one serving remains. Restart required.");
        }
    }
}