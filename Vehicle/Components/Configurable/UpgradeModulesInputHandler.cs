using UnityEngine.Serialization;

namespace VehicleFrameworkNautilus.Items.Vehicle.Components.Configurable;

public class UpgradeModulesInputHandler : HandlerComponent
{
    [SerializeField] private FMODAsset openSound;
    [SerializeField] private FMODAsset closeSound;
    [SerializeField] private VehicleUpgradeConsoleInput.Slot[] slots;

    public void Awake()
    {
        VehicleBehaviourHandler.upgradesInput.animator = VehicleBehaviourHandler.GetComponentInChildren<Animator>();
        
        VehicleBehaviourHandler.upgradesInput.openSound = openSound;
        VehicleBehaviourHandler.upgradesInput.closeSound = closeSound;
    }

    public UpgradeModulesInputHandler WithUpgradeConsole(Transform console, Transform consoleRoot, Transform flap)
    {
        var upgradeConsoleInput = console.gameObject.AddComponent<VehicleUpgradeConsoleInput>();
        var upgradeConsoleRootIdentifier = consoleRoot.gameObject.AddComponent<ChildObjectIdentifier>();

        upgradeConsoleInput.flap = flap;
        upgradeConsoleInput.collider = GetComponent<Collider>();
        upgradeConsoleInput.dockType = global::Vehicle.DockType.Base;
        upgradeConsoleInput.interactText = "UpgradeConsole";
        upgradeConsoleInput.animatorParamOpen = "";
        upgradeConsoleInput.anglesOpened = new Vector3(-180, -90, 90);
        upgradeConsoleInput.anglesClosed = new Vector3(-90, -90, 90);
        upgradeConsoleInput.timeOpen = 0.5f;
        upgradeConsoleInput.timeClose = 0.25f;
        
        VehicleBehaviourHandler.modulesRoot = upgradeConsoleRootIdentifier;
        VehicleBehaviourHandler.upgradesInput = upgradeConsoleInput;
        
        return this;
    }

    public UpgradeModulesInputHandler WithSlots(Transform slot1, Transform slot2, Transform slot3, Transform slot4)
    {
        slots = new[]
        {
            new VehicleUpgradeConsoleInput.Slot{id = VehicleBehaviourHandler.slotIDs[0], model = slot1.gameObject},
            new VehicleUpgradeConsoleInput.Slot{id = VehicleBehaviourHandler.slotIDs[1], model = slot2.gameObject},
            new VehicleUpgradeConsoleInput.Slot{id = VehicleBehaviourHandler.slotIDs[2], model = slot3.gameObject},
            new VehicleUpgradeConsoleInput.Slot{id = VehicleBehaviourHandler.slotIDs[3], model = slot4.gameObject}
        };

        return this;
    }

    public UpgradeModulesInputHandler WithSound(FMODAsset openSound, FMODAsset closeSound)
    {
        this.openSound = openSound;
        this.closeSound = closeSound;

        return this;
    }
}