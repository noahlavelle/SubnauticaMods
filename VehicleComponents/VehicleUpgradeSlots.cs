using UnityEngine;

namespace VehicleFramework.VehicleComponents;

public class VehicleUpgradeConsole : VehicleComponent
{
    private readonly FMODAsset _upgradeOpenSound;
    private readonly FMODAsset _upgradeCloseSound;
    private readonly string _upgradeConsolePath;
    private readonly string _upgradeConsoleFlapPath;
    private readonly string _upgradeModulesRootPath;
    
    public VehicleUpgradeConsole(FMODAsset upgradeOpenSound, FMODAsset upgradeCloseSound, string upgradeConsolePath = "UpgradeConsole",
        string upgradeConsoleFlapPath = "Model/Vehicle_Anim/UpgradeSlot_Hatch_geo", string upgradeModulesRootPath = "UpgradeModulesRoot")
    {
        _upgradeOpenSound = upgradeOpenSound;
        _upgradeCloseSound = upgradeCloseSound;
        _upgradeConsolePath = upgradeConsolePath;
        _upgradeConsoleFlapPath = upgradeConsoleFlapPath;
        _upgradeModulesRootPath = upgradeModulesRootPath;
    }

    public VehicleUpgradeConsole(string upgradeOpenPath, string upgradeOpenId, string upgradeOpenName, string upgradeClosePath, string upgradeCloseId,
        string upgradeCloseName, string upgradeConsolePath = "UpgradeConsole", string upgradeConsoleFlapPath = "Model/Vehicle_Anim/UpgradeSlot_Hatch_geo",
        string upgradeModulesRootPath = "UpgradeModulesRoot")
    {
        _upgradeOpenSound = AssetManager.LoadFmodAsset(upgradeOpenId, upgradeOpenPath, upgradeOpenName);
        _upgradeCloseSound = AssetManager.LoadFmodAsset(upgradeCloseId, upgradeClosePath, upgradeCloseName);
        _upgradeConsolePath = upgradeConsolePath;
        _upgradeConsoleFlapPath = upgradeConsoleFlapPath;
        _upgradeModulesRootPath = upgradeModulesRootPath;
    }
    
    public override void AddComponent(ModVehicle parentVehicle)
    {
        var animator = parentVehicle.Prefab.GetComponentInChildren<Animator>();
        var upgradeConsoleInput = parentVehicle.Prefab.transform.Find(_upgradeConsolePath).gameObject
            .AddComponent<VehicleUpgradeConsoleInput>();
        var upgradeModulesRootIdentifier = parentVehicle.Prefab.transform.Find(_upgradeModulesRootPath).gameObject.AddComponent<ChildObjectIdentifier>();
        
        upgradeConsoleInput.openSound = _upgradeOpenSound;
        upgradeConsoleInput.closeSound = _upgradeCloseSound;
        upgradeConsoleInput.animator = animator;
        upgradeConsoleInput.collider = upgradeConsoleInput.GetComponent<Collider>();
        upgradeConsoleInput.dockType = Vehicle.DockType.Base;
        upgradeConsoleInput.interactText = "UpgradeConsole";
        upgradeConsoleInput.flap = parentVehicle.Prefab.transform.Find(_upgradeConsoleFlapPath);
        
        // upgradeConsoleInput.slots = new[]
        // {
        //     new VehicleUpgradeConsoleInput.Slot
        //     {
        //         id = "SeamothModule1",
        //         model = parentVehicle.Prefab.transform.Find("Model/VehicleAnim/UpgradeSlot_1_geo").gameObject
        //     },
        //     new VehicleUpgradeConsoleInput.Slot
        //     {
        //         id = "SeamothModule2",
        //         model = parentVehicle.Prefab.transform.Find("Model/VehicleAnim/UpgradeSlot_2_geo").gameObject
        //     },
        //     new VehicleUpgradeConsoleInput.Slot
        //     {
        //         id = "SeamothModule3",
        //         model = parentVehicle.Prefab.transform.Find("Model/VehicleAnim/UpgradeSlot_3_geo").gameObject
        //     },
        //     new VehicleUpgradeConsoleInput.Slot
        //     {
        //         id = "SeamothModule4",
        //         model = parentVehicle.Prefab.transform.Find("Model/VehicleAnim/UpgradeSlot_4_geo").gameObject
        //     }
        // };

        parentVehicle.VehicleBehaviour.modulesRoot = upgradeModulesRootIdentifier;
        parentVehicle.VehicleBehaviour.upgradesInput = upgradeConsoleInput;
    }
}