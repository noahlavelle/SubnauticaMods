namespace VehicleFrameworkNautilus.Items.Vehicle.Components;

public abstract class HandlerComponent : MonoBehaviour
{
    private VehicleBehaviourHandler _vehicleBehaviourHandler;

    protected VehicleBehaviourHandler VehicleBehaviourHandler
    {
        get
        {
            if (_vehicleBehaviourHandler == null)
            {
                _vehicleBehaviourHandler = gameObject.GetComponent<VehicleBehaviourHandler>();
            }

            return _vehicleBehaviourHandler;
        }
    }
}