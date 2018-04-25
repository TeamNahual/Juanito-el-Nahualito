using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK.XRay
{
    public static class MKXRayFreeMaterialHelper
    {
        public static class PropertyNames
        {
            //Editor Properties
            public const string SHOW_MAIN_BEHAVIOR = "_MKEditorShowMainBehavior";
            public const string SHOW_XRAY_BEHAVIOR = "_MKEditorShowXRayBehavior";

            //Main
            public const string MAIN_COLOR = "_Color";

            //XRay
            public const string XRAY_RIM_COLOR = "_XRayRimColor";
            public const string XRAY_RIM_SIZE = "_XRayRimSize";
            public const string XRAY_INSIDE = "_XRayInside";

            //Emission
            public const string EMISSION_COLOR = "_EmissionColor";
            public const string EMISSION = "_Emission";
        }

        //Main
        public static void SetMainColor(Material material, Color color)
        {
            material.SetColor(PropertyNames.MAIN_COLOR, color);
        }
        public static Color MainColor(Material material)
        {
            return material.GetColor(PropertyNames.MAIN_COLOR);
        }

        //XRay
        public static void SetXRayRimColor(Material material, Color color)
        {
            material.SetColor(PropertyNames.XRAY_RIM_COLOR, color);
        }
        public static Color GetXRayRimColor(Material material)
        {
            return material.GetColor(PropertyNames.XRAY_RIM_COLOR);
        }

        public static void SetXRayRimSize(Material material, float size)
        {
            material.SetFloat(PropertyNames.XRAY_RIM_SIZE, size);
        }
        public static float GetXRayRimSize(Material material)
        {
            return material.GetFloat(PropertyNames.XRAY_RIM_SIZE);
        }

        public static void SetXRayInside(Material material, float v)
        {
            material.SetFloat(PropertyNames.XRAY_INSIDE, v);
        }
        public static float GetXRayInside(Material material)
        {
            return material.GetFloat(PropertyNames.XRAY_INSIDE);
        }

        //Emission
        public static void SetEmissionColor(Material material, Color color)
        {
            material.SetColor(PropertyNames.EMISSION_COLOR, color);
        }
        public static Color GetEmissionColor(Material material)
        {
            return material.GetColor(PropertyNames.EMISSION_COLOR);
        }
    }
}