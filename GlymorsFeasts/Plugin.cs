using System.IO;
using System.Reflection;
using BepInEx;
using HarmonyLib;
using Jotunn.Entities;
using Jotunn.Managers;
using UnityEngine;

namespace GlymorsFeasts
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid, BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency(LegacyPulledBearFeastGuid, BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency(LegacySerpentBreadBowlFeastGuid, BepInDependency.DependencyFlags.SoftDependency)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "com.benhough.valheim.GlymorsFeasts";
        public const string PluginName = "Glymor's Feasts";
        public const string PluginVersion = "1.1.1";

        /// <summary>
        /// GUID of the retired standalone Grizzled Platter / Pulled Bear Feast plugin.
        /// Soft-dependency so this plugin loads after it when both are installed.
        /// </summary>
        public const string LegacyPulledBearFeastGuid = "com.benhough.valheim.PulledBearFeast";
        public const string LegacyPulledBearFeastAssembly = "PulledBearFeast";

        /// <summary>
        /// GUID of the retired standalone Serpent's Hollow / SerpentBreadBowlFeast plugin.
        /// Soft-dependency so this plugin loads after it when both are installed.
        /// </summary>
        public const string LegacySerpentBreadBowlFeastGuid = "com.benhough.valheim.SerpentBreadBowlFeast";
        public const string LegacySerpentBreadBowlFeastAssembly = "SerpentBreadBowlFeast";

        internal static Plugin Instance { get; private set; }
        internal static BepInEx.Logging.ManualLogSource Log { get; private set; }

        /// <summary>
        /// Register additional dishes here when they are ready for release.
        /// </summary>
        internal static readonly IFeastDish[] Dishes =
        {
            new GrizzledPlatterDish(),
            new SerpentsHollowDish()
        };

        private Harmony _harmony;

        private void Awake()
        {
            Instance = this;
            Log = Logger;

            foreach (IFeastDish dish in Dishes)
            {
                dish.BindConfig(Config);
            }

            AddLocalization();
            PrefabManager.OnVanillaPrefabsAvailable += OnVanillaPrefabsAvailable;
            ItemManager.OnItemsRegistered += OnItemsRegistered;
            ItemManager.OnItemsRegisteredFejd += OnItemsRegistered;

            _harmony = new Harmony(PluginGuid);
            _harmony.PatchAll();
            Log.LogInfo(PluginName + " " + PluginVersion + " loaded.");
        }

        private void OnDestroy()
        {
            PrefabManager.OnVanillaPrefabsAvailable -= OnVanillaPrefabsAvailable;
            ItemManager.OnItemsRegistered -= OnItemsRegistered;
            ItemManager.OnItemsRegisteredFejd -= OnItemsRegistered;
            _harmony?.UnpatchSelf();
        }

        private static void AddLocalization()
        {
            CustomLocalization loc = LocalizationManager.Instance.GetLocalization();
            foreach (IFeastDish dish in Dishes)
            {
                dish.AddLocalization(loc);
            }
        }

        private void OnVanillaPrefabsAvailable()
        {
            PrefabManager.OnVanillaPrefabsAvailable -= OnVanillaPrefabsAvailable;
            Sprite icon = LoadIcon();
            foreach (IFeastDish dish in Dishes)
            {
                try
                {
                    if (!dish.ShouldRegister())
                    {
                        continue;
                    }
                    dish.Register(icon);
                }
                catch (System.Exception ex)
                {
                    Log.LogError("Failed to register " + dish.DisplayName + ": " + ex);
                }
            }
        }

        private void OnItemsRegistered()
        {
            foreach (IFeastDish dish in Dishes)
            {
                try
                {
                    dish.ApplyRegisteredStats();
                }
                catch (System.Exception ex)
                {
                    Log.LogError("Failed to apply feast stats for " + dish.DisplayName + ": " + ex);
                }
            }
        }

        internal static Sprite LoadIcon()
        {
            try
            {
                string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string path = Path.Combine(dir ?? string.Empty, "icon.png");
                if (File.Exists(path))
                {
                    byte[] bytes = File.ReadAllBytes(path);
                    Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                    if (tex.LoadImage(bytes))
                    {
                        tex.name = "GlymorsFeasts_Icon";
                        return Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.LogWarning("Could not load icon.png: " + ex.Message);
            }
            return null;
        }
    }
}