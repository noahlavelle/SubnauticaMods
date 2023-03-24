namespace VehicleFramework.Seamoth;

public class SeamothBehaviour : ModVehicleBehaviour
{
    protected override string EnterVehicleText => "Enter Seamoth";
    protected override ControlSheme ControlScheme => ControlSheme.Submersible;
    protected override float EnergyConsumptionRate => 1f / 15f;
    protected override float ForwardForce => 12.5f;
    protected override float BackwardForce => 5.4f;
    protected override float SidewardForce => 12.52f;
    protected override float VerticalForce => 11.93f;
}