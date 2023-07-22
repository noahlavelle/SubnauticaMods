using System.Linq;
using VehicleFrameworkNautilus.Items.Vehicle.Components.Configurable;

namespace VehicleFrameworkNautilus.Patches;

[HarmonyPatch(typeof(VehicleDockingBay))]
public class MoonpoolPatcher
{
    public static Transform DockExitLeft;
    public static Transform DockExitRight;
    
    [HarmonyPostfix, HarmonyPatch(nameof(VehicleDockingBay.Start))]
    static void StartPostfix(VehicleDockingBay __instance)
    {
        DockExitLeft = __instance.exosuitDockPlayerCinematic.endTransform;
        DockExitRight = __instance.dockPlayerCinematic.endTransform;
    }
    
    /**
     * When checking if a vehicle is eligible to dock, the tech type is compared to a SeaTruck and prawn suit
     * This extends this check to include all dockable custom vehicles
     */
    [HarmonyPostfix, HarmonyPatch("IDockingBay.AllowedToDock")]
    static void AllowedToDockPostfix(ref bool __result, Dockable dockable, Dockable ____dockedObject)
    {
        if (____dockedObject != null) return;
        if (__result == false && Plugin.RegisteredVehicles.TryGetValue(GetTechType(dockable.gameObject), out _))
        {
            __result = true;
        }
    }
    
    /**
     * During docking procedure, an animation is played for the prawn suit and SeatTuck only
     * This extends to run animations for all dockable custom vehicles
     */
    [HarmonyPostfix, HarmonyPatch("Dock")]
    static void ReplaceDockingAnimationPostfix(Dockable dockable, Animator ___animator, PlayerCinematicController ___dockPlayerCinematic)
    {
        if (dockable == null) return;

        Plugin.RegisteredVehicles.TryGetValue(GetTechType(dockable.gameObject), out var vehicle);
        var isVehicleOverrideActive = ___animator.runtimeAnimatorController.GetType() == typeof(AnimatorOverrideController);
        var isPlayerOverrideActive = Player.main.playerAnimator.runtimeAnimatorController.GetType() == typeof(AnimatorOverrideController);

        if (vehicle == null)
        {
            if (isPlayerOverrideActive)
            {
                Plugin.Logger.LogInfo("Removing player animation overrides");
                Player.main.playerAnimator.runtimeAnimatorController =
                    ((AnimatorOverrideController) Player.main.playerAnimator.runtimeAnimatorController).runtimeAnimatorController;
            }
            
            if (isVehicleOverrideActive)
            {
                Plugin.Logger.LogInfo("Removing vehicle animation overrides");
                ___animator.runtimeAnimatorController =
                    ((AnimatorOverrideController) ___animator.runtimeAnimatorController).runtimeAnimatorController;

            }
        } else
        {
            var customDockable = vehicle.GetComponent<DockingHandler>();
            
            if (!isVehicleOverrideActive)
            {
                Plugin.Logger.LogInfo("Overriding vehicle docking animation");
                var overrideController = new AnimatorOverrideController
                {
                    runtimeAnimatorController = ___animator.runtimeAnimatorController
                };
            
                ___animator.runtimeAnimatorController = overrideController;
            
                var dockingAnimation = overrideController.animationClips.First(a => a.name == "seatruck_dock");
                var dockingLoopAnimation = overrideController.animationClips.First(a => a.name == "seatruck_docked");
                var launchLeftAnimation = overrideController.animationClips.First(a => a.name == "enter_left");
                var launchRightAnimation = overrideController.animationClips.First(a => a.name == "enter_right");

                var clipOverrides = new List<KeyValuePair<AnimationClip, AnimationClip>>
                {
                    new(dockingAnimation, customDockable.DockingAnimation),
                    new(dockingLoopAnimation, customDockable.DockingLoopAnimation),
                    new(launchLeftAnimation, customDockable.LaunchLeftAnimation),
                    new(launchRightAnimation, customDockable.LaunchRightAnimation),
                };
        
                overrideController.ApplyOverrides(clipOverrides);
            }

            if (!isPlayerOverrideActive)
            {
                Plugin.Logger.LogInfo("Overriding player docking animation");
                var overrideController = new AnimatorOverrideController
                {
                    runtimeAnimatorController = Player.main.playerAnimator.runtimeAnimatorController
                };
                
                Player.main.playerAnimator.runtimeAnimatorController = overrideController;

                overrideController["player_seatruck_moonpool_dock"] = customDockable.PlayerDockingAnimation;
            }
        }
    }
    
    [HarmonyPostfix, HarmonyPatch("UpdateDocking")]
    static void TriggerDockingAnimationPostfix(Dockable ____dockedObject, Animator ___animator, bool ___docked_param, VehicleDockingBay __instance)
    {
        if (____dockedObject && Plugin.RegisteredVehicles.TryGetValue(GetTechType(____dockedObject.gameObject), out var vehicle))
        {
            var dockingHandler = vehicle.GetComponent<DockingHandler>();
            if (dockingHandler == null) return;
            
            SafeAnimator.SetBool(___animator, "seamoth_docked", ___docked_param);
            if (__instance.dockPlayerCinematic.cinematicModeActive)
            {
                __instance.dockPlayerCinematic.endTransform = dockingHandler.DockingCinematicExitSide == DockingHandler.DockingExitSide.Left ? DockExitLeft : DockExitRight;
            }
        }
        else
        {
            __instance.exosuitDockPlayerCinematic.endTransform = DockExitLeft;
            __instance.dockPlayerCinematic.endTransform = DockExitRight;
        }
    }

    /**
     * During docking animation, the vehicle moves towards an end point, which may not fit for all custom vehicles
     * Any custom end points defined in the VehicleDockable component will be applied here
     */
    [HarmonyPrefix, HarmonyPatch("UpdateDockedPosition")]
    static bool FixDockingTargetPositionPostfix(Dockable dockable, float interpfraction, Vector3 ___startPosition,
        Quaternion ___startRotation, Transform ___dockingEndPos)
    {
        if (!Plugin.RegisteredVehicles.TryGetValue(GetTechType(dockable.gameObject), out var vehicle)) return true;
        var customDockable = vehicle.GetComponent<DockingHandler>();
        var originalPosition = ___dockingEndPos.localPosition;
        ___dockingEndPos.localPosition = customDockable.DockingEndPoint;
        dockable.transform.position = Vector3.Lerp(___startPosition, ___dockingEndPos.position, interpfraction);
        dockable.transform.rotation = Quaternion.Lerp(___startRotation, ___dockingEndPos.rotation, interpfraction);
        ___dockingEndPos.localPosition = originalPosition;
        return false;
    }

    /**
     * Vehicles repeatedly undock then dock. This mirrors the SeaTruckRedockLock class
    */
    [HarmonyPrefix, HarmonyPatch("OnTriggerEnter")]
    static bool CheckIfRedockLockedPrefix(Collider other)
    {
        var entityRoot = UWE.Utils.GetEntityRoot(other.gameObject);
        if (!entityRoot)
        {
            entityRoot = other.gameObject;
        }
        
        var redockLock = entityRoot.GetComponent<DockingHandler.RedockLock>();
        if (redockLock == null) return true;
        return !redockLock.IsLocked();
    }

    [HarmonyPostfix, HarmonyPatch("LockSeatruckRedocking")]
    static void LockVehicleRedocking(Dockable ____dockedObject, VehicleDockingBay __instance)
    {
        var redockLock = ____dockedObject.gameObject.GetComponent<DockingHandler.RedockLock>();
        redockLock.Lock(__instance);
    }

}