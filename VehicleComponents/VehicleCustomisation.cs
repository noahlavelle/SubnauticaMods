using System.Linq;
using TMPro;
using UnityEngine;

namespace VehicleFramework.VehicleComponents;

public class VehicleCustomisation : VehicleComponent
{
    private ColorData[] _colorDatas;
    
    public VehicleCustomisation(params ColorData[] colorDatas)
    {
        _colorDatas = colorDatas;
    }
    
    public override void AddComponent(ModVehicle parentVehicle)
    {
        var colorNameControl = parentVehicle.Prefab.AddComponent<ColorNameControl>();
        var colorCustomizer = parentVehicle.Prefab.AddComponent<ColorCustomizer>();
        var namePlate = parentVehicle.Prefab.AddComponent<NamePlate>();
        var textMeshProUGUI = parentVehicle.Prefab.GetComponentInChildren<TextMeshProUGUI>();

        colorNameControl.defaultName = parentVehicle.FriendlyName;
        colorNameControl.namePlate = namePlate;
        colorNameControl.pingInstance = parentVehicle.Ping;

        var loadedColorDatas = _colorDatas.Select(c =>
            new ColorCustomizer.ColorData(parentVehicle.Prefab.transform.Find(c.RendererPath).GetComponent<Renderer>(), c.MaterialIndex)).ToArray();
        colorCustomizer.colorDatas = loadedColorDatas;
        colorCustomizer.isBase = false;

        parentVehicle.VehicleBehaviour.colorNameControl = colorNameControl;

        namePlate.text = textMeshProUGUI;
        namePlate.isBase = false;
    }

    public class ColorData
    {
        public string RendererPath;
        public int MaterialIndex;

        public ColorData(string rendererPath, int materialIndex)
        {
            RendererPath = rendererPath;
            MaterialIndex = materialIndex;
        }
    }
}