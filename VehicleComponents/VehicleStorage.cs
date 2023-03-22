using System.Collections.Generic;
using UnityEngine;

namespace VehicleFramework.VehicleComponents;

public class VehicleStorage : VehicleComponent
{
    private readonly FMODAsset _openAsset;
    private readonly FMODAsset _closeAsset;
    private GameObject _storageParent;
    private readonly int _width;
    private readonly int _height;
    
    public readonly Dictionary<Transform, StorageContainer> StorageContainers;

    public VehicleStorage(int width, int height, FMODAsset openSound, FMODAsset closeSound, GameObject storageParent = null)
    {
        StorageContainers = new Dictionary<Transform, StorageContainer>();
        _openAsset = openSound;
        _closeAsset = closeSound;
        _storageParent = storageParent;
        _width = width;
        _height = height;
    }
    
    public VehicleStorage(int width, int height, string openPath, string openId, string openName, string closePath, string
        closeId, string closeName, GameObject storageParent = null)
    {
        StorageContainers = new Dictionary<Transform, StorageContainer>();
        _openAsset = AssetManager.LoadFmodAsset(openPath, openId, openName);
        _closeAsset = AssetManager.LoadFmodAsset(closePath, closeId, closeName);
        _storageParent = storageParent;
        _width = width;
        _height = height;
    }

    public override void AddComponent(ModVehicle parentVehicle)
    {
        if (!_storageParent)
        {
            _storageParent = parentVehicle.Prefab.transform.Find("Storage").gameObject;
        }

        var storageRoot = _storageParent.AddComponent<ChildObjectIdentifier>();
        foreach (Transform child in _storageParent.transform)
        {
            var container = child.gameObject.AddComponent<StorageContainer>();
            container.width = _width;
            container.height = _height;
            container.hoverText = "OpenStorage";
            container.storageLabel = "StorageLabel";
            container.storageRoot = storageRoot;
            container.prefabRoot = parentVehicle.Prefab;
            container.openSound = _openAsset;
            container.closeSound = _closeAsset;
            StorageContainers.Add(child, container);
        }
    }
}