namespace VehicleFrameworkNautilus.Extensions;

public static class GameObjectExtensions
{
    public static GameObject ApplyAlterraVehicleMaterial(this GameObject gameObject)
    {
        var marmosetShader = VehicleHelper.ReferenceVehicle.transform.Find("model/seatruck_anim/Seatruck_cabin_hatch_interior_geo").GetComponent<Renderer>().material.shader;
        foreach (var renderer in gameObject.GetComponentsInChildren<Renderer>())
        {
            foreach (var material in renderer.materials)
            {
                material.shader = marmosetShader;

                if (!material.name.Contains("Glass")) continue;
                material.EnableKeyword("_ZWRITE_ON");
                material.EnableKeyword("WBOIT");
                material.SetInt("_ZWrite", 0);
                material.SetInt("_Cutoff", 0);
                material.SetFloat("_SrcBlend", 1f);
                material.SetFloat("_DstBlend", 1f);
                material.SetFloat("_SrcBlend2", 0f);
                material.SetFloat("_DstBlend2", 10f);
                material.SetFloat("_AddSrcBlend", 1f);
                material.SetFloat("_AddDstBlend", 1f);
                material.SetFloat("_AddSrcBlend2", 0f);
                material.SetFloat("_AddDstBlend2", 10f);
                material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack | MaterialGlobalIlluminationFlags.RealtimeEmissive;
                material.renderQueue = 3101;
                material.enableInstancing = true;
            }
        }

        return gameObject;
    }
}