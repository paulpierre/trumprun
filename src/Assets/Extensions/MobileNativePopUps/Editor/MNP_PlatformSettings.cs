using UnityEngine;
using System.IO;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
#endif

public class MNP_PlatformSettings : ScriptableObject {

	private const string ISNSettingsAssetName = "MNPSettings";
	private const string ISNSettingsPath = "Extensions/MobileNativePopUps/Resources";
	private const string ISNSettingsAssetExtension = ".asset";
	public const string VERSION_NUMBER = "4.1";

	private static MNP_PlatformSettings instance = null;


	public static MNP_PlatformSettings Instance {
		
		get {
			if (instance == null) {
				instance = Resources.Load(ISNSettingsAssetName) as MNP_PlatformSettings;
				
				if (instance == null) {
					
					// If not found, autocreate the asset object.
					instance = CreateInstance<MNP_PlatformSettings>();
					#if UNITY_EDITOR
					//string properPath = Path.Combine(Application.dataPath, ISNSettingsPath);
					
					FileStaticAPI.CreateFolder(ISNSettingsPath);
					
					/*
					if (!Directory.Exists(properPath)) {
						AssetDatabase.CreateFolder("Extensions/", "GooglePlayCommon");
						AssetDatabase.CreateFolder("Extensions/GooglePlayCommon", "Resources");
					}
					*/
					
					string fullPath = Path.Combine(Path.Combine("Assets", ISNSettingsPath),
					                               ISNSettingsAssetName + ISNSettingsAssetExtension
					                               );
					
					AssetDatabase.CreateAsset(instance, fullPath);
					
					

					
					#endif
				}
			}
			return instance;
		}
	}
}
