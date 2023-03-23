namespace VehicleFramework.Seamoth;

public class SeamothBehaviour : ModVehicleBehaviour
{
    protected override string EnterVehicleText => "Enter Seamoth";
    protected override ControlSheme ControlScheme => ControlSheme.Submersible;
    protected override float EnergyConsumptionRate => 1f / 15f;
}