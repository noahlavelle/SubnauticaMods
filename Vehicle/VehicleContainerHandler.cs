using System.Linq;
using UnityEngine.Events;
using VehicleFrameworkNautilus.Items.Vehicle.Components;
using VehicleFrameworkNautilus.Items.Vehicle.Components.Base;
using VehicleFrameworkNautilus.Items.Vehicle.Components.Configurable;

namespace VehicleFrameworkNautilus.Items.Vehicle;

public abstract class VehicleContainerHandler : RegisteredItemHandler
{
    // private readonly List<HandlerComponent> _components = new();

    /// <summary>
    /// Registers the vehicle as a craftable prefab
    /// </summary>
    public override void Register()
    {
        Info = PrefabInfo
            .WithTechType(ClassID, DisplayName, Description)
            .WithIcon(CraftIcon);
        
        var customPrefab = new CustomPrefab(Info);
        
        ApplyCoreComponents();
        
        customPrefab.SetGameObject(Prefab);
        customPrefab.SetRecipe(Recipe).WithFabricatorType(CraftTree.Type.Constructor).WithStepsToFabricatorTab("Vehicles");
        
        customPrefab.Register();
        
        Plugin.RegisteredVehicles.Add(Info.TechType, this);
        
        // Upgrade Modules Crafting
        CraftTreeHandler.AddTabNode(ModulesFabricatorType, ModulesNodeTabID,  ModulesNodeName, CraftIcon, "Upgrades");
        ModulesEquipmentType = EnumHandler.AddEntry<EquipmentType>(ModulesEquipmentTypeName);
    }

    /// <summary>
    /// Run at vehicle registration to add and configure essential vehicle components.<para />
    /// <br />- <see cref="ConstructionVFXHandler" />: Audio and visual effects during vehicle construction (currently not configurable)
    /// <br />- <see cref="T:PrefabIdentifier" />: Required for an object to be considered a prefab.
    /// <br />- <see cref="T:TechTag" />: Required for inventory items, crafting, scanning, etc.
    /// <br />- <see cref="T:LargeWorldEntity" />: Required for objects to persist after saving and exiting.
    /// <br />- <see cref="T:SkyApplier" />: Added if Renderers exist in the hierarchy. Applies the correct lighting onto an object.
    /// </summary>

    private void ApplyCoreComponents()
    {
        ConstructionVFXHandler = AddHandler<ConstructionVFXHandler>();
        UpgradeModulesInputHandler = AddHandler<UpgradeModulesInputHandler>();
        PhysicsHandler = AddHandler<PhysicsHandler>();
        PingHandler = AddHandler<PingHandler>();
        EnergyHandler = AddHandler<EnergyHandler>();
        PositionHandler = AddHandler<PositionHandler>();
        EcoTargetHandler = AddHandler<EcoTargetHandler>();
        SoundHandler = AddHandler<SoundHandler>();
        HealthHandler = AddHandler<HealthHandler>();
        CrushDepthHandler = AddHandler<CrushDepthHandler>();
        
        PrefabUtils.AddBasicComponents(Prefab, ClassID, Info.TechType, LargeWorldEntity.CellLevel.Global);
        
        foreach (var skyApplier in Prefab.GetComponentsInChildren<SkyApplier>())
        {
            skyApplier.dynamic = true;
        }
    }

    public T AddHandler<T>()
        where T : HandlerComponent
    {
        var component = Prefab.AddComponent<T>();
        return component;
    }

    public T SetBehaviour<T>()
        where T : VehicleBehaviourHandler
    {
        var behaviour = Prefab.AddComponent<T>();
        BehaviourHandler = behaviour;
        
        return behaviour;
    }
    
    public VehicleBehaviourHandler BehaviourHandler { get; private set; }
    
    /* Essential Components */

    public ConstructionVFXHandler ConstructionVFXHandler { get; private set; }
    public UpgradeModulesInputHandler UpgradeModulesInputHandler { get; private set; }
    public PhysicsHandler PhysicsHandler { get; private set; }
    public PingHandler PingHandler { get; private set; }
    public EnergyHandler EnergyHandler { get; private set; }
    public PositionHandler PositionHandler { get; private set; }
    public EcoTargetHandler EcoTargetHandler { get; private set; }
    public SoundHandler SoundHandler { get; private set; }
    public HealthHandler HealthHandler { get; private set; }
    public CrushDepthHandler CrushDepthHandler { get; private set; }
    
    // Upgrade Modules
    public abstract CraftTree.Type ModulesFabricatorType { get; }
    public abstract string ModulesNodeTabID { get; }
    protected abstract string ModulesNodeName { get; }
    protected abstract string ModulesEquipmentTypeName { get;  }
    
    public EquipmentType ModulesEquipmentType { get; protected set; }
    
}