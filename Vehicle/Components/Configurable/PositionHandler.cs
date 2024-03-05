using Mono.Cecil;

namespace VehicleFrameworkNautilus.Items.Vehicle.Components.Configurable;

public class PositionHandler : HandlerComponent
{
    [SerializeField] private Transform _sitPosition;
    [SerializeField] private Transform _exitPosition;
    [SerializeField] private Transform _leftHandTarget;
    [SerializeField] private Transform _rightHandTarget;
    [SerializeField] private Transform _altPositionsParent;
    
    public void Awake()
    {
        var movePlayer = gameObject.AddComponent<MovePlayer>();
        VehicleBehaviourHandler.movePlayerComp = movePlayer;
        movePlayer.from = _sitPosition;
        movePlayer.to = _exitPosition;
        movePlayer.followTransformMovement = false;
        
        VehicleBehaviourHandler.playerPosition = _sitPosition.gameObject;
        VehicleBehaviourHandler.playerSits = true;
        VehicleBehaviourHandler.leftHandPlug = _leftHandTarget;
        VehicleBehaviourHandler.rightHandPlug = _rightHandTarget;

        VehicleBehaviourHandler.altExitPositions = new Transform[_altPositionsParent.childCount];
        foreach (Transform child in _altPositionsParent)
        {
            VehicleBehaviourHandler.altExitPositions.Add(child);
        }
        
        VehicleBehaviourHandler.exitPosLand = _exitPosition;
        VehicleBehaviourHandler.exitPosWater = _exitPosition;
        VehicleBehaviourHandler.movePlayerComp = movePlayer;
    }

    public PositionHandler WithPositions(Transform sitPosition, Transform exitPosition, Transform leftHandTarget, Transform rightHandTarget, Transform altPositionsParent)
    {
        _sitPosition = sitPosition;
        _exitPosition = exitPosition;

        _leftHandTarget = leftHandTarget;
        _rightHandTarget = rightHandTarget;

        _altPositionsParent = altPositionsParent;
        

        
        return this;
    }
}