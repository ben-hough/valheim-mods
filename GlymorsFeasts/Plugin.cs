using System;
using System.Collections.Generic;
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
        public const string PluginVersion = "1.2.0";

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
        /// Every dish this plugin registers: the two original feasts plus the 1.2.0 catalog.
        /// Register additional dishes in BuildDishes / V120Catalog when they are ready for release.
        /// </summary>
        internal static readonly IFeastDish[] Dishes = BuildDishes();

        private const string SeaGodsFavorToken = "se_seagodsfavor";
        private const string SeaGodsFavorName = "Sea-God's Favor";
        private const string SeaGodsFavorDescToken = "se_seagodsfavor_desc";
        private const string SeaGodsFavorDesc = "The sea asks less of you. Swim stamina use is reduced.";

        private static IFeastDish[] BuildDishes()
        {
            IFeastDish[] catalog = V120Catalog.CreateDishes();
            IFeastDish[] all = new IFeastDish[2 + catalog.Length];
            all[0] = new GrizzledPlatterDish();
            all[1] = new SerpentsHollowDish();
            Array.Copy(catalog, 0, all, 2, catalog.Length);
            return all;
        }

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
            Log.LogInfo(PluginName + " " + PluginVersion + " loaded (" + Dishes.Length + " dishes).");
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
            RegisterSeaGodsFavorTokens();
        }

        private static void RegisterSeaGodsFavorTokens()
        {
            RegisterEnglishToken(SeaGodsFavorToken, SeaGodsFavorName);
            RegisterEnglishToken(SeaGodsFavorDescToken, SeaGodsFavorDesc);
        }

        /// <summary>
        /// Adds an English token through Jotunn and, when the game's Localization instance
        /// already exists, directly through Localization.AddWord so late lookups resolve too.
        /// </summary>
        internal static void RegisterEnglishToken(string key, string value)
        {
            string token = (key ?? string.Empty).TrimStart('$');
            if (token.Length == 0 || string.IsNullOrEmpty(value))
            {
                return;
            }
            try
            {
                CustomLocalization loc = LocalizationManager.Instance.GetLocalization();
                string language = "English";
                loc.AddTranslation(language, new Dictionary<string, string> { { token, value } });
            }
            catch (Exception ex)
            {
                Log.LogWarning("Jotunn token " + token + ": " + ex.Message);
            }
            TryAddWord(token, value);
        }

        internal static void TryAddWord(string key, string value)
        {
            try
            {
                Type type = Type.GetType("Localization, assembly_valheim");
                if (type == null)
                {
                    foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        type = asm.GetType("Localization");
                        if (type != null)
                        {
                            break;
                        }
                    }
                }
                if (type == null)
                {
                    return;
                }
                const BindingFlags StaticAny = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
                object instance = null;
                PropertyInfo prop = type.GetProperty("instance", StaticAny);
                if (prop != null)
                {
                    instance = prop.GetValue(null, null);
                }
                if (instance == null)
                {
                    FieldInfo field = type.GetField("instance", StaticAny);
                    if (field != null)
                    {
                        instance = field.GetValue(null);
                    }
                }
                if (instance == null)
                {
                    return;
                }
                MethodInfo addWord = type.GetMethod("AddWord", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    null, new[] { typeof(string), typeof(string) }, null);
                addWord?.Invoke(instance, new object[] { key, value });
            }
            catch (Exception ex)
            {
                Log.LogWarning("AddWord " + key + ": " + ex.Message);
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
                catch (Exception ex)
                {
                    Log.LogError("Failed to register " + dish.DisplayName + ": " + ex);
                }
            }
        }

        private void OnItemsRegistered()
        {
            RegisterSeaGodsFavorTokens();
            foreach (IFeastDish dish in Dishes)
            {
                try
                {
                    dish.ApplyRegisteredStats();
                }
                catch (Exception ex)
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
            catch (Exception ex)
            {
                Log.LogWarning("Could not load icon.png: " + ex.Message);
            }
            return null;
        }
    }
}