using TMPro;

namespace VehicleFramework.VehicleComponents;

public class VehicleName : VehicleComponent
{
    public override void AddComponent(ModVehicle parentVehicle)
    {
        var colorNameControl = parentVehicle.Prefab.AddComponent<ColorNameControl>();
        var namePlate = parentVehicle.Prefab.AddComponent<NamePlate>();
        var textMeshProUGUI = parentVehicle.Prefab.GetComponentInChildren<TextMeshProUGUI>();

        colorNameControl.defaultName = parentVehicle.FriendlyName;
        colorNameControl.namePlate = namePlate;
        colorNameControl.pingInstance = parentVehicle.Ping;

        namePlate.text = textMeshProUGUI;
        namePlate.isBase = false;
    }
}