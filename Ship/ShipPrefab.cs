using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using UnityEngine;
using UnityEditor;

namespace Seamoth.Ship
{
    public class ShipPrefab : Craftable
    {
        public override string AssetsFolder => Path.Combine(Plugin.ModFolderPath, "Assets");
        
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
                craftAmount = 1,
                Ingredients =  new List<Ingredient>()
                {
                    new(TechType.TitaniumIngot, 1),
                    new(TechType.PowerCell, 1),
                    new(TechType.Glass, 2),
                    new(TechType.Lubricant, 1),
                    new(TechType.Lead, 1)
                }
            };
        }

        public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
        {
            var taskResult = new TaskResult<GameObject>();
            yield return CraftData.InstantiateFromPrefabAsync(TechType.Exosuit, taskResult);
            var prefab = taskResult.Get();

            var techTag = prefab.GetComponent<TechTag>();
            var prefabIdentifier = prefab.GetComponent<PrefabIdentifier>();

            techTag.type = TechType;
            prefabIdentifier.ClassId = ClassID;
            gameObject.Set(prefab);
        }

        protected override Sprite GetItemSprite()
        {
            return Plugin.AssetBundle.LoadAsset<Sprite>("SeamothCraftIcon");
        }

        public override TechCategory CategoryForPDA => TechCategory.Constructor;
        public override TechGroup GroupForPDA => TechGroup.Constructor;
        public override string[] StepsToFabricatorTab => new[] { "Vehicles" };
        public override CraftTree.Type FabricatorType => CraftTree.Type.Constructor;
        public override bool UnlockedAtStart => true;
        public override float CraftingTime => 15f;
    }
}
