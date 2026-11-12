using BepInEx.Configuration;

namespace GlymorsFeasts
{
    internal static class SerpentsHollowConfig
    {
        internal const string DefaultRecipe = "SerpentStew:2,Bread:3,QueensJam:2,SpiceOceans:1";

        internal static ConfigEntry<float> StatMultiplier;
        internal static ConfigEntry<string> Recipe;
        internal static ConfigEntry<bool> EnableSteam;
        internal static ConfigEntry<int> Servings;

        internal static void Bind(ConfigFile config)
        {
            StatMultiplier = config.Bind(
                "SerpentsHollow - Food",
                "StatMultiplier",
                1.0f,
                "Multiplier applied to Sailor's Bounty (FeastOceans) health, stamina, eitr and regen. 1.0 matches vanilla Sailor's Bounty (45 / 45 / 0 / 3, 50 minutes, 10 servings). Restart required.");

            Recipe = config.Bind(
                "SerpentsHollow - Crafting",
                "Recipe",
                DefaultRecipe,
                "Food Preparation Table ingredients as PrefabName:Amount pairs, comma-separated. Default is 2 Serpent Stew, 3 Bread, 2 Queen's Jam, 1 Seafarer's Herbs. Restart required.");

            Servings = config.Bind(
                "SerpentsHollow - Food",
                "Servings",
                10,
                "Number of servings on a placed feast. Vanilla feasts use 10. Restart required.");

            EnableSteam = config.Bind(
                "SerpentsHollow - Visuals",
                "EnableSteam",
                true,
                "Use the same fx_FoodSteam the vanilla feasts use, rising from the pot and the filled bread bowl while servings remain. Restart required.");
        }
    }
}
