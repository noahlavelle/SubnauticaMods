namespace VehicleFrameworkNautilus.Items.Vehicle.Components.Configurable;

public class UpgradeModulesHandler : HandlerComponent
{
    [SerializeField] private Transform _console;
    [SerializeField] private Transform _consoleRoot;
    [SerializeField] private Transform _flap;
    [SerializeField] private FMODAsset _openSound;
    [SerializeField] private FMODAsset _closeSound;

    public void Awake()
    {
        VehicleBehaviour.upgradesInput.animator = VehicleBehaviour.GetComponentInChildren<Animator>();
        VehicleBehaviour.upgradesInput.collider = VehicleBehaviour.upgradesInput.GetComponent<Collider>(); 

        VehicleBehaviour.upgradesInput.dockType = global::Vehicle.DockType.Base;
        VehicleBehaviour.upgradesInput.interactText = "UpgradeConsole";
        VehicleBehaviour.upgradesInput.flap = _flap.transform;
        VehicleBehaviour.upgradesInput.animatorParamOpen = "";
        VehicleBehaviour.upgradesInput.anglesOpened = new Vector3(-180, -90, 90);
        VehicleBehaviour.upgradesInput.anglesClosed = new Vector3(-90, -90, 90);
        VehicleBehaviour.upgradesInput.openSound = _openSound;
        VehicleBehaviour.upgradesInput.closeSound = _closeSound;
        

    }

    public UpgradeModulesHandler WithUpgradeConsole(Transform console, Transform consoleRoot, Transform flap)
    {
        _console = console;
        _consoleRoot = consoleRoot;
        _flap = flap;
        
        var upgradeConsoleInput = _console.gameObject.AddComponent<VehicleUpgradeConsoleInput>();
        var upgradeConsoleRootIdentifier = _consoleRoot.gameObject.AddComponent<ChildObjectIdentifier>();
        
        upgradeConsoleInput.slots = new[]
        {
            new VehicleUpgradeConsoleInput.Slot(),
            new VehicleUpgradeConsoleInput.Slot(),
            new VehicleUpgradeConsoleInput.Slot(),
            new VehicleUpgradeConsoleInput.Slot()
        };
        
        VehicleBehaviour.modulesRoot = upgradeConsoleRootIdentifier;
        VehicleBehaviour.upgradesInput = upgradeConsoleInput;
        
        return this;
    }

    public UpgradeModulesHandler WithSound(FMODAsset openSound, FMODAsset closeSound)
    {
        _openSound = openSound;
        _closeSound = closeSound;

        return this;
    }
}