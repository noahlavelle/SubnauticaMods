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
        VehicleBehaviour.movePlayerComp = movePlayer;
        movePlayer.from = _sitPosition;
        movePlayer.to = _exitPosition;
        movePlayer.followTransformMovement = false;
        
        VehicleBehaviour.playerPosition = _sitPosition.gameObject;
        VehicleBehaviour.playerSits = true;
        VehicleBehaviour.leftHandPlug = _leftHandTarget;
        VehicleBehaviour.rightHandPlug = _rightHandTarget;

        VehicleBehaviour.altExitPositions = new Transform[_altPositionsParent.childCount];
        foreach (Transform child in _altPositionsParent)
        {
            VehicleBehaviour.altExitPositions.Add(child);
        }
        
        VehicleBehaviour.exitPosLand = _exitPosition;
        VehicleBehaviour.exitPosWater = _exitPosition;
        VehicleBehaviour.movePlayerComp = movePlayer;
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