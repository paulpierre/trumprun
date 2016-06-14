using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SIS
{
    [InitializeOnLoad]
    public class PluginSetup : EditorWindow
    {
        private static string packagesPath;

        private Packages selectedPackage = Packages.UnityIAP;
        private enum Packages
        {
            UnityIAP,
            VoxelBusters,
            StansAssets,
            Prime31
        }


        [MenuItem("Window/Simple IAP System/Plugin Setup")]
        static void Init()
        {
            packagesPath = "/Packages/";
            EditorWindow window = EditorWindow.GetWindowWithRect(typeof(PluginSetup), new Rect(0, 0, 350, 250), false, "Plugin Setup");

            var script = MonoScript.FromScriptableObject(window);
            string thisPath = AssetDatabase.GetAssetPath(script);
            packagesPath = thisPath.Replace("/PluginSetup.cs", packagesPath);
        }


        void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Simple IAP System - Billing Plugin Setup", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Please choose the billing plugin you are using for SIS:");

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();

            selectedPackage = (Packages)EditorGUILayout.EnumPopup(selectedPackage);

            if (GUILayout.Button("?", GUILayout.Width(20)))
            {
                switch (selectedPackage)
                {
                    case Packages.UnityIAP:
                        Application.OpenURL("https://unity3d.com/services/analytics/iap");
                        break;
                    case Packages.Prime31:
                        Application.OpenURL("https://www.assetstore.unity3d.com/#/publisher/270");
                        break;
                    case Packages.StansAssets:
                        Application.OpenURL("https://www.assetstore.unity3d.com/#/publisher/2256");
                        break;
                    case Packages.VoxelBusters:
                        Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/publisher/11527");
                        break;
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            if (GUILayout.Button("Import"))
            {
                AssetDatabase.ImportPackage(packagesPath + selectedPackage.ToString() + ".unitypackage", true);
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Note:", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Unity IAP is a free option to the paid plugins (Unity 5.3+).");
            EditorGUILayout.LabelField("If you only want virtual currency and items (no real money");
            EditorGUILayout.LabelField("products) then you don't have to import a billing plugin.");

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Please read the PDF documentation for further details.");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Support links: Window > Simple IAP System > About.");
        }
    }
}