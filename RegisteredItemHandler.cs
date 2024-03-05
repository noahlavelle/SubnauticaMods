namespace VehicleFrameworkNautilus.Items;

public abstract class RegisteredItemHandler
{
    public virtual void Register() {}
    
    public abstract string ClassID { get; }
    public abstract string DisplayName { get; }
    public abstract string Description { get; }
    public PrefabInfo Info { get; protected set; }
    public abstract RecipeData Recipe { get; }
    public abstract float CraftTime { get; }
    public abstract Sprite CraftIcon { get;  }
    public abstract GameObject Prefab { get; }
}