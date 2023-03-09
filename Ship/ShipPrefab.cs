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
        private static GameObject _prefab;
        
        public override string AssetsFolder => Path.Combine(Plugin.ModFolderPath, "Assets");
        
        public ShipPrefab(string classId, string friendlyName, string description) : base(classId, friendlyName,
            description)
        {
            
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
            // Skip loading if the prefab is already cached
            if (_prefab != null)
            {
                // Load seamoth model
                _prefab = Plugin.AssetBundle.LoadAsset<GameObject>("SeamothPrefab");

                // Set essential prefab properties
                _prefab.AddComponent<TechTag>().type = TechType;
                _prefab.AddComponent<PrefabIdentifier>().ClassId = ClassID;
            
                //Spawn a seatruck for reference.
                var seamothTask = CraftData.GetPrefabForTechTypeAsync(TechType.SeaTruck);
                yield return seamothTask;
                var seamothRef = seamothTask.GetResult();
                
                // Prevents vehicle disappearance when out of range
                var largeWorldEntity = _prefab.AddComponent<LargeWorldEntity>();
                largeWorldEntity.cellLevel = LargeWorldEntity.CellLevel.Global;
                
                // Add a ping
                var ping = _prefab.AddComponent<PingInstance>();
                ping.pingType = Plugin.SeamothPingType;
                ping.origin = _prefab.FindChild("PingOrigin").transform;
                
                //Unload the prefab to save on resources.
                Resources.UnloadAsset(seamothRef);
            }

            gameObject.Set(_prefab);
            
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
