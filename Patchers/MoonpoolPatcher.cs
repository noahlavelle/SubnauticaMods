using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UIElements;
using VehicleFramework.VehicleComponents;

namespace VehicleFramework.Patchers;

[HarmonyPatch(typeof(VehicleDockingBay))]
public class MoonpoolPatcher
{
    /**
     * When checking if a vehicle is eligible to dock, the tech type is compared to a SeaTruck and prawn suit
     * This extends this check to include all dockable custom vehicles
     */
    [HarmonyPostfix, HarmonyPatch("IDockingBay.AllowedToDock")]
    static void AllowedToDockPostfix(ref bool __result, Dockable dockable, Dockable ____dockedObject)
    {
        if (____dockedObject != null) return;
        if (__result == false && Plugin.Vehicles.TryGetValue(CraftData.GetTechType(dockable.gameObject), out _))
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
        Plugin.Vehicles.TryGetValue(CraftData.GetTechType(dockable.gameObject), out var vehicle);
        if (vehicle == null) return;
        var customDockable = vehicle.VehicleComponents.OfType<VehicleDockable>().ToList();
        if (!customDockable.Any()) return;
        var customDockableFirst = customDockable.First();

        // Moonpool animation overriding
        if (___animator.runtimeAnimatorController.GetType() != typeof(AnimatorOverrideController)) {
            customDockableFirst.OverrideController.runtimeAnimatorController = ___animator.runtimeAnimatorController;
            ___animator.runtimeAnimatorController = customDockableFirst.OverrideController;
            
            var dockingAnimation = ___animator.runtimeAnimatorController.animationClips.First(a => a.name == "seatruck_dock");
            var dockingLoopAnimation = ___animator.runtimeAnimatorController.animationClips.First(a => a.name == "seatruck_docked");
            var launchLeftAnimation = ___animator.runtimeAnimatorController.animationClips.First(a => a.name == "enter_left");
            var launchRightAnimation = ___animator.runtimeAnimatorController.animationClips.First(a => a.name == "enter_right");

            var clipOverrides = new List<KeyValuePair<AnimationClip, AnimationClip>>
            {
                new(dockingAnimation, customDockableFirst.DockingAnimation),
                new(dockingLoopAnimation, customDockableFirst.DockingLoopAnimation),
                new(launchLeftAnimation, customDockableFirst.LaunchLeftAnimation),
                new(launchRightAnimation, customDockableFirst.LaunchRightAnimation),
            };
        
            customDockableFirst.OverrideController.ApplyOverrides(clipOverrides);
        }
        
        // Player animation overriding
        // if (Player.main.playerAnimator.runtimeAnimatorController.GetType() != typeof(AnimatorOverrideController))
        // {
        //     var animOverride = new AnimatorOverrideController();
        //     animOverride.runtimeAnimatorController = Player.main.playerAnimator.runtimeAnimatorController;
        //     Player.main.playerAnimator.runtimeAnimatorController = animOverride;
        // }
        //
        // ((AnimatorOverrideController) Player.main.playerAnimator.runtimeAnimatorController)
        //     ["player_seatruck_moonpool_dock"] = customDockableFirst.PlayerDockingAnimation;
    }
    
    [HarmonyPostfix, HarmonyPatch("UpdateDocking")]
    static void TriggerDockingAnimationPostfix(Dockable ____dockedObject, Animator ___animator, bool ___docked_param)
    {
        SafeAnimator.SetBool(___animator, "seamoth_docked", ___docked_param);
    }

    /**
     * During docking animation, the vehicle moves towards an end point, which may not fit for all custom vehicles
     * Any custom end points defined in the VehicleDockable component will be applied here
     */
    [HarmonyPrefix, HarmonyPatch("UpdateDockedPosition")]
    static bool FixDockingTargetPositionPostfix(Dockable dockable, float interpfraction, Vector3 ___startPosition,
        Quaternion ___startRotation, Transform ___dockingEndPos)
    {
        if (!Plugin.Vehicles.TryGetValue(CraftData.GetTechType(dockable.gameObject), out var vehicle)) return true;
        var customDockable = vehicle.VehicleComponents.OfType<VehicleDockable>().First();
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
        
        var redockLock = entityRoot.GetComponent<VehicleDockable.VehicleRedockLock>();
        if (redockLock == null) return true;
        return !redockLock.IsLocked();
    }

    [HarmonyPostfix, HarmonyPatch("LockSeatruckRedocking")]
    static void LockVehicleRedocking(Dockable ____dockedObject, VehicleDockingBay __instance)
    {
        var redockLock = ____dockedObject.gameObject.GetComponent<VehicleDockable.VehicleRedockLock>();
        redockLock.Lock(__instance);
    }
}