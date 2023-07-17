namespace VehicleFrameworkNautilus;

public static class VehicleHelper
{
    public static GameObject ReferenceVehicle;

    public static IEnumerator LoadReferenceVehicleAsync()
    {
        var task = GetPrefabForTechTypeAsync(TechType.SeaTruck);
        yield return task;
        ReferenceVehicle = task.GetResult();
    }
}