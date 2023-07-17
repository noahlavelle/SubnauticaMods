using System.Linq;
using Nautilus.Extensions;

namespace VehicleFrameworkNautilus.Items.Vehicle.Components.Base;

public class ConstructionVFXHandler : IHandlerComponent
{
    public GameObject GameObject { get; set; }

    public void Instantiate()
    {
        var referenceVFX = VehicleHelper.ReferenceVehicle.GetComponent<VFXConstructing>();

        GameObject.AddComponent<VFXConstructing>().CopyComponent(referenceVFX);

        var buildBots = GameObject.AddComponent<BuildBotBeamPoints>();
        var beamPointsParent = GameObject.transform.Find("BuildBotBeamPoints");
        var pathsParent = GameObject.transform.Find("BuildBotPaths");
        
        buildBots.beamPoints = Enumerable.Range(0, beamPointsParent.childCount).Select(beamPointsParent.GetChild).ToArray();
        Enumerable.Range(0, pathsParent.childCount)
            .ForEach(i => CreateBuildBotPath( GameObject, pathsParent.GetChild(i)));
    }
    
    private static void CreateBuildBotPath(GameObject prefab, Transform parent)
    {
        var path = prefab.AddComponent<BuildBotPath>();
        path.points = Enumerable.Range(0, parent.childCount).Select(parent.GetChild).ToArray();
    }
}