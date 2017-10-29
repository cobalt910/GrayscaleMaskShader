// ==============================
// Copyright (c) cobalt910
// Neogene Games
// http://youtube.com/cobalt9101/
// ==============================

using UnityEngine;
using UnityEditor;

public class GrayscaleEditor : ShaderGUI
{
    private MaterialEditor m_MaterialEditor;

    private MaterialProperty Color;
    private MaterialProperty MainTex;
    private MaterialProperty ColorPow;

    private MaterialProperty EmissionCol;
    private MaterialProperty EmissionTex;
    private MaterialProperty EmissionPow;

    private MaterialProperty NoiseTex;
    private MaterialProperty NoisePow;

    private MaterialProperty BurnTex;
    private MaterialProperty BurnSize;
    private MaterialProperty BurnPow;


    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        FindProperties(properties);
        m_MaterialEditor = materialEditor;
        Material mat = materialEditor.target as Material;
        ShaderPropertiesGUI(mat);
    }

    public static class Styles
    {
        public static GUIContent color = new GUIContent("Color");
        public static GUIContent mainTex = new GUIContent("Albedo");
        public static GUIContent colorPow = new GUIContent("Color Power");

        public static GUIContent emissionCol = new GUIContent("Emission Color");
        public static GUIContent emissionTex = new GUIContent("Emission Texture");
        public static GUIContent emissionPow = new GUIContent("Emission Power");

        public static GUIContent noiseTex = new GUIContent("Noise Texture");
        public static GUIContent noisePow = new GUIContent("Noise Power");

        public static GUIContent burnTex = new GUIContent("Burn Texture");
        public static GUIContent burnSize = new GUIContent("Burn Size");
        public static GUIContent burnPow = new GUIContent("Burn Power");
    }

    private void FindProperties(MaterialProperty[] props)
    {
        Color = FindProperty("_Color", props);
        MainTex = FindProperty("_MainTex", props);
        ColorPow = FindProperty("_ColorPow", props);

        EmissionCol = FindProperty("_EmissionCol", props);
        EmissionTex = FindProperty("_EmissionTex", props);
        EmissionPow = FindProperty("_EmissionPow", props);

        NoiseTex = FindProperty("_NoiseTex", props);
        NoisePow = FindProperty("_NoisePow", props);

        BurnTex = FindProperty("_BurnTex", props);
        BurnSize = FindProperty("_BurnSize", props);
        BurnPow = FindProperty("_BurnPow", props);
    }

    private void ShaderPropertiesGUI(Material mat)
    {
        EditorGUIUtility.labelWidth = 0;

        EditorGUI.BeginChangeCheck();
        {
            GUILayout.Label("Main Field", EditorStyles.centeredGreyMiniLabel);
            if(MainTex.textureValue != null)
                m_MaterialEditor.ColorProperty(Color, "Color");
            m_MaterialEditor.TexturePropertySingleLine(
                Styles.mainTex, MainTex,
                MainTex.textureValue != null ? ColorPow : null);
            GUILayout.Space(10);

            if (EmissionTex.textureValue != null)
                m_MaterialEditor.ColorProperty(EmissionCol, "Emission Color");
            m_MaterialEditor.TexturePropertySingleLine(
                Styles.emissionTex, EmissionTex,
                EmissionTex.textureValue != null ? EmissionPow : null);

            GUILayout.Label("Noise & Burn", EditorStyles.centeredGreyMiniLabel);
            m_MaterialEditor.TexturePropertySingleLine(Styles.noiseTex, NoiseTex);
            if (NoiseTex.textureValue != null)
                m_MaterialEditor.RangeProperty(NoisePow, "Noise Power");
            GUILayout.Space(10);

            m_MaterialEditor.TexturePropertySingleLine(Styles.burnTex, BurnTex);
            if (BurnTex.textureValue != null)
            {
                m_MaterialEditor.RangeProperty(BurnSize, "Burn Size");
                m_MaterialEditor.RangeProperty(BurnPow, "Burn Power");
            }
        }
        EditorGUI.EndChangeCheck();
    }
}
