using System.Collections.Generic;
using SMLHelper.V2.Crafting;

namespace Seamoth.Ship.Modules;

public class ShipDepthModuleMk2 : ShipModuleBase
{
    public ShipDepthModuleMk2() : base(
        "SeamothDepthModuleMK2",
        "Seamoth Depth Module MK2",
        "SeamothDepthModule",
        "Enhances safe diving depth by 300m. Does not stack."
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
                new(Plugin.ShipDepthModuleMk1.TechType, 1),
                new(TechType.PlasteelIngot, 1),
                new(TechType.Magnetite, 2),
                new(TechType.EnameledGlass, 1)
            }
        };
    }
    
    public override TechCategory CategoryForPDA => TechCategory.Workbench;
    public override TechGroup GroupForPDA => TechGroup.Workbench;
    public override CraftTree.Type FabricatorType => CraftTree.Type.Workbench;
}