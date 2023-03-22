using UnityEngine;

namespace VehicleFramework.VehicleComponents;

public class VehicleSeat: VehicleComponent
{
    private readonly string _sitLocationPath;
    private readonly string _leftHandTargetPath;
    private readonly string _rightHandTargetPath;
    private readonly string _startPositionPath;
    private readonly string _endPositionPath;
    
    private MovePlayer _movePlayer;

    public VehicleSeat(
        string sitLocationPath = "SitLocation", string leftHandTargetPath = "Model/Vehicle_Anim/Joints/LeftIKTarget", string rightHandTargetPath = "Model/Vehicle_Anim/Joints/RightIKTarget",
        string startPositionPath = "StartPosition", string endPositionPath = "EndPosition"
    )
    {
        _sitLocationPath = sitLocationPath;
        _leftHandTargetPath = leftHandTargetPath;
        _rightHandTargetPath = rightHandTargetPath;
        _startPositionPath = startPositionPath;
        _endPositionPath = endPositionPath;
    }

    public override void AddComponent(ModVehicle parentVehicle)
    {
        var sitLocation = parentVehicle.Prefab.transform.Find(_sitLocationPath);
        var leftHandTarget = parentVehicle.Prefab.transform.Find(_leftHandTargetPath);
        var rightHandTarget = parentVehicle.Prefab.transform.Find(_rightHandTargetPath);
        var startPosition = parentVehicle.Prefab.transform.Find(_startPositionPath);
        var endPosition = parentVehicle.Prefab.transform.Find(_endPositionPath);

        _movePlayer = parentVehicle.Prefab.AddComponent<MovePlayer>();

        parentVehicle.VehicleBehaviour.playerPosition = sitLocation.gameObject;
        _movePlayer.from = startPosition;
        _movePlayer.to = endPosition;

        if (parentVehicle.PlayerFullyEntered)
        {
            Player.main.armsController.SetWorldIKTarget(leftHandTarget, rightHandTarget);   
        }
    }
}