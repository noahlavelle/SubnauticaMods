using System.Linq;
using Nautilus.Extensions;

namespace VehicleFrameworkNautilus.Items.Vehicle.Components.Base;

public class ConstructionVFXHandler : HandlerComponent
{
    public void Awake()
    {
        var referenceVFX = VehicleHelper.ReferenceVehicle.GetComponent<VFXConstructing>();

        gameObject.AddComponent<VFXConstructing>().CopyComponent(referenceVFX);

        var buildBots = gameObject.AddComponent<BuildBotBeamPoints>();
        var beamPointsParent = gameObject.transform.Find("BuildBotBeamPoints");
        var pathsParent = gameObject.transform.Find("BuildBotPaths");
        
        buildBots.beamPoints = Enumerable.Range(0, beamPointsParent.childCount).Select(beamPointsParent.GetChild).ToArray();
        Enumerable.Range(0, pathsParent.childCount)
            .ForEach(i => CreateBuildBotPath( gameObject, pathsParent.GetChild(i)));
    }
    
    private static void CreateBuildBotPath(GameObject prefab, Transform parent)
    {
        var path = prefab.AddComponent<BuildBotPath>();
        path.points = Enumerable.Range(0, parent.childCount).Select(parent.GetChild).ToArray();
    }
}