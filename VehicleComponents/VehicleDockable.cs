using UnityEngine;

namespace VehicleFramework.VehicleComponents;

public class VehicleDockable : VehicleComponent
{
    public readonly string AnimationName;
    public readonly Vector3 DockingEndPoint;
    private ColorCustomizer _colorCustomizer;
    private float _dockingUnlockDistance;

    public VehicleDockable(string animationName, Vector3 dockingEndPoint, float dockingUnlockDistance = 5)
    {
        AnimationName = animationName;
        DockingEndPoint = dockingEndPoint;
        _dockingUnlockDistance = dockingUnlockDistance;
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