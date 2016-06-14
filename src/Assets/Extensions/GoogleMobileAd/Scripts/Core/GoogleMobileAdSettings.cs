using UnityEngine;
using System.IO;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
#endif

public class GoogleMobileAdSettings : ScriptableObject {

	public const string VERSION_NUMBER = "7.5";
	public const string PLAY_SERVICE_VERSION = "8487000";

	public string IOS_InterstisialsUnitId = "";
	public string IOS_BannersUnitId = "";

	public string Android_InterstisialsUnitId = "";
	public string Android_BannersUnitId = "";

	public string WP8_InterstisialsUnitId = "";
	public string WP8_BannersUnitId = "";


	public bool IsIOSSettinsOpened = true;
	public bool IsAndroidSettinsOpened = true;
	public bool IsWP8SettinsOpened = true;

	public bool IsTestSettinsOpened = false;

	public bool ShowActions = false;
	public bool KeepManifestClean = true;

	public bool TagForChildDirectedTreatment = false;
	
	
	[SerializeField]
	public List<GADTestDevice> testDevices =  new List<GADTestDevice>();

	public bool IsKeywordsOpened = false;
	public List<string> DefaultKeywords =  new List<string>();



	private const string ISNSettingsAssetName = "GoogleMobileAdSettings";
	private const string ISNSettingsAssetExtension = ".asset";

	private static GoogleMobileAdSettings instance = null;

	
	public static GoogleMobileAdSettings Instance {
		
		get {
			if (instance == null) {
				instance = Resources.Load(ISNSettingsAssetName) as GoogleMobileAdSettings;
				
				if (instance == null) {
					
					// If not found, autocreate the asset object.
					instance = CreateInstance<GoogleMobileAdSettings>();
					#if UNITY_EDITOR
					FileStaticAPI.CreateFolder(SA_Config.SettingsPath);


					
					string fullPath = Path.Combine(Path.Combine("Assets", SA_Config.SettingsPath),
					                               ISNSettingsAssetName + ISNSettingsAssetExtension
					                               );
					
					AssetDatabase.CreateAsset(instance, fullPath);
					#endif
				}
			}
			return instance;
		}
	}



	public void AddDevice(GADTestDevice p) {
		testDevices.Add(p);
	}
	
	public void RemoveDevice(GADTestDevice p) {
		testDevices.Remove(p);
	}


}
