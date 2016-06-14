using UnityEngine;
using System.IO;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
#endif

public class WP8NativeSettings : ScriptableObject {

	public const string VERSION_NUMBER = "2.8";

	private const string WP8NativeSettingsAssetName = "WP8NativeSettings";
	private const string WP8NativeSettingsAssetExtension = ".asset";


	private static WP8NativeSettings instance = null;

	public static WP8NativeSettings Instance {
		
		get {
			if (instance == null) {
				instance = Resources.Load(WP8NativeSettingsAssetName) as WP8NativeSettings;
				
				if (instance == null) {				
					// If not found, autocreate the asset object.
					instance = CreateInstance<WP8NativeSettings>();
				
#if UNITY_EDITOR					
					FileStaticAPI.CreateFolder(SA_Config.SettingsPath);
					string fullPath = Path.Combine(Path.Combine("Assets", SA_Config.SettingsPath),
					                               WP8NativeSettingsAssetName + WP8NativeSettingsAssetExtension);
					AssetDatabase.CreateAsset(instance, fullPath);
#endif
				}
			}
			return instance;
		}
	}
}
