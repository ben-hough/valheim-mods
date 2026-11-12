using System;
using System.Collections.Generic;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using UnityEngine;

namespace GlymorsFeasts
{
    internal sealed class SerpentsHollowDish : IFeastDish
    {
        public const string MaterialPrefab = "FeastSerpentBreadBowl_Material";
        public const string PiecePrefab = "FeastSerpentBreadBowl";
        public const string TokenName = "$item_feastserpentbreadbowl";
        public const string TokenDesc = "$item_feastserpentbreadbowl_desc";

        public string Id => "SerpentsHollow";
        public string DisplayName => "Serpent's Hollow";

        private static bool _registered;

        // Sailor's Bounty (FeastOceans) fallbacks, from the live typetree dump of
        // the hidden Consumable referenced by FeastOceans_Material.m_appendToolTip.
        // Do not use the Material item's m_food (it is 0).
        private const float FallbackHealth = 45f;
        private const float FallbackStamina = 45f;
        private const float FallbackEitr = 0f;
        private const float FallbackRegen = 3f;
        private const float FallbackDurationSeconds = 50f * 60f;
        private const float FallbackWeight = 10f;
        private const int FallbackStack = 5;
        private const int FallbackServings = 10;
        private const float FallbackUseDistance = 4.0f;

        private static Sprite _icon;

        public bool ShouldRegister()
        {
            if (FeastHelpers.IsPluginLoaded(Plugin.LegacySerpentBreadBowlFeastGuid) ||
                FeastHelpers.IsAssemblyLoaded(Plugin.LegacySerpentBreadBowlFeastAssembly))
            {
                Plugin.Log.LogWarning(
                    "Standalone Serpent's Hollow (com.benhough.valheim.SerpentBreadBowlFeast / SerpentBreadBowlFeast.dll) is also loaded. " +
                    "Glymor's Feasts will skip registering Serpent's Hollow (FeastSerpentBreadBowl / FeastSerpentBreadBowl_Material) to avoid duplicate prefabs. " +
                    "Remove the standalone MrGlim-SerpentsHollow / SerpentBreadBowlFeast package and restart.");
                return false;
            }
            return true;
        }

        public void BindConfig(BepInEx.Configuration.ConfigFile config)
        {
            SerpentsHollowConfig.Bind(config);
        }

        public void AddLocalization(CustomLocalization loc)
        {
            loc.AddTranslation("English", new Dictionary<string, string>
            {
                { "item_feastserpentbreadbowl", "Serpent's Hollow" },
                {
                    "item_feastserpentbreadbowl_desc",
                    "A hearty pot of Serpent Stew set out with round bread bowls: one filled and steaming, an empty bowl with its lid beside it, and Queen's Jam on leftover rolls. A feast for the whole hall."
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
            if (name == null || name.IndexOf("FeastSerpentBreadBowl") < 0)
            {
                return false;
            }
            EnsurePlacedFeast(feast);
            return true;
        }

        public void Register(Sprite packageIcon)
        {
            if (!ShouldRegister())
            {
                return;
            }
            _icon = LoadDishIcon() ?? packageIcon;
            ItemDrop oceansFood = FindOceansFood();
            ItemDrop oceansMaterial = FindPrefab("FeastOceans_Material")?.GetComponent<ItemDrop>();
            Feast oceansFeast = FindPrefab("FeastOceans")?.GetComponent<Feast>();

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
            if (_icon != null)
            {
                itemConfig.Icon = _icon;
            }
            foreach (RequirementConfig req in ParseRecipe(SerpentsHollowConfig.Recipe.Value))
            {
                itemConfig.AddRequirement(req);
            }

            // Same picnic board as the vanilla Plains pie feast (FeastPlains), so the item-drop scale matches the placed dish.
            CustomItem material = new CustomItem(MaterialPrefab, "FeastPlains_Material", itemConfig);
            ItemManager.Instance.AddItem(material);

            ApplyFoodStats(material.ItemDrop, oceansFood, oceansMaterial, _icon);

            PieceConfig pieceConfig = new PieceConfig
            {
                Name = TokenName,
                Description = TokenDesc,
                PieceTable = PieceTables.ServingTray,
                Category = PieceCategories.Feasts,
                AllowedInDungeons = false
            };
            if (_icon != null)
            {
                pieceConfig.Icon = _icon;
            }
            pieceConfig.AddRequirement(MaterialPrefab, 1, recover: true);

            CustomPiece piece = new CustomPiece(PiecePrefab, "FeastPlains", pieceConfig);
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
                ApplyFoodStats(pieceDrop, oceansFood, oceansMaterial, _icon);
            }

            Feast feast = pieceGo.GetComponent<Feast>();
            if (feast == null)
            {
                feast = pieceGo.AddComponent<Feast>();
            }
            feast.m_eatStacks = Mathf.Max(1, SerpentsHollowConfig.Servings.Value > 0 ? SerpentsHollowConfig.Servings.Value : (oceansFeast != null ? oceansFeast.m_eatStacks : FallbackServings));
            feast.m_foodItem = material.ItemDrop != null ? material.ItemDrop : pieceDrop;
            feast.m_useDistance = oceansFeast != null ? oceansFeast.m_useDistance : FallbackUseDistance;
            if (oceansFeast != null && oceansFeast.m_eatEffect != null)
            {
                feast.m_eatEffect = oceansFeast.m_eatEffect;
            }

            SerpentsHollowVisuals.Compose(pieceGo, feast);
            EnsurePlacedFeast(feast);
            GameObject materialGo = material.ItemPrefab != null ? material.ItemPrefab : (material.ItemDrop != null ? material.ItemDrop.gameObject : null);
            SerpentsHollowVisuals.ComposeItemDrop(materialGo, pieceGo);
            Plugin.Log.LogInfo(string.Format(
                "Registered {0}: {1:0} health, {2:0} stamina, {3:0} eitr, {4:0} regen, {5:0} min, {6} servings. Recipe: {7}",
                PiecePrefab,
                material.ItemDrop.m_itemData.m_shared.m_food,
                material.ItemDrop.m_itemData.m_shared.m_foodStamina,
                material.ItemDrop.m_itemData.m_shared.m_foodEitr,
                material.ItemDrop.m_itemData.m_shared.m_foodRegen,
                material.ItemDrop.m_itemData.m_shared.m_foodBurnTime / 60f,
                feast.m_eatStacks,
                SerpentsHollowConfig.Recipe.Value));
        }

        private static void ApplyFoodStats(ItemDrop target, ItemDrop foodSrc, ItemDrop materialSrc, Sprite icon)
        {
            if (target == null || target.m_itemData == null || target.m_itemData.m_shared == null)
            {
                return;
            }

            ItemDrop.ItemData.SharedData shared = target.m_itemData.m_shared;
            float mult = Mathf.Max(0f, SerpentsHollowConfig.StatMultiplier.Value);
            ItemDrop.ItemData.SharedData src = foodSrc != null ? foodSrc.m_itemData?.m_shared : null;
            bool haveLive = src != null && (src.m_food > 0f || src.m_foodStamina > 0f || src.m_foodBurnTime > 0f);
            if (haveLive)
            {
                shared.m_food = src.m_food * mult;
                shared.m_foodStamina = src.m_foodStamina * mult;
                shared.m_foodEitr = src.m_foodEitr * mult;
                shared.m_foodRegen = src.m_foodRegen * mult;
                shared.m_foodBurnTime = src.m_foodBurnTime;
            }
            else
            {
                Plugin.Log.LogInfo("Using baked-in Sailor's Bounty stats (live prefab values were not ready or were empty).");
                shared.m_food = FallbackHealth * mult;
                shared.m_foodStamina = FallbackStamina * mult;
                shared.m_foodEitr = FallbackEitr * mult;
                shared.m_foodRegen = FallbackRegen * mult;
                shared.m_foodBurnTime = FallbackDurationSeconds;
            }

            // Weight and stack live on the craftable material (10 / 5), not on the
            // hidden Consumable that holds the 45/45 food stats (those are 1 / 1).
            ItemDrop.ItemData.SharedData mat = materialSrc != null ? materialSrc.m_itemData?.m_shared : null;
            if (mat != null && mat.m_weight > 0f)
            {
                shared.m_weight = mat.m_weight;
            }
            else
            {
                shared.m_weight = FallbackWeight;
            }
            if (mat != null && mat.m_maxStackSize > 0)
            {
                shared.m_maxStackSize = mat.m_maxStackSize;
            }
            else
            {
                shared.m_maxStackSize = FallbackStack;
            }

            shared.m_itemType = ItemDrop.ItemData.ItemType.Consumable;
            if (src != null && src.m_consumeStatusEffect != null)
            {
                shared.m_consumeStatusEffect = src.m_consumeStatusEffect;
            }
            // FeastPlains_Material (our clone source) may leave m_appendToolTip pointing
            // at the plains food item. If we also copied oceans stats onto this item,
            // ItemDrop.GetTooltip would print them twice. Clear the pointer.
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
            ItemDrop oceansFood = FindOceansFood();
            ItemDrop oceansMaterial = FindPrefab("FeastOceans_Material")?.GetComponent<ItemDrop>();
            if (material != null)
            {
                feast.m_foodItem = material;
                ApplyFoodStats(material, oceansFood, oceansMaterial, _icon);
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
            // Cover the FeastPlains picnic board (~2.23 x 1.79, plus the handle) and the center pot.
            box.center = new Vector3(0f, 0.32f, 0f);
            box.size = new Vector3(2.40f, 0.90f, 2.00f);
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
            ItemDrop oceansFood = FindOceansFood();
            ItemDrop oceansMaterial = FindPrefab("FeastOceans_Material")?.GetComponent<ItemDrop>();
            ItemDrop material = FindPrefab(MaterialPrefab)?.GetComponent<ItemDrop>();
            GameObject pieceGo = FindPrefab(PiecePrefab);
            if (material != null)
            {
                ApplyFoodStats(material, oceansFood, oceansMaterial, _icon);
            }
            if (pieceGo != null)
            {
                ItemDrop pieceDrop = pieceGo.GetComponent<ItemDrop>();
                if (pieceDrop != null)
                {
                    ApplyFoodStats(pieceDrop, oceansFood, oceansMaterial, _icon);
                }
                Feast feast = pieceGo.GetComponent<Feast>();
                if (feast != null && feast.m_foodItem != null)
                {
                    ApplyFoodStats(feast.m_foodItem, oceansFood, oceansMaterial, _icon);
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

        // FeastOceans_Material.m_food is 0; the 45/45/3/50 stats live on the
        // hidden Consumable referenced by m_appendToolTip. Follow that pointer.
        internal static ItemDrop FindOceansFood()
        {
            ItemDrop mat = FindPrefab("FeastOceans_Material")?.GetComponent<ItemDrop>();
            ItemDrop fromAppend = FollowAppendTooltip(mat);
            if (fromAppend != null)
            {
                return fromAppend;
            }

            ItemDrop[] candidates =
            {
                FindPrefab("FeastOceans")?.GetComponent<Feast>()?.m_foodItem,
                FollowAppendTooltip(FindPrefab("FeastOceans")?.GetComponent<ItemDrop>()),
                FindPrefab("FeastOceans")?.GetComponent<ItemDrop>(),
                mat
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

        private static ItemDrop FollowAppendTooltip(ItemDrop drop)
        {
            if (drop?.m_itemData?.m_shared == null)
            {
                return null;
            }
            ItemDrop.ItemData.SharedData s = drop.m_itemData.m_shared;
            if (s.m_food > 0f || s.m_foodStamina > 0f)
            {
                return drop;
            }
            ItemDrop append = s.m_appendToolTip;
            if (append?.m_itemData?.m_shared == null)
            {
                return null;
            }
            ItemDrop.ItemData.SharedData a = append.m_itemData.m_shared;
            if (a.m_food > 0f || a.m_foodStamina > 0f || a.m_foodBurnTime > 0f)
            {
                return append;
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
                raw = SerpentsHollowConfig.DefaultRecipe;
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
                return ParseRecipe(SerpentsHollowConfig.DefaultRecipe);
            }
            return list;
        }

        private static Sprite LoadDishIcon()
        {
            try
            {
                byte[] embedded = SerpentsHollowMeshBank.ReadResource("icon.png");
                if (embedded != null && embedded.Length > 0)
                {
                    Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                    if (tex.LoadImage(embedded))
                    {
                        tex.name = "SerpentBreadBowlFeast_Icon";
                        Plugin.Log.LogInfo("Loaded Serpent's Hollow icon from embedded resource.");
                        return Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);
                    }
                }
            }
            catch (Exception ex)
            {
                Plugin.Log.LogWarning("Could not load embedded Serpent's Hollow icon: " + ex.Message);
            }
            return null;
        }
    }
}
