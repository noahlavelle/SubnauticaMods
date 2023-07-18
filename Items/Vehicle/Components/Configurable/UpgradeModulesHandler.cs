namespace VehicleFrameworkNautilus.Items.Vehicle.Components.Configurable;

public class UpgradeModulesHandler : HandlerComponent
{
    private VehicleUpgradeConsoleInput _upgradeConsoleInput;
    private ChildObjectIdentifier _upgradeConsoleRootIdentifier;
    
    public override void Instantiate() { }

    public UpgradeModulesHandler WithUpgradeConsole(Transform console, Transform consoleRoot, Transform flap)
    {
        _upgradeConsoleInput = console.gameObject.AddComponent<VehicleUpgradeConsoleInput>();
        _upgradeConsoleRootIdentifier = consoleRoot.gameObject.AddComponent<ChildObjectIdentifier>();

        _upgradeConsoleInput.animator = parentVehicle.Model.GetComponentInChildren<Animator>();
        _upgradeConsoleInput.collider = _upgradeConsoleInput.GetComponent<Collider>();
        _upgradeConsoleInput.dockType = global::Vehicle.DockType.Base;
        _upgradeConsoleInput.interactText = "UpgradeConsole";
        _upgradeConsoleInput.flap = flap.transform;
        _upgradeConsoleInput.animatorParamOpen = "";
        _upgradeConsoleInput.anglesOpened = new Vector3(-180, -90, 90);
        _upgradeConsoleInput.anglesClosed = new Vector3(-90, -90, 90);
        
        parentVehicle.Behaviour.modulesRoot = _upgradeConsoleRootIdentifier;
        parentVehicle.Behaviour.upgradesInput = _upgradeConsoleInput;
        return this;
    }

    public UpgradeModulesHandler WithSound(FMODAsset openSound, FMODAsset closeSound)
    {
        _upgradeConsoleInput.openSound = openSound;
        _upgradeConsoleInput.closeSound = closeSound;

        return this;
    }
    
    public UpgradeModulesHandler(BaseVehiclePrefab parentVehicle) : base(parentVehicle) { }
}