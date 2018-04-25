using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using UnityEditor.Utils;
using UnityEditorInternal;

#if UNITY_EDITOR
namespace MK.XRay
{
    public class MKXRayFreeEditor : ShaderGUI
    {
        public static class GuiStyles
        {
            public static GUIStyle header = new GUIStyle("ShurikenModuleTitle")
            {
                font = (new GUIStyle("Label")).font,
                border = new RectOffset(15, 7, 4, 4),
                fixedHeight = 22,
                contentOffset = new Vector2(20f, -2f),
            };

            public static GUIStyle headerCheckbox = new GUIStyle("ShurikenCheckMark");
            public static GUIStyle headerCheckboxMixed = new GUIStyle("ShurikenCheckMarkMixed");
        }

        private static class GUIContentCollection
        {
            public static GUIContent mainColor = new GUIContent("Inside Color", "Basic color tint");
            public static GUIContent xRayRimSize = new GUIContent("Rim Size", "Amount of highlighted areas by rim");
            public static GUIContent xRayInside = new GUIContent("Inside", "Intensity of the inside color");
            public static GUIContent xRayRimColor = new GUIContent("Rim Color", "Color of the rim highlight");
            public static GUIContent emissionColor = new GUIContent("Emission", "Color of the emission");
        }

        //hdr config
        private ColorPickerHDRConfig colorPickerHDRConfig = new ColorPickerHDRConfig(0f, 99f, 1 / 99f, 3f);

        //Editor Properties
        private MaterialProperty showMainBehavior = null;
        private MaterialProperty showXRayBehavior = null;

        //Main
        private MaterialProperty mainColor = null;

        //XRay
        private MaterialProperty xRayRimColor = null;
        private MaterialProperty xRayRimSize = null;
        private MaterialProperty xRayInside = null;

        //Emission
        private MaterialProperty emissionColor = null;

        private bool showGIField = false;

        public void FindProperties(MaterialProperty[] props, Material mat)
        {
            //Editor Properties
            showMainBehavior = FindProperty(MKXRayFreeMaterialHelper.PropertyNames.SHOW_MAIN_BEHAVIOR, props);
            showXRayBehavior = FindProperty(MKXRayFreeMaterialHelper.PropertyNames.SHOW_XRAY_BEHAVIOR, props);

            //Main
            mainColor = FindProperty(MKXRayFreeMaterialHelper.PropertyNames.MAIN_COLOR, props);

            //XRay
            xRayRimColor = FindProperty(MKXRayFreeMaterialHelper.PropertyNames.XRAY_RIM_COLOR, props);
            xRayRimSize = FindProperty(MKXRayFreeMaterialHelper.PropertyNames.XRAY_RIM_SIZE, props);
            xRayInside = FindProperty(MKXRayFreeMaterialHelper.PropertyNames.XRAY_INSIDE, props);

            //Emission
            emissionColor = FindProperty(MKXRayFreeMaterialHelper.PropertyNames.EMISSION_COLOR, props);
        }

        //Colorfield
        private void ColorProperty(MaterialProperty prop, bool showAlpha, bool hdrEnabled, GUIContent label)
        {
            EditorGUI.showMixedValue = prop.hasMixedValue;
            EditorGUI.BeginChangeCheck();
            Color c = EditorGUILayout.ColorField(label, prop.colorValue, false, showAlpha, hdrEnabled, colorPickerHDRConfig);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
                prop.colorValue = c;
        }

        //Setup GI emission
        private void SetGIFlags()
        {
            foreach (Material obj in emissionColor.targets)
            {
                bool emissive = true;
                if (MKXRayFreeMaterialHelper.GetEmissionColor(obj) == Color.black)
                {
                    emissive = false;
                }
                MaterialGlobalIlluminationFlags flags = obj.globalIlluminationFlags;
                if ((flags & (MaterialGlobalIlluminationFlags.BakedEmissive | MaterialGlobalIlluminationFlags.RealtimeEmissive)) != 0)
                {
                    flags &= ~MaterialGlobalIlluminationFlags.EmissiveIsBlack;
                    if (!emissive)
                        flags |= MaterialGlobalIlluminationFlags.EmissiveIsBlack;

                    obj.globalIlluminationFlags = flags;
                }
            }
        }

        //BoldToggle
        private void ToggleBold(MaterialEditor materialEditor, MaterialProperty prop)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(prop.displayName, EditorStyles.boldLabel, GUILayout.Width(100));
            materialEditor.ShaderProperty(prop, "");
            EditorGUILayout.EndHorizontal();
        }

        public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
        {
            if (material.HasProperty(MKXRayFreeMaterialHelper.PropertyNames.EMISSION))
            {
                MKXRayFreeMaterialHelper.SetEmissionColor(material, material.GetColor(MKXRayFreeMaterialHelper.PropertyNames.EMISSION));
            }
            base.AssignNewShaderToMaterial(material, oldShader, newShader);

            MaterialProperty[] properties = MaterialEditor.GetMaterialProperties(new Material[] { material });
            FindProperties(properties, material);

            SetGIFlags();
        }

        private bool HandleBehavior(string title, MaterialProperty behavior, MaterialEditor materialEditor)
        {
            EditorGUI.showMixedValue = behavior.hasMixedValue;
            var rect = GUILayoutUtility.GetRect(16f, 22f, GuiStyles.header);
            rect.x -= 10;
            rect.width += 10;
            var e = Event.current;

            GUI.Box(rect, title, GuiStyles.header);

            var foldoutRect = new Rect(EditorGUIUtility.currentViewWidth * 0.5f, rect.y + 2, 13f, 13f);
            if (behavior.hasMixedValue)
            {
                foldoutRect.x -= 13;
                foldoutRect.y -= 2;
            }

            EditorGUI.BeginChangeCheck();
            if (e.type == EventType.MouseDown)
            {
                if (rect.Contains(e.mousePosition))
                {
                    if (behavior.hasMixedValue)
                        behavior.floatValue = 0.0f;
                    else
                        behavior.floatValue = Convert.ToSingle(!Convert.ToBoolean(behavior.floatValue));
                    e.Use();
                }
            }
            if (EditorGUI.EndChangeCheck())
            {
                if (Convert.ToBoolean(behavior.floatValue))
                    materialEditor.RegisterPropertyChangeUndo(behavior.displayName + " Show");
                else
                    materialEditor.RegisterPropertyChangeUndo(behavior.displayName + " Hide");
            }

            EditorGUI.showMixedValue = false;


            if (e.type == EventType.Repaint && behavior.hasMixedValue)
                EditorStyles.radioButton.Draw(foldoutRect, "", false, false, true, false);
            else
                EditorGUI.Foldout(foldoutRect, Convert.ToBoolean(behavior.floatValue), "");

            if (behavior.hasMixedValue)
                return true;
            else
                return Convert.ToBoolean(behavior.floatValue);
        }

        override public void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            Material targetMat = materialEditor.target as Material;
            //get properties
            FindProperties(properties, targetMat);

            if (emissionColor.colorValue != Color.black)
                showGIField = true;
            else
                showGIField = false;

            EditorGUI.BeginChangeCheck();
            //main settings
            if (HandleBehavior("Main", showMainBehavior, materialEditor))
            {
                EditorGUI.BeginChangeCheck();

                EditorGUI.BeginChangeCheck();
                ColorProperty(emissionColor, false, true, GUIContentCollection.emissionColor);

                if (showGIField)
                    materialEditor.LightmapEmissionProperty(MaterialEditor.kMiniTextureFieldLabelIndentLevel + 1);
                if (EditorGUI.EndChangeCheck())
                {
                    SetGIFlags();
                }

#if UNITY_5_6_OR_NEWER
                materialEditor.EnableInstancingField();
#endif
            }

            //XRay settings
            if (HandleBehavior("XRay", showXRayBehavior, materialEditor))
            {
                ColorProperty(mainColor, false, false, GUIContentCollection.mainColor);
                materialEditor.ShaderProperty(xRayInside, GUIContentCollection.xRayInside);
                ColorProperty(xRayRimColor, false, true, GUIContentCollection.xRayRimColor);
                materialEditor.ShaderProperty(xRayRimSize, GUIContentCollection.xRayRimSize);
            }

            EditorGUI.EndChangeCheck();
        }
    }
}
#endif