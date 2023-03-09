namespace Seamoth.Ship;

public struct ShipModuleCraftType
{
    public readonly CraftTree.Type FabricatorType;
    public readonly TechCategory CategoryForPda;
    public readonly TechGroup GroupForPda;

    public ShipModuleCraftType(CraftTree.Type fabricatorType, TechCategory categoryForPda, TechGroup groupForPda)
    {
        FabricatorType = fabricatorType;
        CategoryForPda = categoryForPda;
        GroupForPda = groupForPda;
    }
}