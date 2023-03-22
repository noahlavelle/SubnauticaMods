using System.Linq;
using UnityEngine;

namespace VehicleFramework.VehicleComponents;

public class VehicleConstructionVFX : VehicleComponent
{
    private readonly string _buildBotPathsPath;
    private readonly string _buildBotBeamPointsPath;
    
    public VehicleConstructionVFX(string buildBotBeamPointsPath = "BuildBotBeamPoints", string buildBotPathsPath = "BuildBotPaths")
    {
        _buildBotBeamPointsPath = buildBotBeamPointsPath;
        _buildBotPathsPath = buildBotPathsPath;
    }
    
    public override void AddComponent(ModVehicle parentVehicle)
    {
        var vfx = parentVehicle.Prefab.AddComponent<VFXConstructing>();
        var referenceVfx = parentVehicle.ReferencePrefab.GetComponent<VFXConstructing>();
        
        vfx.ghostMaterial = referenceVfx.ghostMaterial;
        vfx.constructSound = referenceVfx.constructSound;
        vfx.surfaceSplashSound = referenceVfx.surfaceSplashSound;
        vfx.surfaceSplashFX = referenceVfx.surfaceSplashFX;
        vfx.surfaceSplashVelocity = referenceVfx.surfaceSplashVelocity;
        vfx.alphaTexture = referenceVfx.alphaTexture;
        vfx.alphaDetailTexture = referenceVfx.alphaDetailTexture;

        var buildBots = parentVehicle.Prefab.AddComponent<BuildBotBeamPoints>();
        var beamPointsParent = parentVehicle.Prefab.transform.Find(_buildBotBeamPointsPath);
        var pathsParent = parentVehicle.Prefab.transform.Find(_buildBotPathsPath);
        
        buildBots.beamPoints = Enumerable.Range(0, beamPointsParent.childCount).Select(beamPointsParent.GetChild).ToArray();
        Enumerable.Range(0, pathsParent.childCount)
            .ForEach(i => CreateBuildBotPath( parentVehicle.Prefab, pathsParent.GetChild(i)));
    }
    
    private static void CreateBuildBotPath(GameObject prefab, Transform parent)
    {
        var path = prefab.AddComponent<BuildBotPath>();
        path.points = Enumerable.Range(0, parent.childCount).Select(parent.GetChild).ToArray();
    }
}