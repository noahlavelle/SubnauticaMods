using VehicleFrameworkNautilus.Items.Vehicle;

namespace VehicleFrameworkNautilus.Patches;

[HarmonyPatch(typeof(uGUI_SeamothHUD))]
public class VehicleUIPatcher
{
    [HarmonyPrefix, HarmonyPatch(nameof(uGUI_SeamothHUD.Update))]
    static bool PatchUI(uGUI_SeamothHUD __instance)
    {
        VehicleBehaviourHandler vehicle = null;
        PDA pda = null;

        if (Player.main != null)
        {
            vehicle = Player.main.GetVehicle() as VehicleBehaviourHandler;
            pda = Player.main.GetPDA();
        }

        var displayUI = vehicle && (!pda || !pda.isInUse);
        if (__instance.root.activeSelf != displayUI)
        {
            __instance.root.SetActive(displayUI);
        }

        if (displayUI)
        {
            vehicle.GetHUDValues(out var health, out var power, out var temperature);
            if (__instance.lastHealth != health)
            {
                __instance.lastHealth = health;
                __instance.textHealth.text = IntStringCache.GetStringForInt(health);
            }
            if (__instance.lastPower != health)
            {
                __instance.lastPower = power;
                __instance.textPower.text = IntStringCache.GetStringForInt(power);
            }
            
            __instance.temperatureSmoothValue = __instance.temperatureSmoothValue < -10000f ? temperature :
                Mathf.SmoothDamp(__instance.temperatureSmoothValue, temperature, ref __instance.temperatureVelocity, 1f);
            var displayTemperature = Mathf.CeilToInt(__instance.temperatureSmoothValue);
            if (__instance.lastTemperature != displayTemperature)
            {
                __instance.lastTemperature = displayTemperature;
                __instance.textTemperature.text = IntStringCache.GetStringForInt(displayTemperature);
                __instance.textTemperatureSuffix.text = Language.main.GetFormat("ThermometerFormat");
            }
        }
        
        return false;
    }
}