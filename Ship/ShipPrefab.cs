using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using TMPro;
using UnityEngine;

namespace Seamoth.Ship
{
    public class ShipPrefab : Craftable
    {
        private GameObject _prefab;
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
            if (_prefab == null)
            {
                // Load seamoth model
                _prefab = Plugin.AssetBundle.LoadAsset<GameObject>("SeamothPrefab.prefab");

                // Set essential prefab properties
                _prefab.AddComponent<TechTag>().type = TechType;
                _prefab.AddComponent<PrefabIdentifier>().ClassId = ClassID;

                var rigidbody = _prefab.AddComponent<Rigidbody>();
                InitShipRigidbody(rigidbody);

                // Necessary for buoyancy
                var worldForces = _prefab.AddComponent<WorldForces>();
                InitShipWorldForces(worldForces, rigidbody);

                //Spawn a seatruck for vfx.
                var seatruckTask = CraftData.GetPrefabForTechTypeAsync(TechType.SeaTruck);
                yield return seatruckTask;
                var seatruckRef = seatruckTask.GetResult();
                
                // Construction vfx
                var vfxConstructing = _prefab.AddComponent<VFXConstructing>();
                var seatruckVfx = seatruckRef.GetComponent<VFXConstructing>();
                InitShipVfx(vfxConstructing, seatruckVfx);

                // Prevents vehicle disappearance when out of range
                var largeWorldEntity = _prefab.AddComponent<LargeWorldEntity>();
                InitShipLwe(largeWorldEntity);

                // Add a ping
                var ping = _prefab.AddComponent<PingInstance>();
                InitShipPing(ping, _prefab.FindChild("PingOrigin").transform);
                
                // Set ship statistics
                var liveMixin = _prefab.AddComponent<LiveMixin>();
                InitShipLiveMixin(liveMixin);
                
                // Build bot paths
                var buildBots = _prefab.AddComponent<BuildBotBeamPoints>();
                InitShipBuildBots(buildBots);
                
                // Lighting Controllers
                var lightingController = _prefab.transform.Find("lights_parent").gameObject.AddComponent<LightingController>();
                InitLightingControllers(lightingController);
                
                // Sky Appliers
                var skyApplier = _prefab.AddComponent<SkyApplier>();
                var seatruckSkyApplier = seatruckRef.GetComponent<SkyApplier>();
                var renderers = _prefab.GetComponentsInChildren<Renderer>();
                InitSkyApplier(skyApplier, renderers, seatruckSkyApplier.customSkyPrefab);
                
                // Nameplate
                var colorNameControl = _prefab.AddComponent<ColorNameControl>();
                var namePlate = _prefab.AddComponent<NamePlate>();
                var textMeshProUGUI = _prefab.transform.Find("NameText/Text").GetComponent<TextMeshProUGUI>();
                InitShipNamePlate(colorNameControl, namePlate, ping, textMeshProUGUI);
                
                // Moonpool docking
                var dockable = _prefab.AddComponent<Dockable>();
                InitDockable(dockable, rigidbody);
                
                // Moonpool colour customisation
                var colorCustomizer = _prefab.AddComponent<ColorCustomizer>();
                // Needs no initialisation

                // Collision Damage
                var dealDamageOnImpact = _prefab.AddComponent<DealDamageOnImpact>();
                InitDealDamageOnImpact(dealDamageOnImpact);

                // Change all materials to MarmosetUBER
                ApplyMaterials(_prefab, seatruckRef);
            }
            
            gameObject.Set(_prefab);
            yield return null;
        }

        // Ship configuration
        private static void InitShipRigidbody(in Rigidbody rigidbody)
        {
            rigidbody.mass = 800;
            rigidbody.useGravity = false;
            rigidbody.drag = 2;
            rigidbody.angularDrag = 4;
        }
        
        private static void InitShipWorldForces(in WorldForces worldForces, Rigidbody rigidbody)
        {
            worldForces.useRigidbody = rigidbody;
            worldForces.underwaterGravity = 0f;
            worldForces.aboveWaterGravity = 9.81f;
            worldForces.waterDepth = -5f;
        }
        
        private static void InitShipVfx(in VFXConstructing vfx, VFXConstructing referenceVfx)
        {
            vfx.ghostMaterial = referenceVfx.ghostMaterial;
            vfx.constructSound = referenceVfx.constructSound;
            vfx.surfaceSplashSound = referenceVfx.surfaceSplashSound;
            vfx.surfaceSplashFX = referenceVfx.surfaceSplashFX;
            vfx.alphaTexture = referenceVfx.alphaTexture;
            vfx.alphaDetailTexture = referenceVfx.alphaDetailTexture;
            vfx.delay = 3f;
            vfx.timeToConstruct = 15f;
            // referenceVfx.CopyAllTo(vfx);
            vfx.Regenerate();
        }

        private static void InitShipLwe(in LargeWorldEntity lwe)
        {
            lwe.cellLevel = LargeWorldEntity.CellLevel.Global;
        }

        private static void InitShipPing(in PingInstance ping, Transform origin)
        {
            ping.pingType = Plugin.SeamothPingType;
            ping.origin = origin;
        }

        private static void InitShipLiveMixin(in LiveMixin liveMixin)
        {
            liveMixin.health = 200;
            liveMixin.tempDamage = -1;
            
            var lmData = ScriptableObject.CreateInstance<LiveMixinData>();
            lmData.broadcastKillOnDeath = true;
            lmData.destroyOnDeath = false;
            lmData.weldable = true;
            lmData.maxHealth = 200f;
            liveMixin.data = lmData;
        }

        private void InitShipBuildBots(in BuildBotBeamPoints buildBots)
        {
            var beamPointsParent = _prefab.FindChild("buildbotbeampoints").transform;
            var beamPathsParent = _prefab.FindChild("build_bot_paths").transform;
                
            buildBots.beamPoints = Enumerable.Range(0, beamPointsParent.childCount).Select(beamPointsParent.GetChild).ToArray();
            Enumerable.Range(0, beamPathsParent.childCount)
                .ForEach(i => CreateBuildBotPath(beamPathsParent.GetChild(i)));
        }

        private static void InitLightingControllers(in LightingController lightingController)
        {
            lightingController.lights = Array.Empty<MultiStatesLight>();
            lightingController.state = LightingController.LightingState.Operational;
            foreach (Transform child in lightingController.transform)
            {
                var newLight = new MultiStatesLight
                {
                    light = child.GetComponent<Light>(),
                    intensities = new[] { 1f, 0.5f, 0f }  //Full power: intensity 1. Emergency : intensity 0.5. No power: intensity 0.
                };
                lightingController.RegisterLight(newLight);
            }
        }
        
        private static void InitSkyApplier(in SkyApplier skyApplier, IReadOnlyCollection<Renderer> renderers, GameObject customSkyPrefab)
        {
            skyApplier.anchorSky = Skies.Custom;
            skyApplier.customSkyPrefab = customSkyPrefab;
            skyApplier.customSkyPrefab = null;
            skyApplier.dynamic = false;
            skyApplier.emissiveFromPower = true;
            skyApplier.renderers = new Renderer[renderers.Count];
            foreach(var renderer in renderers)
            {
                if (renderer.gameObject.name.Contains("interior") || renderer.gameObject.name.Contains("hatch"))
                {
                    skyApplier.renderers.Append(renderer);
                }
            }
        }

        private static void InitShipNamePlate(in ColorNameControl colorNameControl, NamePlate namePlate, PingInstance pingInstance, TextMeshProUGUI textMeshProUGUI)
        {
            colorNameControl.defaultName = "Seamoth";
            colorNameControl.namePlate = namePlate;
            colorNameControl.pingInstance = pingInstance;

            namePlate.text = textMeshProUGUI;
            namePlate.isBase = false;
        }

        private static void InitDockable(in Dockable dockable, Rigidbody rigidbody)
        {
            dockable.rb = rigidbody;
            // dockable.powerRelay
        }

        private static void InitDealDamageOnImpact(in DealDamageOnImpact dealDamageOnImpact)
        {
            dealDamageOnImpact.speedMinimumForSelfDamage = 4;
            dealDamageOnImpact.speedMinimumForDamage = 2;
            dealDamageOnImpact.affectsEcosystem = true;
            dealDamageOnImpact.allowDamageToPlayer = false;
        }

        // Crafting Properties
        protected override Sprite GetItemSprite()
        {
            return Plugin.AssetBundle.LoadAsset<Sprite>("SeamothCraftIcon");
        }

        private void CreateBuildBotPath(Transform parent)
        {
            var path = _prefab.AddComponent<BuildBotPath>();
            path.points = Enumerable.Range(0, parent.childCount).Select(parent.GetChild).ToArray();
        }

        public override TechCategory CategoryForPDA => TechCategory.Constructor;
        public override TechGroup GroupForPDA => TechGroup.Constructor;
        public override string[] StepsToFabricatorTab => new[] { "Vehicles" };
        public override CraftTree.Type FabricatorType => CraftTree.Type.Constructor;
        public override bool UnlockedAtStart => true;
        public override float CraftingTime => 9f;

        private static void ApplyMaterials(GameObject vehicle, GameObject refVehicle)
        {
            var modelParent = vehicle.transform.Find("Model/Submersible_SeaMoth/Submersible_seaMoth_geo");
            var marmosetShader = refVehicle.transform.Find("model/seatruck_anim/Seatruck_cabin_exterior_geo")
                .GetComponent<Renderer>().material.shader;
            foreach (var renderer in modelParent.GetComponentsInChildren<Renderer>())
            {
                foreach (var material in renderer.materials)
                {
                    material.shader = marmosetShader;

                    if (!material.name.Contains("Glass")) continue;
                    material.EnableKeyword("_ZWRITE_ON");
                    material.EnableKeyword("WBOIT");
                    material.SetInt("_ZWrite", 0);
                    material.SetInt("_Cutoff", 0);
                    material.SetFloat("_SrcBlend", 1f);
                    material.SetFloat("_DstBlend", 1f);
                    material.SetFloat("_SrcBlend2", 0f);
                    material.SetFloat("_DstBlend2", 10f);
                    material.SetFloat("_AddSrcBlend", 1f);
                    material.SetFloat("_AddDstBlend", 1f);
                    material.SetFloat("_AddSrcBlend2", 0f);
                    material.SetFloat("_AddDstBlend2", 10f);
                    material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack | MaterialGlobalIlluminationFlags.RealtimeEmissive;
                    material.renderQueue = 3101;
                    material.enableInstancing = true;
                }
            }
        }
    }
}
