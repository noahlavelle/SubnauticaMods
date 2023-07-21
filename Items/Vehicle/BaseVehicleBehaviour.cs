using mset;

namespace VehicleFrameworkNautilus.Items.Vehicle;

public abstract class BaseVehicleBehaviour : global::Vehicle, IInteriorSpace, IHandTarget
{
    private bool _playerFullyEntered;
    private static readonly int DockedAnimation = Animator.StringToHash("docked");
    
    public EngineRpmSFXManager engineRpmSfxManager;
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

    public override void Start()
    {
        base.Start();

        mainAnimator = transform.GetComponentInChildren<Animator>();

        stabilizeRoll = true;
        controlSheme = ControlScheme;
        forwardForce = ForwardForce;
        backwardForce = BackwardForce;
        sidewardForce = SidewardForce;
        verticalForce = VerticalForce;
        handLabel = EnterVehicleText;
    }

    public override void Update()
    {
        base.Update();
        
        UpdateSounds();
        
        if (GetPilotingMode())
        {
            ApplyMovement();
        }

        mainAnimator.SetBool(DockedAnimation, docked);
    }

    public virtual void ApplyMovement()
    {
        var moveVector = AvatarInputHandler.main.IsEnabled() ? GameInput.GetMoveDirection() : Vector3.zero;
        if (moveVector.magnitude > 0.1f)
        {
            ConsumeEngineEnergy(Time.deltaTime * EnergyConsumptionRate * moveVector.magnitude);
        }
    }
    
    private void UpdateSounds()
    {
        var vector = AvatarInputHandler.main.IsEnabled() ? GameInput.GetMoveDirection() : Vector3.zero;
        if (CanPilot() && vector.magnitude > 0f && GetPilotingMode())
        {
            engineRpmSfxManager.AccelerateInput();
        }
    }

    public override void OnPilotModeBegin()
    {
        base.OnPilotModeBegin();
        UWE.Utils.EnterPhysicsSyncSection();
        Player.main.EnterInterior(this);
        SetPlayerInside(true);
    }

    public override void OnPilotModeEnd()
    {
        base.OnPilotModeEnd();
        UWE.Utils.ExitPhysicsSyncSection();
        Player.main.armsController.SetWorldIKTarget(null, null);
        mainAnimator.SetBool("player_in", false);
        Player.main.ExitCurrentInterior();
        PlatformUtils.ResetLightBarColor();
        if (movePlayerComp != null)
        {
            movePlayerComp.BeginMove();
        }
    }

    public override bool CanPilot()
    {
        return !FPSInputModule.current.lockMovement && IsPowered();
    }

    public override void SetPlayerInside(bool inside)
    {
        base.SetPlayerInside(inside);
        PlayerFullyEntered = inside;
    }
    
    public void SetPlayerInsideState(bool state) { }
    public void OnPlayerKill(){ }

    public bool IsPlayerInside()
    {
        return PlayerFullyEntered;
    }

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
    
    protected abstract string EnterVehicleText { get; }
    protected abstract ControlSheme ControlScheme { get; }
    protected abstract float EnergyConsumptionRate { get;  }
    protected abstract float ForwardForce { get; }
    protected abstract float BackwardForce { get; }
    protected abstract float SidewardForce { get; }
    protected abstract float VerticalForce { get; }
}