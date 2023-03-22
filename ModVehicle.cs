using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using UnityEngine;
using VehicleFramework.VehicleComponents;

namespace VehicleFramework;

public abstract class ModVehicle : Craftable
{
    public bool PlayerFullyEntered;
    public Rigidbody Rigidbody;
    public PingInstance Ping;
    public ModVehicleBehaviour VehicleBehaviour;
    public GameObject Prefab;
    public GameObject ReferencePrefab;
    
    private GameObject _vehicleModel;

    public ModVehicle(string classId, string name, string description, GameObject model) : base(classId, name, description)
    {
        VehicleComponents = new List<VehicleComponent>();
        _vehicleModel = model;
    }

    public T AddVehicleBehaviour<T>() where T : ModVehicleBehaviour, new()
    {
        var behaviour = Prefab.AddComponent<T>();
        VehicleBehaviour = behaviour;
        return behaviour;
    }

    public T AddVehicleComponent<T>(T component) where T : VehicleComponent
    {
        VehicleComponents.Add(component);
        return component;
    }

    public T GetVehicleComponentOfType<T>() where T : VehicleComponent
    {
        var result = VehicleComponents.OfType<T>().ToList();
        if (!result.Any())
        {
            return null;
        }

        return result.First();
    }
    
    public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
    {
        if (ReferencePrefab == null)
        {
            var referenceTask = CraftData.GetPrefabForTechTypeAsync(TechType.Exosuit);
            yield return referenceTask;
            ReferencePrefab = referenceTask.GetResult();
        }
        
        if (Prefab == null)
        {
            Prefab = _vehicleModel;
            ApplyMarmosetMaterial();
            PrefabConstructionInitialised();

            Prefab.AddComponent<TechTag>().type = TechType;
            Prefab.AddComponent<PrefabIdentifier>().ClassId = ClassID;
            Prefab.AddComponent<OutOfBoundsWarp>();
            foreach (var component in VehicleComponents)
            {
                component.AddComponent(this);
            }
        }
        
        gameObject.Set(Prefab);
        yield return null;
    }

    protected override Sprite GetItemSprite()
    {
        return ItemSprite;
    }

    protected override RecipeData GetBlueprintRecipe()
    {
        return BlueprintRecipe;
    }

    protected abstract void PrefabConstructionInitialised();

    private void ApplyMarmosetMaterial()
    {
        var marmosetShader = ReferencePrefab.transform.Find("exosuit_01/exosuit_body_geo")
            .GetComponent<Renderer>().material.shader;
        foreach (var renderer in Prefab.GetComponentsInChildren<Renderer>())
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

    public abstract Sprite ItemSprite { get; }
    public abstract RecipeData BlueprintRecipe { get; }
    public List<VehicleComponent> VehicleComponents { get; }
}