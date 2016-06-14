using UnityEngine;
using System.IO;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
#endif



public class GoogleAnalyticsSettings : ScriptableObject {

	public static string VERSION_NUMBER = "2.9";

	
	[SerializeField]
	public List<GAProfile> accounts =  new List<GAProfile>();

	[SerializeField]
	public List<GAPlatfromBound> platfromBounds =  new List<GAPlatfromBound>();



	public bool showAdditionalParams = false;
	public bool showAdvancedParams = false;
	public bool showCParams = false;
	
	public bool showAccounts = true;
	public bool showPlatfroms = false;
	public bool showTestingMode = false;



	public string AppName = "My App";
	public string AppVersion = "0.0.1";

	public bool UseHTTPS = false;
	public bool StringEscaping = true;
	public bool EditorAnalytics = true;
	public bool IsDisabled = false;


	public bool IsTestingModeEnabled = false;
	public int TestingModeAccIndex = 0;


	public bool IsRequetsCachingEnabled= true;
	public bool IsQueueTimeEnabled = true;
	

	public bool AutoLevelTracking = true;
	public string LevelPrefix = "Level_";
	public string LevelPostfix = "";


	public bool AutoAppQuitTracking = true;
	public bool AutoCampaignTracking  = true;
	public bool AutoExceptionTracking = true;
	public bool AutoAppResumeTracking = true;
	public bool SubmitSystemInfoOnFirstLaunch = true;




	public bool UsePlayerSettingsForAppInfo = true;



	private const string AnalyticsSettingsAssetName = "GoogleAnalyticsSettings";
	private const string AnalyticsSettingsAssetExtension = ".asset";





	private static GoogleAnalyticsSettings instance = null;



	public static GoogleAnalyticsSettings Instance {

		get {


			if (instance == null) {
				instance = Resources.Load(AnalyticsSettingsAssetName) as GoogleAnalyticsSettings;
				
				if (instance == null) {

					// If not found, autocreate the asset object.
					instance = CreateInstance<GoogleAnalyticsSettings>();
					#if UNITY_EDITOR
					FileStaticAPI.CreateFolder(SA_Config.SettingsPath);
					
					string fullPath = Path.Combine(Path.Combine("Assets", SA_Config.SettingsPath),
					                               AnalyticsSettingsAssetName + AnalyticsSettingsAssetExtension
					                               );
					
					AssetDatabase.CreateAsset(instance, fullPath);
					#endif
				}
			}
			return instance;
		}
	}


	public void UpdateVersionAndName() {

		#if UNITY_EDITOR
			AppName = PlayerSettings.productName;
			
			#if UNITY_ANDROID
			AppVersion = PlayerSettings.bundleVersion + "-" + PlayerSettings.Android.bundleVersionCode;
			#else
			AppVersion = PlayerSettings.bundleVersion;
			#endif

		#endif
	}

	public void AddProfile(GAProfile p) {
		accounts.Add(p);
	}

	public void RemoveProfile(GAProfile p) {
		accounts.Remove(p);
	}

	public void SetProfileIndexForPlatfrom(RuntimePlatform platfrom, int index, bool IsTesting) {
		foreach(GAPlatfromBound pb in platfromBounds) {
			if(pb.platfrom.Equals(platfrom)) {

				if(IsTesting) {
					pb.profileIndexTestMode = index;
				} else {
					pb.profileIndex = index;
				}

				return;
			}
		}

		GAPlatfromBound bound =  new GAPlatfromBound();
		bound.platfrom = platfrom;
		bound.profileIndex = 0;
		bound.profileIndexTestMode = 0;
		if(IsTesting) {
			bound.profileIndexTestMode = index;
		} else {
			bound.profileIndex = index;
		}

		platfromBounds.Add(bound);

	}

	public int GetProfileIndexForPlatfrom(RuntimePlatform platfrom, bool IsTestMode) {
		foreach(GAPlatfromBound pb in platfromBounds) {
			if(pb.platfrom.Equals(platfrom)) {
				int index =  pb.profileIndex;
				if(IsTestMode) {
					index = pb.profileIndexTestMode;
				} 

				if(index < accounts.Count) {
					return index;
				} else {
					return 0;
				}
			}
		}

		return 0;
	}
	
	public string[] GetProfileNames() {
		List<string> names =  new List<string>();
		foreach(GAProfile p in accounts) {
			names.Add(p.Name);
		}

		return names.ToArray();
	}

	public int GetProfileIndex(GAProfile p ) {
		int index = 0;
		string[] names = GetProfileNames();

		foreach(string name in names) {
			if(name.Equals(p.Name)) {
				return index;
			}

			index++;
		}

		return 0;

	}




	public GAProfile GetCurentProfile() {
		return GetPrfileForPlatfrom(Application.platform, IsTestingModeEnabled);
	}

	public GAProfile GetPrfileForPlatfrom(RuntimePlatform platfrom, bool IsTestMode) {

		if (accounts.Count == 0) {
			return new GAProfile();
		}



		return accounts[GetProfileIndexForPlatfrom(platfrom, IsTestMode)];

	}

}
