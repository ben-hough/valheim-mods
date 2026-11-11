using System;
using System.Collections.Generic;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using UnityEngine;

namespace GlymorsFeasts
{
    internal sealed class GrizzledPlatterDish : IFeastDish
    {
        public const string MaterialPrefab = "FeastPulledBear_Material";
        public const string PiecePrefab = "FeastPulledBear";
        public const string TokenName = "$item_feastpulledbear";
        public const string TokenDesc = "$item_feastpulledbear_desc";

        public string Id => "GrizzledPlatter";
        public string DisplayName => "Grizzled Platter";

        private static bool _registered;

        public bool ShouldRegister()
        {
            if (FeastHelpers.IsPluginLoaded(Plugin.LegacyPulledBearFeastGuid) ||
                FeastHelpers.IsAssemblyLoaded(Plugin.LegacyPulledBearFeastAssembly))
            {
                Plugin.Log.LogWarning(
                    "Standalone Grizzled Platter (com.benhough.valheim.PulledBearFeast / PulledBearFeast.dll) is also loaded. " +
                    "Glymor's Feasts will skip registering Grizzled Platter (FeastPulledBear / FeastPulledBear_Material) to avoid duplicate prefabs. " +
                    "Remove the standalone MrGlim-GrizzledPlatter package and restart.");
                return false;
            }
            return true;
        }

        public void BindConfig(BepInEx.Configuration.ConfigFile config)
        {
            GrizzledPlatterConfig.Bind(config);
        }

        public void AddLocalization(Jotunn.Entities.CustomLocalization loc)
        {
            loc.AddTranslation("English", new System.Collections.Generic.Dictionary<string, string>
            {
                { "item_feastpulledbear", "Grizzled Platter" },
                {
                    "item_feastpulledbear_desc",
                    "A Grizzled Platter of bear meat cooked until it falls apart, spread across a wooden board with roasted carrot and turnip chunks nestled in the shreds. Served steaming hot, it is a feast for the whole hall."
                }
            });
        }

        public bool TryEnsurePlacedFeast(Feast feast)
        {
            if (!_registered || feast == null)
            {
                return false;
            }
            string name = feast.gameObject.name;
            if (name == null || name.IndexOf("FeastPulledBear") < 0)
            {
                return false;
            }
            EnsurePlacedFeast(feast);
            return true;
        }

        // Swamp Dweller's Delight fallbacks, used only if the vanilla prefab is missing.
        private const float FallbackHealth = 35f;
        private const float FallbackStamina = 35f;
        private const float FallbackEitr = 0f;
        private const float FallbackRegen = 3f;
        private const float FallbackDurationSeconds = 50f * 60f;
        private const float FallbackWeight = 10f;
        private const int FallbackStack = 5;
        private const int FallbackServings = 10;

        private static Sprite _icon;

        public void Register(Sprite icon)
        {
            if (!ShouldRegister())
            {
                return;
            }
            _icon = icon;
            ItemDrop swampFood = FindSwampFood();
            Feast swampFeast = FindPrefab("FeastSwamps")?.GetComponent<Feast>();

            ItemConfig itemConfig = new ItemConfig
            {
                Name = TokenName,
                Description = TokenDesc,
                CraftingStation = CraftingStations.FoodPreparationTable,
                MinStationLevel = 1,
                Amount = 1,
                Weight = FallbackWeight,
                StackSize = FallbackStack
            };
            if (icon != null)
            {
                itemConfig.Icon = icon;
            }
            foreach (RequirementConfig req in ParseRecipe(GrizzledPlatterConfig.Recipe.Value))
            {
                itemConfig.AddRequirement(req);
            }

            CustomItem material = new CustomItem(MaterialPrefab, "FeastSwamps_Material", itemConfig);
            ItemManager.Instance.AddItem(material);

            ApplyFoodStats(material.ItemDrop, swampFood, icon);

            PieceConfig pieceConfig = new PieceConfig
            {
                Name = TokenName,
                Description = TokenDesc,
                PieceTable = PieceTables.ServingTray,
                Category = PieceCategories.Feasts,
                AllowedInDungeons = false
            };
            if (icon != null)
            {
                pieceConfig.Icon = icon;
            }
            pieceConfig.AddRequirement(MaterialPrefab, 1, recover: true);

            // Visual base is the Black Forest buffet platter (same stat tier, a handsome wooden tray).
            // Stats themselves are copied from Swamp Dweller's Delight, per the request.
            CustomPiece piece = new CustomPiece(PiecePrefab, "FeastBlackforest", pieceConfig);
            PieceManager.Instance.AddPiece(piece);
            _registered = true;

            GameObject pieceGo = piece.PiecePrefab;
            if (pieceGo == null)
            {
                Plugin.Log.LogError("Feast piece prefab was not created.");
                return;
            }

            ItemDrop pieceDrop = pieceGo.GetComponent<ItemDrop>();
            if (pieceDrop != null)
            {
                ApplyFoodStats(pieceDrop, swampFood, icon);
            }

            Feast feast = pieceGo.GetComponent<Feast>();
            if (feast == null)
            {
                feast = pieceGo.AddComponent<Feast>();
            }
            feast.m_eatStacks = Mathf.Max(1, GrizzledPlatterConfig.Servings.Value > 0 ? GrizzledPlatterConfig.Servings.Value : (swampFeast != null ? swampFeast.m_eatStacks : FallbackServings));
            feast.m_foodItem = material.ItemDrop != null ? material.ItemDrop : pieceDrop;
            if (swampFeast != null)
            {
                feast.m_useDistance = swampFeast.m_useDistance;
                if (swampFeast.m_eatEffect != null)
                {
                    feast.m_eatEffect = swampFeast.m_eatEffect;
                }
            }

            GrizzledPlatterVisuals.Compose(pieceGo, feast);
            EnsurePlacedFeast(feast);
            GameObject materialGo = material.ItemPrefab != null ? material.ItemPrefab : (material.ItemDrop != null ? material.ItemDrop.gameObject : null);
            GrizzledPlatterVisuals.ComposeItemDrop(materialGo, pieceGo);
            Plugin.Log.LogInfo(string.Format(
                "Registered {0}: {1:0} health, {2:0} stamina, {3:0} eitr, {4:0} regen, {5:0} min, {6} servings. Recipe: {7}",
                PiecePrefab,
                material.ItemDrop.m_itemData.m_shared.m_food,
                material.ItemDrop.m_itemData.m_shared.m_foodStamina,
                material.ItemDrop.m_itemData.m_shared.m_foodEitr,
                material.ItemDrop.m_itemData.m_shared.m_foodRegen,
                material.ItemDrop.m_itemData.m_shared.m_foodBurnTime / 60f,
                feast.m_eatStacks,
                GrizzledPlatterConfig.Recipe.Value));
        }

        private static void ApplyFoodStats(ItemDrop target, ItemDrop reference, Sprite icon)
        {
            if (target == null || target.m_itemData == null || target.m_itemData.m_shared == null)
            {
                return;
            }

            ItemDrop.ItemData.SharedData shared = target.m_itemData.m_shared;
            float mult = Mathf.Max(0f, GrizzledPlatterConfig.StatMultiplier.Value);
            ItemDrop.ItemData.SharedData src = reference != null ? reference.m_itemData?.m_shared : null;
            bool haveLive = src != null && (src.m_food > 0f || src.m_foodStamina > 0f || src.m_foodBurnTime > 0f);
            if (haveLive)
            {
                shared.m_food = src.m_food * mult;
                shared.m_foodStamina = src.m_foodStamina * mult;
                shared.m_foodEitr = src.m_foodEitr * mult;
                shared.m_foodRegen = src.m_foodRegen * mult;
                shared.m_foodBurnTime = src.m_foodBurnTime;
                if (src.m_weight > 0f)
                {
                    shared.m_weight = src.m_weight;
                }
                if (src.m_maxStackSize > 0)
                {
                    shared.m_maxStackSize = src.m_maxStackSize;
                }
            }
            else
            {
                Plugin.Log.LogInfo("Using baked-in Swamp Dweller's Delight stats (live prefab values were not ready or were empty).");
                shared.m_food = FallbackHealth * mult;
                shared.m_foodStamina = FallbackStamina * mult;
                shared.m_foodEitr = FallbackEitr * mult;
                shared.m_foodRegen = FallbackRegen * mult;
                shared.m_foodBurnTime = FallbackDurationSeconds;
                shared.m_weight = FallbackWeight;
                shared.m_maxStackSize = FallbackStack;
            }

            shared.m_itemType = ItemDrop.ItemData.ItemType.Consumable;
            if (src != null && src.m_consumeStatusEffect != null)
            {
                shared.m_consumeStatusEffect = src.m_consumeStatusEffect;
            }
            // FeastSwamps_Material (our clone source) leaves m_appendToolTip pointing at the
            // swamp food item, which has the same 35/35/3/50 stats we just copied onto this
            // item. ItemDrop.GetTooltip already prints those via printConsumable(item), then
            // printConsumable(m_appendToolTip) would print them a second time in the craft
            // panel. Clear the pointer; the stats live on this item now.
            if (shared.m_appendToolTip != null)
            {
                Plugin.Log.LogInfo("Cleared inherited m_appendToolTip (" + shared.m_appendToolTip.name + ") to prevent doubled food stats in the tooltip.");
                shared.m_appendToolTip = null;
            }
            shared.m_name = TokenName;
            shared.m_description = TokenDesc;
            if (icon != null)
            {
                shared.m_icons = new Sprite[] { icon };
            }
        }

        internal static ItemDrop FindMaterialDrop()
        {
            GameObject go = FindPrefab(MaterialPrefab);
            return go != null ? go.GetComponent<ItemDrop>() : null;
        }

        internal static void EnsurePlacedFeast(Feast feast)
        {
            if (feast == null)
            {
                return;
            }

            ItemDrop material = FindMaterialDrop();
            if (material != null)
            {
                feast.m_foodItem = material;
                ApplyFoodStats(material, FindSwampFood(), _icon);
            }
            else if (feast.m_foodItem == null)
            {
                feast.m_foodItem = feast.GetComponent<ItemDrop>();
                Plugin.Log.LogWarning("Material item was missing; Feast.m_foodItem fell back to the piece ItemDrop.");
            }

            if (feast.m_foodItem != null && feast.m_foodItem.m_itemData != null && feast.m_foodItem.m_itemData.m_shared != null)
            {
                feast.m_foodItem.m_itemData.m_shared.m_itemType = ItemDrop.ItemData.ItemType.Consumable;
            }

            EnsureInteractCollider(feast.gameObject);
            LogFeastState(feast);
        }

        internal static void EnsureInteractCollider(GameObject piece)
        {
            if (piece == null)
            {
                return;
            }

            Transform t = piece.transform.Find("collider");
            BoxCollider box = t != null ? t.GetComponent<BoxCollider>() : null;
            if (box == null)
            {
                GameObject go = t != null ? t.gameObject : new GameObject("collider");
                if (t == null)
                {
                    go.transform.SetParent(piece.transform, false);
                }
                box = go.GetComponent<BoxCollider>();
                if (box == null)
                {
                    box = go.AddComponent<BoxCollider>();
                }
                t = go.transform;
                Plugin.Log.LogInfo("Added a feast interact collider on " + piece.name);
            }

            t.gameObject.SetActive(true);
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            box.enabled = true;
            // Cover the wooden board (~1.55 across) and the low meat bed so a look at the
            // food hits this child and walks up to the Feast component, same as vanilla.
            box.center = new Vector3(0f, 0.16f, 0f);
            box.size = new Vector3(1.55f, 0.45f, 1.55f);
        }

        internal static void LogFeastState(Feast feast)
        {
            if (feast == null)
            {
                return;
            }
            ItemDrop food = feast.m_foodItem;
            string foodName = food != null ? food.name : "null";
            string itemType = "none";
            float health = 0f, stam = 0f, regen = 0f, minutes = 0f;
            if (food != null && food.m_itemData != null && food.m_itemData.m_shared != null)
            {
                ItemDrop.ItemData.SharedData s = food.m_itemData.m_shared;
                itemType = s.m_itemType.ToString();
                health = s.m_food;
                stam = s.m_foodStamina;
                regen = s.m_foodRegen;
                minutes = s.m_foodBurnTime / 60f;
            }
            int col = feast.GetComponentsInChildren<Collider>(true).Length;
            Plugin.Log.LogInfo(string.Format(
                "Feast state on {0}: foodItem={1} itemType={2} food={3:0}/{4:0} regen={5:0} {6:0}min stacks={7} colliders={8}",
                feast.gameObject.name, foodName, itemType, health, stam, regen, minutes, feast.m_eatStacks, col));
        }

        public void ApplyRegisteredStats()
        {
            if (!_registered)
            {
                return;
            }
            ItemDrop swamp = FindSwampFood();
            ItemDrop material = FindPrefab(MaterialPrefab)?.GetComponent<ItemDrop>();
            GameObject pieceGo = FindPrefab(PiecePrefab);
            if (material != null)
            {
                ApplyFoodStats(material, swamp, _icon);
            }
            if (pieceGo != null)
            {
                ItemDrop pieceDrop = pieceGo.GetComponent<ItemDrop>();
                if (pieceDrop != null)
                {
                    ApplyFoodStats(pieceDrop, swamp, _icon);
                }
                Feast feast = pieceGo.GetComponent<Feast>();
                if (feast != null && feast.m_foodItem != null)
                {
                    ApplyFoodStats(feast.m_foodItem, swamp, _icon);
                }
                if (feast != null)
                {
                    EnsurePlacedFeast(feast);
                }
            }
            if (material != null && material.m_itemData != null && material.m_itemData.m_shared != null)
            {
                ItemDrop.ItemData.SharedData s = material.m_itemData.m_shared;
                Plugin.Log.LogInfo(string.Format(
                    "Applied feast stats: {0:0} health, {1:0} stamina, {2:0} eitr, {3:0} regen, {4:0} min",
                    s.m_food, s.m_foodStamina, s.m_foodEitr, s.m_foodRegen, s.m_foodBurnTime / 60f));
            }
        }

        private static ItemDrop FindSwampFood()
        {
            ItemDrop[] candidates =
            {
                FindPrefab("FeastSwamps_Material")?.GetComponent<ItemDrop>(),
                FindPrefab("FeastSwamps")?.GetComponent<Feast>()?.m_foodItem,
                FindPrefab("FeastSwamps")?.GetComponent<ItemDrop>(),
                FindPrefab("FeastBlackforest_Material")?.GetComponent<ItemDrop>(),
                FindPrefab("FeastBlackforest")?.GetComponent<Feast>()?.m_foodItem,
                FindPrefab("FeastBlackforest")?.GetComponent<ItemDrop>()
            };
            foreach (ItemDrop drop in candidates)
            {
                if (drop?.m_itemData?.m_shared == null)
                {
                    continue;
                }
                ItemDrop.ItemData.SharedData s = drop.m_itemData.m_shared;
                if (s.m_food > 0f || s.m_foodStamina > 0f || s.m_foodBurnTime > 0f)
                {
                    return drop;
                }
            }
            foreach (ItemDrop drop in candidates)
            {
                if (drop != null)
                {
                    return drop;
                }
            }
            return null;
        }

        internal static GameObject FindPrefab(string name)
        {
            return FeastHelpers.FindPrefab(name);
        }

        internal static IEnumerable<RequirementConfig> ParseRecipe(string raw)
        {
            List<RequirementConfig> list = new List<RequirementConfig>();
            if (string.IsNullOrEmpty(raw))
            {
                raw = GrizzledPlatterConfig.DefaultRecipe;
            }
            string[] parts = raw.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string part in parts)
            {
                string trimmed = part.Trim();
                if (trimmed.Length == 0)
                {
                    continue;
                }
                string[] kv = trimmed.Split(new[] { ':', 'x', 'X', '*' }, 2);
                string item = kv[0].Trim();
                int amount = 1;
                if (kv.Length > 1)
                {
                    int.TryParse(kv[1].Trim(), out amount);
                }
                if (!string.IsNullOrEmpty(item) && amount > 0)
                {
                    list.Add(new RequirementConfig { Item = item, Amount = amount });
                }
            }
            if (list.Count == 0)
            {
                Plugin.Log.LogWarning("Recipe parsed empty, falling back to default.");
                return ParseRecipe(GrizzledPlatterConfig.DefaultRecipe);
            }
            return list;
        }
    }
}
