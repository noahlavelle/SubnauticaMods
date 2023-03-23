using UnityEngine;

namespace VehicleFramework.VehicleComponents;

public class VehicleSeat: VehicleComponent
{
    private readonly bool _playerSits;
    private readonly string _sitLocationPath;
    private readonly string _leftHandTargetPath;
    private readonly string _rightHandTargetPath;
    private readonly string _endPositionPath;
    
    private MovePlayer _movePlayer;

    public VehicleSeat(bool playerSits,
        string sitLocationPath = "SitLocation", string leftHandTargetPath = "Model/Vehicle_Anim/Joints/LeftIKTarget", string rightHandTargetPath = "Model/Vehicle_Anim/Joints/RightIKTarget",
        string endPositionPath = "EndPosition"
    )
    {
        _playerSits = playerSits;
        _sitLocationPath = sitLocationPath;
        _leftHandTargetPath = leftHandTargetPath;
        _rightHandTargetPath = rightHandTargetPath;
        _endPositionPath = endPositionPath;
    }

    public override void AddComponent(ModVehicle parentVehicle)
    {
        var sitLocation = parentVehicle.Prefab.transform.Find(_sitLocationPath);
        var leftHandTarget = parentVehicle.Prefab.transform.Find(_leftHandTargetPath);
        var rightHandTarget = parentVehicle.Prefab.transform.Find(_rightHandTargetPath);
        var endPosition = parentVehicle.Prefab.transform.Find(_endPositionPath);

        _movePlayer = parentVehicle.Prefab.AddComponent<MovePlayer>();

        parentVehicle.VehicleBehaviour.playerPosition = sitLocation.gameObject;
        parentVehicle.VehicleBehaviour.playerSits = _playerSits;

        parentVehicle.VehicleBehaviour.leftHandPlug = leftHandTarget;
        parentVehicle.VehicleBehaviour.rightHandPlug = rightHandTarget;
        
        _movePlayer.from = sitLocation;
        _movePlayer.to = endPosition;

        parentVehicle.VehicleBehaviour.movePlayerComp = _movePlayer;
    }
}