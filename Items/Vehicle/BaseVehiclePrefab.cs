using System.Linq;
using VehicleFrameworkNautilus.Items.Vehicle.Components;
using VehicleFrameworkNautilus.Items.Vehicle.Components.Base;

namespace VehicleFrameworkNautilus.Items.Vehicle;

public abstract class BaseVehiclePrefab
{
    private readonly List<IHandlerComponent> _components = new();
    
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
        
        customPrefab.SetGameObject(BlankVehicleModel);
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
        PrefabUtils.AddBasicComponents(BlankVehicleModel, ClassID, Info.TechType, LargeWorldEntity.CellLevel.Global);
        AddComponent<ConstructionVFXHandler>();
    }

    /// <summary>
    ///  Attach an existing instance of a modded handler component to the vehicle
    /// </summary>
    /// <param name="component">Instance of the handler component</param>
    public void AddComponent<T>(T component)
        where T : IHandlerComponent
    {
        component.GameObject = BlankVehicleModel;
        _components.Add(component);
        component.Instantiate();
    }
    
    /// <summary>
    /// Instantiate and attach a modded handler component to the vehicle
    /// </summary>
    /// <typeparam name="T">Handler type to be attached</typeparam>
    public void AddComponent<T>()
        where T : IHandlerComponent, new()
    {
        var component = new T
        {
            GameObject = BlankVehicleModel
        };
        
        _components.Add(component);
        component.Instantiate();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Type of the handler component to be fetched</typeparam>
    /// <returns>The attached handler component of type T (present)</returns>
    public T GetComponent<T>()
        where T : IHandlerComponent
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
    public abstract GameObject BlankVehicleModel { get; }
}