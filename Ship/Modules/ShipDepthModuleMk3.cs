using System.Collections.Generic;
using SMLHelper.V2.Crafting;

namespace Seamoth.Ship.Modules;

public class ShipDepthModuleMk3 : ShipModuleBase
{
    public ShipDepthModuleMk3() : base(
        new ShipModuleCraftType(CraftTree.Type.Workbench, TechCategory.Workbench, TechGroup.Workbench),
        "SeamothDepthModuleMK3",
        "Seamoth Depth Module MK3",
        "SeamothDepthModule",
        "Enhances safe diving depth by 700m. Does not stack."
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
                new(Plugin.ShipDepthModuleMk2.TechType, 1),
                new(TechType.PlasteelIngot, 1),
                new(TechType.AluminumOxide, 3)
            }
        };
    }
}