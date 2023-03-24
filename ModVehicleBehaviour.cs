using mset;
using UnityEngine;

namespace VehicleFramework;

public abstract class ModVehicleBehaviour : Vehicle, IScheduledUpdateBehaviour, IInteriorSpace, IHandTarget
{
    private bool _playerFullyEntered;
    private Color _lightColor;
    
    private static readonly int DockedAnimation = Animator.StringToHash("docked");

    public EngineRpmSFXManager EngineRpmSfxManager;
    public ToggleLights LightsToggle;

    public bool PlayerFullyEntered
    {
        get => _playerFullyEntered;
        set
        {
            _playerFullyEntered = value;
            if (_playerFullyEntered)
            {
                Player.main.armsController.SetWorldIKTarget(leftHandPlug, rightHandPlug);
            }
            else
            {
                Player.main.armsController.SetWorldIKTarget(null, null);
            }
        }
    }
    
    
    public override string[] slotIDs => new []{ "SeamothModule1", "SeamothModule2", "SeamothModule3", "SeamothModule4" };
    
    public override Vector3[] vehicleDefaultColors => new Vector3[5]
    {
        new(0f, 0f, 1f),
        new(0f, 0f, 0f),
        new(0f, 0f, 1f),
        new(0.577f, 0.447f, 0.604f),
        new(0.114f, 0.729f, 0.965f)
    };


    public virtual string GetProfileTag()
    {
        return "Vehicle";
    }

    public override void Start()
    {
        base.Start();

        mainAnimator = transform.GetComponentInChildren<Animator>();
        
        // Apply vehicle config
        controlSheme = ControlScheme;
        stabilizeRoll = true;
        forwardForce = ForwardForce;
        backwardForce = BackwardForce;
        sidewardForce = SidewardForce;
        verticalForce = VerticalForce;

        var light = transform.GetComponentInChildren<Light>();
        if (light != null)
        {
            _lightColor = light.color;
        }
    }
    
    public override void Update()
    {
        base.Update();

        UpdateSounds();
        
        if (GetPilotingMode())
        {
            var buttonFormat = LanguageCache.GetButtonFormat("PressToExit", GameInput.Button.Exit);
            HandReticle.main.SetTextRaw(HandReticle.TextType.Use, buttonFormat);
            HandReticle.main.SetTextRaw(HandReticle.TextType.UseSubscript, string.Empty);
            
            var moveVector = AvatarInputHandler.main.IsEnabled() ? GameInput.GetMoveDirection() : Vector3.zero;
            if (moveVector.magnitude > 0.1f)
            {
                ConsumeEngineEnergy(Time.deltaTime * EnergyConsumptionRate * moveVector.magnitude);
            }

            LightsToggle.CheckLightToggle();
        }
        
        mainAnimator.SetBool(DockedAnimation, docked);
    }

    private void UpdateSounds()
    {
        var vector = AvatarInputHandler.main.IsEnabled() ? GameInput.GetMoveDirection() : Vector3.zero;
        if (CanPilot() && vector.magnitude > 0f && GetPilotingMode())
        {
            EngineRpmSfxManager.AccelerateInput();
        }
    }

    public override bool CanPilot()
    {
        return !FPSInputModule.current.lockMovement && IsPowered();
    }

    public override void SetPlayerInside(bool inside)
    {
        base.SetPlayerInside(inside);
        UWE.Utils.SetIsKinematicAndUpdateInterpolation(useRigidbody, isKinematic: false);
        PlayerFullyEntered = inside;
    }

    public override void OnPilotModeBegin()
    {
        base.OnPilotModeBegin();
        UWE.Utils.EnterPhysicsSyncSection();
        UWE.Utils.SetIsKinematicAndUpdateInterpolation(useRigidbody, isKinematic: false);
        
        Player.main.EnterInterior(this);
        SetPlayerInside(true);
        PlatformUtils.SetLightBarColor(_lightColor);
    }
    
    public override void OnPilotModeEnd()
    {
        base.OnPilotModeEnd();
        UWE.Utils.ExitPhysicsSyncSection();
        Player.main.inSeamoth = false;
        PlayerFullyEntered = false;
        Player.main.ExitCurrentInterior();
        PlatformUtils.ResetLightBarColor();
    }
    
    // Override nonfunctional default behaviour
    public new void OnHandHover(GUIHand hand)
    {
        if (GetPilotingMode()) return;
        HandReticle.main.SetIcon(HandReticle.IconType.Hand);
        HandReticle.main.SetText(HandReticle.TextType.Hand, EnterVehicleText, translate: true, GameInput.Button.LeftHand);
        HandReticle.main.SetText(HandReticle.TextType.HandSubscript, string.Empty, translate: false);
    }

    public new void OnHandClick(GUIHand hand)
    {
        if (GetPilotingMode()) return;
        EnterVehicle(hand.player, teleport: true);
    }


    public void ScheduledUpdate()
    {
        throw new System.NotImplementedException();
    }

    public int scheduledUpdateIndex { get; set; }
    public void SetPlayerInsideState(bool state) { }

    public bool IsPlayerInside()
    {
        return PlayerFullyEntered;
    }

    public void OnPlayerKill() { }

    public float GetInsideTemperature()
    {
        return 20f;
    }

    public bool CanBreathe()
    {
        return energyInterface.hasCharge;
    }

    public bool CanDropItemsInside()
    {
        return false;
    }

    public Sky GetInteriorSky()
    {
        return null;
    }

    public Sky GetGlassSky()
    {
        return null;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public RespawnPoint GetRespawnPoint()
    {
        return gameObject.GetComponentInChildren<RespawnPoint>();
    }

    public VFXSurfaceTypes GetDefaultSurface()
    {
        return VFXSurfaceTypes.metal;
    }

    public bool IsStoryBase()
    {
        return true;
    }

    public bool IsInside(GameObject obj)
    {
        return true;
    }

    public bool IsValidForRespawn()
    {
        return energyInterface.hasCharge;
    }
    
    // Custom vehicle parameters
    protected abstract string EnterVehicleText { get; }
    protected abstract ControlSheme ControlScheme { get; }
    protected abstract float EnergyConsumptionRate { get;  }
    protected abstract float ForwardForce { get; }
    protected abstract float BackwardForce { get; }
    protected abstract float SidewardForce { get; }
    protected abstract float VerticalForce { get; }
}