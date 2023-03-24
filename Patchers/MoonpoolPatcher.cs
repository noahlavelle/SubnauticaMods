using System.Linq;
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
    static void AllowedToDockPostfix(ref bool __result, Dockable dockable)
    {
        if (__result == false && Plugin.Vehicles.TryGetValue(CraftData.GetTechType(dockable.gameObject), out _))
        {
            __result = true;
        }
    }

    /**
     * During docking procedure, an animation is played for the prawn suit and SeatTuck only
     * This extends to run animations for all dockable custom vehicles
     */
    [HarmonyPostfix, HarmonyPatch("UpdateDocking")]
    static void UpdateDockingPostfix(Animator ___animator, bool ___docked_param, Dockable ____dockedObject)
    {
        if (____dockedObject == null) return;
        Plugin.Vehicles.TryGetValue(CraftData.GetTechType(____dockedObject.gameObject), out var vehicle);
        if (vehicle == null) return;
        var customDockable = vehicle.VehicleComponents.OfType<VehicleDockable>().ToList();
        if (!customDockable.Any()) return;
        SafeAnimator.SetBool(___animator, customDockable.First().AnimationName, ___docked_param);
    }

    /**
     * During docking animation, the vehicle moves towards an end point, which may not fit for all custom vehicles
     * Any custom end points defined in the VehicleDockable component will be applied here
     */
    [HarmonyPostfix, HarmonyPatch("UpdateDockedPosition")]
    static void UpdateDockedPositionPostfix(Dockable dockable, float interpfraction, Vector3 ___startPosition,
        Quaternion ___startRotation, Transform ___dockingEndPos)
    {
        if (!Plugin.Vehicles.TryGetValue(CraftData.GetTechType(dockable.gameObject), out var vehicle)) return;
        var customDockable = vehicle.VehicleComponents.OfType<VehicleDockable>().First();
        ___dockingEndPos.localPosition = customDockable.DockingEndPoint;
        dockable.transform.position = Vector3.Lerp(___startPosition, ___dockingEndPos.position, interpfraction);
        dockable.transform.rotation = Quaternion.Lerp(___startRotation, ___dockingEndPos.rotation, interpfraction);
    }

    /**
     * Vehicles repeatedly undock then dock. This mirrors the SeaTruckRedockLock class
    */
    [HarmonyPrefix, HarmonyPatch("OnTriggerEnter")]
    static bool OnTriggerEnterPrefix(Collider other)
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