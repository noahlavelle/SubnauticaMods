using System;
using Mono.Cecil;
using UnityEngine;

namespace VehicleFramework.VehicleComponents;

public class VehicleSeat: VehicleComponent
{
    private readonly bool _playerSits;
    private readonly string _sitLocationPath;
    private readonly string _leftHandTargetPath;
    private readonly string _rightHandTargetPath;
    private readonly string _endPositionPath;
    private readonly string _altExitPositionsPath;

    private MovePlayer _movePlayer;

    public VehicleSeat(bool playerSits,
        string sitLocationPath = "Model/Vehicle_Anim/Joints/AttachJoint/CameraPivot", string leftHandTargetPath = "Model/Vehicle_Anim/Joints/Wheel_Base/Wheel_Segment_1/Wheel_Segment_2/Wheel_Head/LeftIKTarget",
        string rightHandTargetPath = "Model/Vehicle_Anim/Joints/Wheel_Base/Wheel_Segment_1/Wheel_Segment_2/Wheel_Head/RightIKTarget", string endPositionPath = "EndPosition", string altExitPositionsPath = "AlternateExits"
    )
    {
        _playerSits = playerSits;
        _sitLocationPath = sitLocationPath;
        _leftHandTargetPath = leftHandTargetPath;
        _rightHandTargetPath = rightHandTargetPath;
        _endPositionPath = endPositionPath;
        _altExitPositionsPath = altExitPositionsPath;
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
        _movePlayer.followTransformMovement = false;

        var altExitPositionsParent = parentVehicle.Prefab.transform.Find(_altExitPositionsPath);
        parentVehicle.VehicleBehaviour.altExitPositions = new Transform[altExitPositionsParent.childCount];
        foreach (Transform child in parentVehicle.Prefab.transform.Find(_altExitPositionsPath))
        {
            parentVehicle.VehicleBehaviour.altExitPositions.Add(child);
        }
        
        parentVehicle.VehicleBehaviour.exitPosLand = endPosition;
        parentVehicle.VehicleBehaviour.exitPosWater = endPosition;
        parentVehicle.VehicleBehaviour.movePlayerComp = _movePlayer;
    }
}