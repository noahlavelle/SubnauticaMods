using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Seamoth;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Utility;
using UnityEngine;

namespace Seamoth.Ship
{
    public class ShipPrefab : Craftable
    {
        public override string AssetsFolder => Path.Combine(base.AssetsFolder, "Assets");
        
        public ShipPrefab(string classId, string friendlyName, string description) : base(classId, friendlyName,
            description)
        {
            OnFinishedPatching += () =>
            {
                KnownTechHandler.SetAnalysisTechEntry(TechType, Array.Empty<TechType>());
            };
        }


        protected override RecipeData GetBlueprintRecipe()
        {
            return new RecipeData
            {
                Ingredients =  new List<Ingredient>()
                {
                    new(TechType.TitaniumIngot, 1),
                    new(TechType.PowerCell, 1),
                    new(TechType.Glass, 2),
                    new(TechType.Lubricant, 1),
                    new(TechType.Lead, 1)
                },
                craftAmount = 1
            };
        }

        protected override Sprite GetItemSprite()
        {
            return Plugin.LoadSprite(Path.Combine(AssetsFolder, "SeamothCraftIcon.png"));
        }

        public override TechCategory CategoryForPDA => TechCategory.Constructor;
        public override TechGroup GroupForPDA => TechGroup.Constructor;
        public override string[] StepsToFabricatorTab => new[] { "Vehicles" };
        public override CraftTree.Type FabricatorType => CraftTree.Type.Constructor;
        public override bool UnlockedAtStart => true;
    }  
}
