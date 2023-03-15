using BepInEx;

namespace VehicleFramework
{
    [BepInPlugin("com.noahlavelle.vehicleframework", "VehicleFramework", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }
    }
}
