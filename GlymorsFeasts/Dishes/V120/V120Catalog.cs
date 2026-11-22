namespace GlymorsFeasts
{
    internal static class V120Catalog
    {
        internal static readonly V120Def[] All = new V120Def[8]
        {
            new V120Def
            {
                LayoutId = "hearthmorning-skillet",
                Id = "HearthmorningSkillet",
                DisplayName = "Hearthmorning Skillet",
                MaterialPrefab = "FeastHearthmorning_Material",
                PiecePrefab = "FeastHearthmorning",
                TokenName = "$item_feasthearthmorning",
                TokenDesc = "$item_feasthearthmorning_desc",
                Description = "A skillet breakfast of fried eggs, sausage and hash, with toasted bread around the rim. A feast for the whole hall.",
                DefaultRecipe = "CookedEgg:2,Sausages:2,MushroomOmelette:1,Carrot:2,Bread:1",
                IconFile = "hearthmorning-skillet.png",
                Health = 55f,
                Stamina = 55f,
                Eitr = 0f,
                Regen = 4f,
                EnableSteam = true,
                ConfigSection = "HearthmorningSkillet"
            },
            new V120Def
            {
                LayoutId = "shieldmaidens-spread",
                Id = "ShieldmaidensSpread",
                DisplayName = "Shieldmaiden's Spread",
                MaterialPrefab = "FeastShieldmaiden_Material",
                PiecePrefab = "FeastShieldmaiden",
                TokenName = "$item_feastshieldmaiden",
                TokenDesc = "$item_feastshieldmaiden_desc",
                Description = "A kite-shield breakfast of eggs, honey, bread and fresh berries. A feast for the whole hall.",
                DefaultRecipe = "CookedEgg:2,Bread:2,Honey:2,Raspberry:4,Blueberries:3,Cloudberry:2",
                IconFile = "shieldmaidens-spread.png",
                Health = 35f,
                Stamina = 55f,
                Eitr = 0f,
                Regen = 3f,
                ConfigSection = "ShieldmaidensSpread"
            },
            new V120Def
            {
                LayoutId = "lox-benedict-board",
                Id = "LoxBenedictBoard",
                DisplayName = "Lox Benedict Board",
                MaterialPrefab = "FeastLoxBenedict_Material",
                PiecePrefab = "FeastLoxBenedict",
                TokenName = "$item_feastloxbenedict",
                TokenDesc = "$item_feastloxbenedict_desc",
                Description = "Toasts piled with seared lox, poached eggs and a honey glaze. A feast for the whole hall.",
                DefaultRecipe = "CookedEgg:2,CookedLoxMeat:2,Bread:2,Honey:1",
                IconFile = "lox-benedict-board.png",
                Health = 55f,
                Stamina = 45f,
                Eitr = 0f,
                Regen = 4f,
                ConfigSection = "LoxBenedictBoard"
            },
            new V120Def
            {
                LayoutId = "huntsmans-basket",
                Id = "HuntsmansBasket",
                DisplayName = "Huntsman's Basket",
                MaterialPrefab = "FeastHuntsman_Material",
                PiecePrefab = "FeastHuntsman",
                TokenName = "$item_feasthuntsman",
                TokenDesc = "$item_feasthuntsman_desc",
                Description = "A cloth-lined basket of wolf jerky, eggs, bread and root hash. A feast for the whole hall.",
                DefaultRecipe = "CookedEgg:2,WolfJerky:2,Turnip:2,Bread:1,Onion:1",
                IconFile = "huntsmans-basket.png",
                Health = 40f,
                Stamina = 40f,
                Eitr = 0f,
                Regen = 3f,
                ConfigSection = "HuntsmansBasket"
            },
            new V120Def
            {
                LayoutId = "amber-roost",
                Id = "AmberRoost",
                DisplayName = "The Amber Roost",
                MaterialPrefab = "FeastAmberRoost_Material",
                PiecePrefab = "FeastAmberRoost",
                TokenName = "$item_feastamberroost",
                TokenDesc = "$item_feastamberroost_desc",
                Description = "A honey-glazed roast chicken with ramekins of honey, carrots and bread. A feast for the whole hall.",
                DefaultRecipe = "HoneyGlazedChicken:2,Honey:3,Carrot:2,SpiceMistlands:1",
                IconFile = "amber-roost.png",
                Health = 70f,
                Stamina = 50f,
                Eitr = 0f,
                Regen = 5f,
                ConfigSection = "AmberRoost"
            },
            new V120Def
            {
                LayoutId = "sea-gods-tray",
                Id = "SeaGodsTray",
                DisplayName = "Sea-God's Tray",
                MaterialPrefab = "FeastSeaGods_Material",
                PiecePrefab = "FeastSeaGods",
                TokenName = "$item_feastseagods",
                TokenDesc = "$item_feastseagods_desc",
                Description = "A lacquered tray of maki, nigiri and piled fish, served with a swimmer's mead. Grants Sea-God's Favor. A feast for the whole hall.",
                DefaultRecipe = "FishCooked:2,FishRaw:2,CookedLoxMeat:1,MeadSwimmer:1",
                IconFile = "sea-gods-tray.png",
                Health = 40f,
                Stamina = 60f,
                Eitr = 0f,
                Regen = 3f,
                SeaGodsFavor = true,
                ConfigSection = "SeaGodsTray"
            },
            new V120Def
            {
                LayoutId = "rans-longsmoke",
                Id = "RansLongsmoke",
                DisplayName = "Rán's Longsmoke",
                MaterialPrefab = "FeastRansLongsmoke_Material",
                PiecePrefab = "FeastRansLongsmoke",
                TokenName = "$item_feastranslongsmoke",
                TokenDesc = "$item_feastranslongsmoke_desc",
                Description = "Smoked fish laid on a cedar rack with cloudberries, onion and greens. A feast for the whole hall.",
                DefaultRecipe = "FishCooked:3,Honey:2,Cloudberry:4,Onion:2",
                IconFile = "rans-longsmoke.png",
                Health = 45f,
                Stamina = 50f,
                Eitr = 0f,
                Regen = 3f,
                ConfigSection = "RansLongsmoke"
            },
            new V120Def
            {
                LayoutId = "hall-teinn",
                Id = "HallTeinn",
                DisplayName = "Hall-Teinn",
                MaterialPrefab = "FeastHallTeinn_Material",
                PiecePrefab = "FeastHallTeinn",
                TokenName = "$item_feasthallteinn",
                TokenDesc = "$item_feasthallteinn_desc",
                Description = "Skewers of wolf, neck, lox and boar on a bed of flatbread, served on a sea-grate. A feast for the whole hall.",
                DefaultRecipe = "WolfMeatSkewer:2,NeckTailGrilled:2,CookedLoxMeat:1,CookedMeat:2,Onion:2,Mushroom:2,Bread:2",
                IconFile = "hall-teinn.png",
                Health = 60f,
                Stamina = 45f,
                Eitr = 0f,
                Regen = 4f,
                ConfigSection = "HallTeinn"
            }
        };

        internal static IFeastDish[] CreateDishes()
        {
            IFeastDish[] array = new IFeastDish[All.Length];
            for (int i = 0; i < All.Length; i++)
            {
                array[i] = new V120Dish(All[i]);
            }
            return array;
        }
    }
}
