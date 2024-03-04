namespace VehicleFrameworkNautilus.Items.Vehicle.Components;

public abstract class HandlerComponent : MonoBehaviour
{
    private BaseVehicleBehaviour _vehicleBehaviour;

    protected BaseVehicleBehaviour VehicleBehaviour
    {
        get
        {
            if (_vehicleBehaviour == null)
            {
                _vehicleBehaviour = gameObject.GetComponent<BaseVehicleBehaviour>();
            }

            return _vehicleBehaviour;
        }
    }
}