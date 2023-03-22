using mset;
using UnityEngine;

namespace VehicleFramework;

public class ModVehicleBehaviour : Vehicle, IScheduledUpdateBehaviour, IInteriorSpace, IHandTarget
{
    public string GetProfileTag()
    {
        throw new System.NotImplementedException();
    }

    public void ScheduledUpdate()
    {
        throw new System.NotImplementedException();
    }

    public int scheduledUpdateIndex { get; set; }
    public void SetPlayerInsideState(bool state)
    {
        throw new System.NotImplementedException();
    }

    public bool IsPlayerInside()
    {
        throw new System.NotImplementedException();
    }

    public void OnPlayerKill()
    {
        throw new System.NotImplementedException();
    }

    public float GetInsideTemperature()
    {
        throw new System.NotImplementedException();
    }

    public bool CanBreathe()
    {
        throw new System.NotImplementedException();
    }

    public bool CanDropItemsInside()
    {
        throw new System.NotImplementedException();
    }

    public Sky GetInteriorSky()
    {
        throw new System.NotImplementedException();
    }

    public Sky GetGlassSky()
    {
        throw new System.NotImplementedException();
    }

    public GameObject GetGameObject()
    {
        throw new System.NotImplementedException();
    }

    public RespawnPoint GetRespawnPoint()
    {
        throw new System.NotImplementedException();
    }

    public VFXSurfaceTypes GetDefaultSurface()
    {
        throw new System.NotImplementedException();
    }

    public bool IsStoryBase()
    {
        throw new System.NotImplementedException();
    }

    public bool IsInside(GameObject obj)
    {
        throw new System.NotImplementedException();
    }

    public bool IsValidForRespawn()
    {
        throw new System.NotImplementedException();
    }
}