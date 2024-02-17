using System.Runtime.CompilerServices;
using UnityEngine.EventSystems;

namespace VehicleFrameworkNautilus.Patches;


[HarmonyPatch]
public class ConstructorInputPatcher
{
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(Crafter), nameof(Crafter.Craft))]
    [MethodImpl(MethodImplOptions.NoInlining)]
    static void BaseCraft(Crafter instance, TechType techType, float duration) { }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ConstructorInput), nameof(ConstructorInput.Craft))]
    static bool CraftPostfix(ConstructorInput __instance, TechType techType)
    {
        if (!Plugin.RegisteredVehicles.TryGetValue(techType, out var vehicle)) return true;
        
        var position = Vector3.zero;
        var rotation = Quaternion.identity;
        
        __instance.GetCraftTransform(techType, ref position, ref rotation);
        
        if (!__instance.CheckSpace(techType, position))
        {
            __instance.invalidNotification.Play();
        }
        else if (CrafterLogic.ConsumeResources(techType))
        {
            BaseCraft(__instance, techType, vehicle.CraftTime);
        }

        return false;
    }
}