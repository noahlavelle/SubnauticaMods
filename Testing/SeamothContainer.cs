using VehicleFrameworkNautilus.Extensions;
using VehicleFrameworkNautilus.Items.Vehicle;
using VehicleFrameworkNautilus.Items.Vehicle.Components.Configurable;

namespace VehicleFrameworkNautilus.Testing;

public class SeamothContainer : VehicleContainerHandler
{
    public override string ClassID => "seamothmod";
    public override string DisplayName => "Alterra Seamoth";
    public override string Description => "Desc";
    public override float CraftTime => 13f;
    public override Sprite CraftIcon => null;

    public override RecipeData Recipe => new()
    {
        craftAmount = 1,
        Ingredients =
        {
            new Ingredient(TechType.TitaniumIngot, 1)
        }
    };

    public override GameObject Prefab => Plugin.AssetBundle.LoadAsset<GameObject>("SeamothPrefab.prefab");

    public override void Register()
    { 
        SetBehaviour<SeamothBehaviour>();
        
        base.Register();
        
        Prefab.ApplyAlterraVehicleMaterial();

        PhysicsHandler
            .WithPhysicsConfig(
                new PhysicsHandlerConfig(800, 2, 4, 0, 9.81f, -5f, 4, 2, true, false
                ))
            .WithCollision(Prefab.transform.Find("Collision").gameObject);

        PingHandler
            .WithOrigin(Prefab.transform.Find("PingOrigin"))
            .WithIcon(Plugin.AssetBundle.LoadAsset<Sprite>("SeamothPingIcon"), ClassID)
            .WithName("Seamoth", ClassID);

        UpgradeModulesInputHandler
            .WithUpgradeConsole(
                Prefab.transform.Find("UpgradeConsole"), Prefab.transform.Find("UpgradeModulesRoot"), Prefab.transform.Find("Model/Vehicle_Anim/UpgradeSlot_Hatch_geo"))
            .WithSound(AssetHelper.StorageOpenSound, AssetHelper.StorageCloseSound)
            .WithSlots(
                Prefab.transform.Find("Model/Vehicle_Anim/UpgradeSlot_1_geo"),
                Prefab.transform.Find("Model/Vehicle_Anim/UpgradeSlot_2_geo"),
                Prefab.transform.Find("Model/Vehicle_Anim/UpgradeSlot_3_geo"),
                Prefab.transform.Find("Model/Vehicle_Anim/UpgradeSlot_4_geo")
            );

        EnergyHandler
            .WithBatterySlot(Prefab.transform.Find("BatterySlot"))
            .WithEnergyConfig(TechType.PowerCell);

        PositionHandler
            .WithPositions(
                Prefab.transform.Find("Model/Vehicle_Anim/Joints/AttachJoint/CameraPivot"),
                Prefab.transform.Find("EndPosition"),
                Prefab.transform.Find(
                    "Model/Vehicle_Anim/Joints/Wheel_Base/Wheel_Segment_1/Wheel_Segment_2/Wheel_Head/LeftIKTarget"),
                Prefab.transform.Find(
                    "Model/Vehicle_Anim/Joints/Wheel_Base/Wheel_Segment_1/Wheel_Segment_2/Wheel_Head/RightIKTarget"),
                Prefab.transform.Find(
                    "AlternateExits")
            );

        SoundHandler
            .WithSounds(
                AssetHelper.SeamothWelcomeSound,
                AssetHelper.DamageSound,
                AssetHelper.SplashSound, 
                AssetHelper.SeamothRevUpSound,
                AssetHelper.SeamothRevLoopSound,
                "Seamoth: Welcome aboard, captain"
            );

        HealthHandler
            .WithConfig(
                200, -1, 70, 0.2f, true, true, false
            );

        CrushDepthHandler
            .WithConfig(200, 20, 3)
            .WithSound(Prefab.transform.Find("crushDamageSound").gameObject, AssetHelper.CrushDamageSound, AssetHelper.SeamothDepthWarning);

        AddHandler<DockingHandler>()
            .WithPositions(new Vector3(0, 0, 0), DockingHandler.DockingExitSide.Left)
            .WithAnimations(Plugin.AssetBundle.LoadAsset<AnimationClip>("seamoth_dock"),
                Plugin.AssetBundle.LoadAsset<AnimationClip>("loop_seamoth_docked"),
                Plugin.AssetBundle.LoadAsset<AnimationClip>("seamoth_launch_left"),
                Plugin.AssetBundle.LoadAsset<AnimationClip>("seamoth_launch_right"),
                Plugin.AssetBundle.LoadAsset<AnimationClip>("player_view_moon_seamoth_dock"),
                3f);

        AddHandler<LightHandler>()
            .WithLightingParent(Prefab.transform.Find("Lighting"))
            .WithEnergyHandler(EnergyHandler)
            .WithSound(AssetHelper.LightOnSound, AssetHelper.LightOffSound);
    }

    public override CraftTree.Type ModulesFabricatorType => CraftTree.Type.Fabricator;
    public override string ModulesNodeTabID => "SeamothModules";
    protected override string ModulesNodeName => "Seamoth";
    protected override string ModulesEquipmentTypeName => "SeamothUpgradeModule";
}