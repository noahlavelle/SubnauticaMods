using System.Linq;

namespace VehicleFrameworkNautilus.Items.Vehicle.Components.Configurable;

public class DockingHandler : HandlerComponent
{
    public enum DockingExitSide
    {
        Left,
        Right,
    }

    public DockingExitSide DockingCinematicExitSide;
    
    public Vector3 DockingEndPoint;
    public AnimatorOverrideController OverrideController;

    public AnimationClip DockingAnimation;
    public AnimationClip DockingLoopAnimation;
    public AnimationClip LaunchLeftAnimation;
    public AnimationClip LaunchRightAnimation;
    public AnimationClip PlayerDockingAnimation;

    private ColorCustomizer _colorCustomizer;
    private RedockLock _redockLock;

    public override void Instantiate()
    {
        var dockable = parentVehicle.Model.AddComponent<Dockable>();
        dockable.rb = parentVehicle.PhysicsHandler.Rigidbody;
        dockable.vehicle = parentVehicle.Behaviour;
        parentVehicle.Behaviour.dockable = dockable;
        
        _colorCustomizer = parentVehicle.Model.AddComponent<ColorCustomizer>();
        _colorCustomizer.isBase = false;
        
        _redockLock = parentVehicle.Model.AddComponent<RedockLock>();
    }

    public DockingHandler WithPositions(Vector3 dockingEndPoint, DockingExitSide dockingCinematicExitSide, float dockingUnlockDistance = 5)
    {
        DockingEndPoint = dockingEndPoint;
        DockingCinematicExitSide = dockingCinematicExitSide;
        _redockLock.unlockDistance = dockingUnlockDistance;

        return this;
    }

    public DockingHandler WithAnimations(
        AnimationClip dockingAnimation, AnimationClip dockingLoopAnimation, AnimationClip launchLeftAnimation, AnimationClip launchRightAnimation, AnimationClip playerDockingAnimation
        )
    {
        DockingAnimation = dockingAnimation;
        DockingLoopAnimation = dockingLoopAnimation;
        LaunchLeftAnimation = launchLeftAnimation;
        LaunchRightAnimation = launchRightAnimation;
        PlayerDockingAnimation = playerDockingAnimation;

        OverrideController = new AnimatorOverrideController();
        return this;
    }

    public class RedockLock : MonoBehaviour
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

    public DockingHandler(BaseVehiclePrefab parentVehicle) : base(parentVehicle) { }
}