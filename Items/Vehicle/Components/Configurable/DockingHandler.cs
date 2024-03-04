using System.Linq;
using UnityEngine.Serialization;

namespace VehicleFrameworkNautilus.Items.Vehicle.Components.Configurable;

[RequireComponent(typeof(PhysicsHandler))]
public class DockingHandler : HandlerComponent
{
    public enum DockingExitSide
    {
        Left,
        Right,
    }

    public DockingExitSide dockingCinematicExitSide;
    
    public Vector3 dockingEndPoint;
    public AnimatorOverrideController overrideController;

    public AnimationClip dockingAnimation;
    public AnimationClip dockingLoopAnimation;
    public AnimationClip launchLeftAnimation;
    public AnimationClip launchRightAnimation;
    public AnimationClip playerDockingAnimation;

    public float dockingExitAnimationDelay;

    [SerializeField] private ColorCustomizer _colorCustomizer;
    [SerializeField] private RedockLock _redockLock;
    [SerializeField] private float _unlockDistance;

    public void Awake()
    {
        var dockable = gameObject.AddComponent<Dockable>();
        dockable.rb = gameObject.GetComponent<PhysicsHandler>().rigidbody;
        dockable.vehicle = VehicleBehaviourHandler;
        VehicleBehaviourHandler.dockable = dockable;

        _colorCustomizer = gameObject.AddComponent<ColorCustomizer>();
        _colorCustomizer.isBase = false;
        
        _redockLock = gameObject.AddComponent<RedockLock>();
        _redockLock.unlockDistance = _unlockDistance;
    }

    public DockingHandler WithPositions(Vector3 dockingEndPoint, DockingExitSide dockingCinematicExitSide, float dockingUnlockDistance = 5)
    {
        this.dockingEndPoint = dockingEndPoint;
        this.dockingCinematicExitSide = dockingCinematicExitSide;
        _unlockDistance = dockingUnlockDistance;

        return this;
    }

    public DockingHandler WithAnimations(
        AnimationClip dockingAnimation, AnimationClip dockingLoopAnimation, AnimationClip launchLeftAnimation, AnimationClip launchRightAnimation, AnimationClip playerDockingAnimation, float dockingExitAnimationDelay)
    {
        this.dockingAnimation = dockingAnimation;
        this.dockingLoopAnimation = dockingLoopAnimation;
        this.launchLeftAnimation = launchLeftAnimation;
        this.launchRightAnimation = launchRightAnimation;
        this.playerDockingAnimation = playerDockingAnimation;
        this.dockingExitAnimationDelay = dockingExitAnimationDelay;

        overrideController = new AnimatorOverrideController();
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
}