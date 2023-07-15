using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VehicleFramework.VehicleComponents;

public class VehicleDockable : VehicleComponent
{
    public readonly Vector3 DockingEndPoint;
    public readonly AnimatorOverrideController OverrideController;
    public AnimatorOverrideController PlayerOverrideController;
    
    public readonly AnimationClip DockingAnimation;
    public readonly AnimationClip DockingLoopAnimation;
    public readonly AnimationClip LaunchLeftAnimation;
    public readonly AnimationClip LaunchRightAnimation;
    public readonly AnimationClip PlayerDockingAnimation;

    private ColorCustomizer _colorCustomizer;
    private float _dockingUnlockDistance;

    public VehicleDockable(Vector3 dockingEndPoint, AnimationClip dockingAnimation, AnimationClip dockingLoopAnimation, AnimationClip launchLeftAnimation, AnimationClip launchRightAnimation, AnimationClip playerDockingAnimation, float dockingUnlockDistance = 5)
    {
        DockingEndPoint = dockingEndPoint;
        DockingAnimation = dockingAnimation;
        DockingLoopAnimation = dockingLoopAnimation;
        LaunchLeftAnimation = launchLeftAnimation;
        LaunchRightAnimation = launchRightAnimation;
        PlayerDockingAnimation = playerDockingAnimation;
        _dockingUnlockDistance = dockingUnlockDistance;

        OverrideController = new AnimatorOverrideController();
        PlayerOverrideController = new AnimatorOverrideController();
    }

    public override void AddComponent(ModVehicle parentVehicle)
    {
        var vehiclePhysics = parentVehicle.GetVehicleComponentOfType<VehiclePhysics>();
        if (vehiclePhysics == null)
        {
            Plugin.Log.LogError("VehicleDocking requires VehiclePhysics component");
            return;
        }

        var dockable = parentVehicle.Prefab.AddComponent<Dockable>();
        dockable.rb = vehiclePhysics.Rigidbody;
        dockable.vehicle = parentVehicle.VehicleBehaviour;
        
        _colorCustomizer = parentVehicle.Prefab.AddComponent<ColorCustomizer>();
        _colorCustomizer.isBase = false;

        parentVehicle.VehicleBehaviour.dockable = dockable;

        var redockLock = parentVehicle.Prefab.AddComponent<VehicleRedockLock>();
        redockLock.unlockDistance = _dockingUnlockDistance;

        PlayerOverrideController.runtimeAnimatorController = Player.main.playerAnimator.runtimeAnimatorController;

        var playerDockAnimationOriginal =
        Player.main.playerAnimator.runtimeAnimatorController.animationClips.First(a =>
            a.name == "player_seatruck_moonpool_dock");

        PlayerOverrideController.ApplyOverrides(new List<KeyValuePair<AnimationClip, AnimationClip>>
        {
            new(playerDockAnimationOriginal, PlayerDockingAnimation)
        });
    }

    public class VehicleRedockLock : MonoBehaviour
    {
        private bool _locked;
        private BoxCollider _dockingBayTrigger;

        public float unlockDistance = 5;

        public bool IsLocked()
        {
            return _locked;
        }

        private bool CheckVehicleColliderStillTouching()
        {
            if (!_dockingBayTrigger) return true;
            var distance = Vector3.Distance(this.transform.position, _dockingBayTrigger.transform.position);
            return distance < unlockDistance;
        }

        private void Start()
        {
            _locked = false;
            _dockingBayTrigger = null;
        }

        private void Update()
        {
            if (!IsLocked()) return;
            if (!CheckVehicleColliderStillTouching()) Unlock();
        }

        public void Unlock()
        {
            _locked = false;
            _dockingBayTrigger = null;
            enabled = false;
        }

        public void Lock(VehicleDockingBay dockingBay)
        {
            _locked = true;
            _dockingBayTrigger = dockingBay.GetComponent<BoxCollider>();
            enabled = true;
        }
    }
}