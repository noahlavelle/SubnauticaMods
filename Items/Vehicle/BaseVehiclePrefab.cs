using System.Linq;
using VehicleFrameworkNautilus.Items.Vehicle.Components;
using VehicleFrameworkNautilus.Items.Vehicle.Components.Base;
using VehicleFrameworkNautilus.Items.Vehicle.Components.Configurable;

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
        
        ConfigureVehicle();
        
        customPrefab.SetGameObject(Model);
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
    protected virtual void ConfigureVehicle()
    {
        PrefabUtils.AddBasicComponents(Model, ClassID, Info.TechType, LargeWorldEntity.CellLevel.Global);

        ConstructionVFXHandler = AddComponent<ConstructionVFXHandler>();
        UpgradeModulesHandler = AddComponent<UpgradeModulesHandler>();
        PhysicsHandler = AddComponent<PhysicsHandler>();
        PingHandler = AddComponent<PingHandler>();
        EnergyHandler = AddComponent<EnergyHandler>();
        PositionHandler = AddComponent<PositionHandler>();
        EcoTargetHandler = AddComponent<EcoTargetHandler>();
        SoundHandler = AddComponent<SoundHandler>();
        HealthHandler = AddComponent<HealthHandler>();
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

    public T AddBehaviour<T>()
        where T : BaseVehicleBehaviour, new()
    {
        Behaviour = Model.AddComponent<T>();
        return (T)Behaviour;
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
    public abstract GameObject Model { get; }
    public BaseVehicleBehaviour Behaviour { get; private set; }
    
    /* Essential Components */

    public ConstructionVFXHandler ConstructionVFXHandler { get; private set; }
    public UpgradeModulesHandler UpgradeModulesHandler { get; private set; }
    public PhysicsHandler PhysicsHandler { get; private set; }
    public PingHandler PingHandler { get; private set; }
    public EnergyHandler EnergyHandler { get; private set; }
    public PositionHandler PositionHandler { get; private set; }
    public EcoTargetHandler EcoTargetHandler { get; private set; }
    public SoundHandler SoundHandler { get; private set; }
    public HealthHandler HealthHandler { get; private set; }
}