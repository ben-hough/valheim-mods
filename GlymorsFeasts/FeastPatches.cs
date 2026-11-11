using HarmonyLib;

namespace GlymorsFeasts
{
    [HarmonyPatch(typeof(Feast), "Start")]
    internal static class FeastStartPatch
    {
        private static void Postfix(Feast __instance)
        {
            if (__instance == null || Plugin.Dishes == null)
            {
                return;
            }
            foreach (IFeastDish dish in Plugin.Dishes)
            {
                if (dish != null && dish.TryEnsurePlacedFeast(__instance))
                {
                    return;
                }
            }
        }
    }
}