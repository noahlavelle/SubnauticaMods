namespace VehicleFrameworkNautilus.Items.Vehicle.Components;

public interface IHandlerComponent
{
    public GameObject GameObject { get; set; }
    public void Instantiate();
}