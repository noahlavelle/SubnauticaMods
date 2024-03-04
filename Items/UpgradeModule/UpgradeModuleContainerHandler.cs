using VehicleFrameworkNautilus.Items.Vehicle;

namespace VehicleFrameworkNautilus.Items.UpgradeModule;

public abstract class UpgradeModuleContainerHandler : RegisteredItemHandler
{
    public abstract string EquipText { get; }
    public abstract string RemoveText { get; }
    public override GameObject Prefab { get; }
    public VehicleContainerHandler VehicleContainer { get; set; }
    
    
    public void Register()
    {
        Info = PrefabInfo.WithTechType(ClassID, DisplayName, Description)
            .WithIcon(CraftIcon);

        CustomPrefab prefab = new CustomPrefab(Info);

        var clone = new CloneTemplate(Info, TechType.HullReinforcementModule);
        prefab.SetGameObject(clone);

        prefab.SetRecipe(Recipe)
            .WithFabricatorType(VehicleContainer.ModulesFabricatorType)
            .WithStepsToFabricatorTab("Upgrades", VehicleContainer.ModulesNodeTabID)
            .WithCraftingTime(CraftTime);


        var gadget = prefab.SetVehicleUpgradeModule()
            .WithOnModuleAdded((global::Vehicle _, int _) =>
            {
                Subtitles.Add(EquipText);
            })
            .WithOnModuleRemoved((global::Vehicle _, int _) =>
            {
                Subtitles.Add(RemoveText);
            });
        
        ConfigureGadget(gadget);
        
        prefab.Register();
    }

    protected abstract void ConfigureGadget(UpgradeModuleGadget gadget);
}