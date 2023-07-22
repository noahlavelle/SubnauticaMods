using System.Runtime.CompilerServices;
using VehicleFrameworkNautilus.Items.Vehicle;

namespace VehicleFrameworkNautilus.Patches;


[HarmonyPatch(typeof(Crafter), nameof(Crafter.Craft))]
public class ConstructorInputPatcher
{
    [HarmonyReversePatch]
    [MethodImpl(MethodImplOptions.NoInlining)]
    static void BaseCraftDummy(Crafter instance, TechType techType, float duration) { }

    [HarmonyPatch(typeof(ConstructorInput), nameof(ConstructorInput.Craft))]
    [HarmonyPrefix]
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
            BaseCraftDummy(__instance, techType, vehicle.CraftTime);
        }

        return false;
    }
}

[HarmonyPatch(typeof(PlayerCinematicController), nameof(PlayerCinematicController.OnPlayerCinematicModeEnd))]
public class TestPatcher
{
}