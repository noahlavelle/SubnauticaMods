using System.Collections.Generic;
using SMLHelper.V2.Crafting;

namespace Seamoth.Ship.Modules;

public class ShipSonarModule : ShipModuleBase
{
    public ShipSonarModule() : base(
        "SeamothSonarModule",
        "Seamoth Sonar",
        "SeamothSonarModule",
        "A dedicated system for detecting and displaying topographical data on the HUD."
    )
    {
        
    }

    protected override RecipeData GetBlueprintRecipe()
    {
        return new RecipeData
        {
            craftAmount = 1,
            Ingredients = new List<Ingredient>
            {
                new(TechType.CopperWire, 1),
                new(TechType.Magnetite, 2),
            }
        };
    }
    
    public override TechCategory CategoryForPDA => TechCategory.VehicleUpgrades;
    public override TechGroup GroupForPDA => TechGroup.VehicleUpgrades;
    public override string[] StepsToFabricatorTab => new[] { "Upgrades", "SeamothUpgrades" };
    public override CraftTree.Type FabricatorType => CraftTree.Type.Fabricator;
}