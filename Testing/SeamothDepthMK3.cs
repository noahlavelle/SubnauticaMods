using VehicleFrameworkNautilus.Items.UpgradeModule;
using VehicleFrameworkNautilus.Items.Vehicle;
using VehicleFrameworkNautilus.Items.Vehicle.Components.Configurable;

namespace VehicleFrameworkNautilus.Testing;

public class SeamothDepthMK3 : UpgradeModuleContainerHandler
{
    public override string ClassID => "SeamothDepthModule3";
    public override string DisplayName => "Seamoth Depth Module MK3";
    public override string Description => "Enhances safe diving depth by 700m. Does not stack.";
    public override float CraftTime => 5f;
    public override Sprite CraftIcon => null;

    public override string EquipText => "Crush depth now 900 meters";
    public override string RemoveText => "Crush depth now 200 meters";
    protected override void ConfigureGadget(UpgradeModuleGadget gadget)
    {
        gadget.WithDepthUpgrade(700);
    }

    public override RecipeData Recipe => new()
    {
        craftAmount = 1,
        Ingredients =
        {
            new Ingredient(TechType.TitaniumIngot, 1)
        }
    };
}