﻿namespace VehicleFrameworkNautilus.Items.Vehicle.Components;

public abstract class HandlerComponent
{
    public HandlerComponent(BaseVehiclePrefab parentVehicle)
    {
        this.parentVehicle = parentVehicle;
        gameObject = parentVehicle.Model;
    }

    public GameObject gameObject { get; }
    public BaseVehiclePrefab parentVehicle { get; }

    public abstract void Instantiate();
}