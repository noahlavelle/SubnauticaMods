namespace VehicleFrameworkNautilus.Patches;

[HarmonyPatch]
public class DockedVehicleHandTargetPatcher
{

    [HarmonyPatch(typeof(DockedVehicleHandTarget), nameof(DockedVehicleHandTarget.OnHandClick))]
    [HarmonyPostfix]
    static void OnHandClickPrefix(DockedVehicleHandTarget __instance, GUIHand hand)
    {
        if (!Plugin.RegisteredVehicles.TryGetValue(GetTechType(__instance.dockingBay.GetDockedObject().gameObject),
                out _)) return;

        __instance.cinematicController.playerViewAnimationName = "";
    }
    
}