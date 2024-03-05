using mset;
using UnityEngine.Events;
using VehicleFrameworkNautilus.Items.Vehicle.Components;
using VehicleFrameworkNautilus.Items.Vehicle.Components.Configurable;

namespace VehicleFrameworkNautilus.Items.Vehicle;

public abstract class VehicleBehaviourHandler : global::Vehicle, IInteriorSpace, IHandTarget
{
    private static readonly int DockedAnimation = Animator.StringToHash("docked");
    
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
        
        if (GetPilotingMode())
        {
            ConsumeEnergy();
        }
        
        mainAnimator.SetBool(DockedAnimation, docked);
    }

    public virtual void ConsumeEnergy()
    {
        var moveVector = AvatarInputHandler.main.IsEnabled() ? GameInput.GetMoveDirection() : Vector3.zero;
        if (moveVector.magnitude > 0.1f)
        {
            ConsumeEngineEnergy(Time.deltaTime * EnergyConsumptionRate * moveVector.magnitude);
        }
    }

    public override void OnPilotModeBegin()
    {
        base.OnPilotModeBegin();

        Player.main.inSeatruckPilotingChair = true;
        UWE.Utils.EnterPhysicsSyncSection();
        Player.main.EnterInterior(this);
        
        Player.main.armsController.SetWorldIKTarget(leftHandPlug, rightHandPlug);
    }

    public override void OnPilotModeEnd()
    {
        base.OnPilotModeEnd();

        Player.main.inSeatruckPilotingChair = false;
        UWE.Utils.ExitPhysicsSyncSection();
        Player.main.armsController.SetWorldIKTarget(null, null);
        Player.main.ExitCurrentInterior();
        PlatformUtils.ResetLightBarColor();
        if (movePlayerComp != null)
        {
            movePlayerComp.BeginMove();
        }
    }

    public void GetHUDValues(out int health, out int power, out float temperature)
    {
        health = Mathf.CeilToInt(liveMixin.health / liveMixin.data.maxHealth * 100f);
        power = Mathf.Max(Mathf.CeilToInt(GetComponent<EnergyMixin>().energy * 100f), 0);
        temperature = GetTemperature();
    }

    public override bool CanPilot()
    {
        return !FPSInputModule.current.lockMovement && IsPowered();
    }

    public void SetPlayerInsideState(bool state) { }
    public void OnPlayerKill(){ }

    public bool IsPlayerInside()
    {
        return Player.main.currentMountedVehicle == this;
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