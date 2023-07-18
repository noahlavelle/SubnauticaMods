using Mono.Cecil;

namespace VehicleFrameworkNautilus.Items.Vehicle.Components.Configurable;

public class PositionHandler : HandlerComponent
{
    private MovePlayer _movePlayer;

    public override void Instantiate()
    {
        _movePlayer = parentVehicle.Model.AddComponent<MovePlayer>();
    }

    public PositionHandler WithPositions(Transform sitPosition, Transform exitPosition, Transform leftHandTarget, Transform rightHandTarget, Transform altPositionsParent)
    {
        _movePlayer.from = sitPosition;
        _movePlayer.to = exitPosition;
        _movePlayer.followTransformMovement = false;

        parentVehicle.Behaviour.movePlayerComp = _movePlayer;
        parentVehicle.Behaviour.playerPosition = sitPosition.gameObject;
        parentVehicle.Behaviour.playerSits = true;
        parentVehicle.Behaviour.leftHandPlug = leftHandTarget;
        parentVehicle.Behaviour.rightHandPlug = rightHandTarget;
        
        parentVehicle.Behaviour.altExitPositions = new Transform[altPositionsParent.childCount];
        foreach (Transform child in altPositionsParent)
        {
            parentVehicle.Behaviour.altExitPositions.Add(child);
        }
        
        parentVehicle.Behaviour.exitPosLand = exitPosition;
        parentVehicle.Behaviour.exitPosWater = exitPosition;
        parentVehicle.Behaviour.movePlayerComp = _movePlayer;
        
        return this;
    }
    
    public PositionHandler(BaseVehiclePrefab parentVehicle) : base(parentVehicle) { }
}