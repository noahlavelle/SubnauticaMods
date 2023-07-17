using System.Linq;
using VehicleFrameworkNautilus.Items.Vehicle.Components;
using VehicleFrameworkNautilus.Items.Vehicle.Components.Base;

namespace VehicleFrameworkNautilus.Items.Vehicle;

public abstract class BaseVehiclePrefab
{
    private readonly List<HandlerComponent> _components = new();
    
    /// <summary>
    /// Registers the vehicle as a craftable prefab
    /// </summary>
    public void Register()
    {
        Info = PrefabInfo
            .WithTechType(ClassID, DisplayName, Description)
            .WithIcon(CraftIcon);
        
        var customPrefab = new CustomPrefab(Info);
        
        PrepareGameObject();
        
        customPrefab.SetGameObject(gameObject);
        customPrefab.SetRecipe(Recipe).WithFabricatorType(CraftTree.Type.Constructor).WithStepsToFabricatorTab("Vehicles");

        customPrefab.Register();
        
        Plugin.RegisteredVehicles.Add(Info.TechType, this);
    }


    /// <summary>
    /// Run at vehicle registration to add and configure essential vehicle components. Overriding allows for manipulation to the GameObject<para />
    /// <br />- <see cref="ConstructionVFXHandler" />: Audio and visual effects during vehicle construction (currently not configurable)
    /// <br />- <see cref="T:PrefabIdentifier" />: Required for an object to be considered a prefab.
    /// <br />- <see cref="T:TechTag" />: Required for inventory items, crafting, scanning, etc.
    /// <br />- <see cref="T:LargeWorldEntity" />: Required for objects to persist after saving and exiting.
    /// <br />- <see cref="T:SkyApplier" />: Added if Renderers exist in the hierarchy. Applies the correct lighting onto an object.
    /// </summary>
    protected virtual void PrepareGameObject()
    {
        PrefabUtils.AddBasicComponents(gameObject, ClassID, Info.TechType, LargeWorldEntity.CellLevel.Global);
        AddComponent<ConstructionVFXHandler>();
    }

    /// <summary>
    /// Instantiate and attach a modded handler component to the vehicle
    /// </summary>
    /// <typeparam name="T">Handler type to be attached</typeparam>
    public T AddComponent<T>()
        where T : HandlerComponent
    {
        var component = Activator.CreateInstance(typeof(T), this) as T;
        _components.Add(component);
        component?.Instantiate();
        return component;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Type of the handler component to be fetched</typeparam>
    /// <returns>The attached handler component of type T (present)</returns>
    public T GetComponent<T>()
        where T : HandlerComponent
    {
        return _components.OfType<T>().ToList().FirstOrDefault();
    }

    /* Prefab Settings */
    public abstract string ClassID { get; }
    public abstract string DisplayName { get; }
    public abstract string Description { get; }
    public PrefabInfo Info { get; private set; }
    
    /* Crafting Settings */
    public abstract RecipeData Recipe { get; }
    public abstract float CraftTime { get; }
    public abstract Sprite CraftIcon { get;  }
    /* Vehicle Data */
    public abstract GameObject gameObject { get; }
}