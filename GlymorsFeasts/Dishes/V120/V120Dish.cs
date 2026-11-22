using System;
using System.Collections.Generic;
using BepInEx.Configuration;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GlymorsFeasts
{
    internal sealed class V120Dish : IFeastDish
    {
        private readonly V120Def _def;

        private bool _registered;

        private Sprite _icon;

        private ConfigEntry<float> _statMult;

        private ConfigEntry<string> _recipe;

        private ConfigEntry<int> _servings;

        private ConfigEntry<bool> _steam;

        public string Id => _def.Id;

        public string DisplayName => _def.DisplayName;

        internal V120Dish(V120Def def)
        {
            _def = def;
        }

        public bool ShouldRegister()
        {
            return true;
        }

        public void BindConfig(ConfigFile config)
        {
            _statMult = config.Bind<float>(_def.ConfigSection + " - Food", "StatMultiplier", 1f, "Multiplier applied to this feast's health, stamina, eitr and regen. Restart required.");
            _recipe = config.Bind<string>(_def.ConfigSection + " - Crafting", "Recipe", _def.DefaultRecipe, "Food Preparation Table ingredients as PrefabName:Amount pairs, comma-separated. Restart required.");
            _servings = config.Bind<int>(_def.ConfigSection + " - Food", "Servings", _def.Servings, "Number of servings on a placed feast. Vanilla feasts use 10. Restart required.");
            if (_def.EnableSteam)
            {
                _steam = config.Bind<bool>(_def.ConfigSection + " - Visuals", "EnableSteam", true, "Use vanilla fx_FoodSteam on this skillet while servings remain. Restart required.");
            }
        }

        public void AddLocalization(CustomLocalization loc)
        {
            string text = "English";
            loc.AddTranslation(text, new Dictionary<string, string>
            {
                {
                    _def.TokenName.TrimStart('$'),
                    _def.DisplayName
                },
                {
                    _def.TokenDesc.TrimStart('$'),
                    _def.Description
                }
            });
            if (_def.SeaGodsFavor)
            {
                text = "English";
                loc.AddTranslation(text, new Dictionary<string, string>
                {
                    { "se_seagodsfavor", "Sea-God's Favor" },
                    { "se_seagodsfavor_desc", "The sea asks less of you. Swim stamina use is reduced." }
                });
            }
        }

        public bool TryEnsurePlacedFeast(Feast feast)
        {
            if (!_registered || feast == null || ((Component)feast).gameObject == null)
            {
                return false;
            }
            string name = ((Object)((Component)feast).gameObject).name;
            if (name == null || name.IndexOf(_def.PiecePrefab, StringComparison.Ordinal) < 0)
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
            _icon = V120LayoutBank.LoadIcon(_def.IconFile, _def.Id + "_Icon") ?? packageIcon;
            ItemConfig val = new ItemConfig
            {
                Name = _def.TokenName,
                Description = _def.TokenDesc,
                CraftingStation = CraftingStations.FoodPreparationTable,
                MinStationLevel = 1,
                Amount = 1,
                Weight = _def.Weight,
                StackSize = _def.Stack
            };
            if (_icon != null)
            {
                val.Icon = _icon;
            }
            foreach (RequirementConfig item in ParseRecipe(_recipe.Value))
            {
                val.AddRequirement(item);
            }
            CustomItem val2 = new CustomItem(_def.MaterialPrefab, "FeastPlains_Material", val);
            ItemManager.Instance.AddItem(val2);
            ApplyFoodStats(val2.ItemDrop, _icon);
            PieceConfig val3 = new PieceConfig
            {
                Name = _def.TokenName,
                Description = _def.TokenDesc,
                PieceTable = PieceTables.ServingTray,
                Category = PieceCategories.Feasts,
                AllowedInDungeons = false
            };
            if (_icon != null)
            {
                val3.Icon = _icon;
            }
            val3.AddRequirement(_def.MaterialPrefab, 1, true);
            CustomPiece val4 = new CustomPiece(_def.PiecePrefab, "FeastPlains", val3);
            PieceManager.Instance.AddPiece(val4);
            _registered = true;
            GameObject piecePrefab = val4.PiecePrefab;
            if (piecePrefab == null)
            {
                Plugin.Log.LogError((_def.PiecePrefab + " piece prefab was not created."));
                return;
            }
            ItemDrop component = piecePrefab.GetComponent<ItemDrop>();
            if (component != null)
            {
                ApplyFoodStats(component, _icon);
            }
            Feast val5 = piecePrefab.GetComponent<Feast>();
            if (val5 == null)
            {
                val5 = piecePrefab.AddComponent<Feast>();
            }
            val5.m_eatStacks = Mathf.Max(1, _servings.Value);
            val5.m_foodItem = ((val2.ItemDrop != null) ? val2.ItemDrop : component);
            GameObject obj = FeastHelpers.FindPrefab("FeastPlains");
            Feast val6 = ((obj != null) ? obj.GetComponent<Feast>() : null);
            val5.m_useDistance = ((val6 != null) ? val6.m_useDistance : 4f);
            if (val6 != null && val6.m_eatEffect != null)
            {
                val5.m_eatEffect = val6.m_eatEffect;
            }
            bool enableSteam = _def.EnableSteam && (_steam == null || _steam.Value);
            V120Composer.Compose(piecePrefab, val5, _def.LayoutId, enableSteam);
            EnsurePlacedFeast(val5);
            V120Composer.ComposeItemDrop((val2.ItemPrefab != null) ? val2.ItemPrefab : ((val2.ItemDrop != null) ? ((Component)val2.ItemDrop).gameObject : null), piecePrefab, _def.LayoutId);
            Plugin.Log.LogInfo($"Registered {_def.PiecePrefab}: {val2.ItemDrop.m_itemData.m_shared.m_food:0}/{val2.ItemDrop.m_itemData.m_shared.m_foodStamina:0}/{val2.ItemDrop.m_itemData.m_shared.m_foodEitr:0} food, regen {val2.ItemDrop.m_itemData.m_shared.m_foodRegen:0}, {val2.ItemDrop.m_itemData.m_shared.m_foodBurnTime / 60f:0} min, {val5.m_eatStacks} servings. Recipe: {_recipe.Value}");
        }

        public void ApplyRegisteredStats()
        {
            if (!_registered)
            {
                return;
            }
            GameObject obj = FeastHelpers.FindPrefab(_def.MaterialPrefab);
            ItemDrop val = ((obj != null) ? obj.GetComponent<ItemDrop>() : null);
            ApplyFoodStats(val, _icon);
            GameObject val2 = FeastHelpers.FindPrefab(_def.PiecePrefab);
            if (val2 != null)
            {
                ApplyFoodStats(val2.GetComponent<ItemDrop>(), _icon);
                Feast component = val2.GetComponent<Feast>();
                if (component != null)
                {
                    EnsurePlacedFeast(component);
                }
            }
            if (!_def.SeaGodsFavor)
            {
                return;
            }
            SeaGodsFavor.Apply(val);
            if (val2 != null)
            {
                SeaGodsFavor.Apply(val2.GetComponent<ItemDrop>());
                Feast component2 = val2.GetComponent<Feast>();
                if (component2 != null && component2.m_foodItem != null)
                {
                    SeaGodsFavor.Apply(component2.m_foodItem);
                }
            }
        }

        private void EnsurePlacedFeast(Feast feast)
        {
            if (!(feast == null))
            {
                GameObject obj = FeastHelpers.FindPrefab(_def.MaterialPrefab);
                ItemDrop val = ((obj != null) ? obj.GetComponent<ItemDrop>() : null);
                if (val != null)
                {
                    feast.m_foodItem = val;
                    ApplyFoodStats(val, _icon);
                }
                else if (feast.m_foodItem == null)
                {
                    feast.m_foodItem = ((Component)feast).GetComponent<ItemDrop>();
                }
                if (feast.m_foodItem != null && feast.m_foodItem.m_itemData != null && feast.m_foodItem.m_itemData.m_shared != null)
                {
                    feast.m_foodItem.m_itemData.m_shared.m_itemType = ItemDrop.ItemData.ItemType.Consumable;
                }
                feast.m_eatStacks = Mathf.Max(1, _servings.Value);
                if (_def.SeaGodsFavor && feast.m_foodItem != null)
                {
                    SeaGodsFavor.Apply(feast.m_foodItem);
                }
            }
        }

        private void ApplyFoodStats(ItemDrop target, Sprite icon)
        {
            if (!(target == null) && target.m_itemData != null && target.m_itemData.m_shared != null)
            {
                ItemDrop.ItemData.SharedData shared = target.m_itemData.m_shared;
                float num = ((_statMult != null) ? Mathf.Max(0f, _statMult.Value) : 1f);
                shared.m_food = _def.Health * num;
                shared.m_foodStamina = _def.Stamina * num;
                shared.m_foodEitr = _def.Eitr * num;
                shared.m_foodRegen = _def.Regen * num;
                shared.m_foodBurnTime = _def.DurationSeconds;
                shared.m_weight = _def.Weight;
                shared.m_maxStackSize = _def.Stack;
                shared.m_itemType = ItemDrop.ItemData.ItemType.Consumable;
                if (shared.m_appendToolTip != null)
                {
                    shared.m_appendToolTip = null;
                }
                shared.m_name = _def.TokenName;
                shared.m_description = _def.TokenDesc;
                if (icon != null)
                {
                    shared.m_icons = (Sprite[])(object)new Sprite[1] { icon };
                }
            }
        }

        private List<RequirementConfig> ParseRecipe(string recipe)
        {
            List<RequirementConfig> list = new List<RequirementConfig>();
            if (string.IsNullOrEmpty(recipe))
            {
                recipe = _def.DefaultRecipe;
            }
            string[] array = recipe.Split(new char[2] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < array.Length; i++)
            {
                string[] array2 = array[i].Trim().Split(':');
                if (array2.Length >= 1 && !string.IsNullOrEmpty(array2[0]))
                {
                    int result = 1;
                    if (array2.Length > 1)
                    {
                        int.TryParse(array2[1].Trim(), out result);
                    }
                    result = Mathf.Max(1, result);
                    list.Add(new RequirementConfig
                    {
                        Item = array2[0].Trim(),
                        Amount = result
                    });
                }
            }
            if (list.Count == 0)
            {
                return ParseRecipe(_def.DefaultRecipe);
            }
            return list;
        }
    }
}
