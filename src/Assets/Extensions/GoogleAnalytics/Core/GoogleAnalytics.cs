using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif


#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
#else
using UnityEngine.SceneManagement;
#endif

public class GoogleAnalytics : MonoBehaviour {


	public const string GOOGLE_ANALYTICS_CLIENTID_PREF_KEY = "google_analytics_clientid_pref_key";
	public const string GOOGLE_ANALYTICS_SYSTEM_INFO_SUBMITED = "system_info_submited";
	public const string SYSTEM_INFO = "SystemInfo";


	private static GoogleAnalytics Instance = null;

	private static string _ClientId;

	private static GoogleAnalyticsClient cleint = null;
	private static string CurrentLevelName;


	private static bool IsSessionStarted = false;



	//--------------------------------------
	// INITIALIZE
	//--------------------------------------


	void Awake() {

		if (Instance != null) {
			DestroyImmediate(gameObject);
			return;
		} else  {
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}

		name = "Google Analytics";



		GenerateClientId();
		cleint =  new GoogleAnalyticsClient(_ClientId);


		if(!IsSessionStarted) {

			Client.CreateHit(GoogleAnalyticsHitType.SCREENVIEW);
			Client.StartSession();
			Client.SetUserLanguage(Application.systemLanguage.ToString());
			Client.SetScreenResolution(Screen.width, Screen.height);
			Client.Send();

			IsSessionStarted = true;
		}

		SendFirstScreenHit();
		SubmitSystemInfo();



		if(GoogleAnalyticsSettings.Instance.AutoExceptionTracking) {
			#if UNITY_3_5 || UNITY_4_0 || UNITY_4_1	|| UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 
			Application.RegisterLogCallback(HandleLog);
			#else
			Application.logMessageReceived += HandleLog;
			#endif
		}

		if(GoogleAnalyticsSettings.Instance.AutoCampaignTracking) {
			GA_AndroidTools.RequestReffer();
		}




		#if UNITY_EDITOR
			if(GoogleAnalyticsSettings.Instance.UsePlayerSettingsForAppInfo) {
				GoogleAnalyticsSettings.Instance.AppName = PlayerSettings.productName;
				GoogleAnalyticsSettings.Instance.AppVersion = PlayerSettings.bundleVersion;
			}
		#endif

	}


	public static void SetTrackingId(string TrackingId) {
		StartTracking();
		cleint.GenerateHeaders(TrackingId);
	}


	public static void StartTracking() {
		if(Instance == null) {
			GameObject an = new GameObject("Google Analytics");
			an.AddComponent<GoogleAnalytics>();
		}
	}

	//--------------------------------------
	// EVENTS
	//--------------------------------------
	
	void OnLevelWasLoaded(int level) {
		TrackNewLevel();
	}


	void OnApplicationPause(bool paused) {

		if(!GoogleAnalyticsSettings.Instance.AutoAppResumeTracking) {
			return;
		}
		
		if (paused) {
			Client.CreateHit(GoogleAnalyticsHitType.APPVIEW);
			//Client.SetScreenName(GoogleAnalyticsSettings.Instance.AppName + " - Enter Background");

			Client.EndSession();
			Client.Send();
		} else {
			Client.CreateHit(GoogleAnalyticsHitType.APPVIEW);
			//Client.SetScreenName(GoogleAnalyticsSettings.Instance.AppName + " - Enter Foreground");

			Client.StartSession();
			Client.Send();
		}
	}


	void OnApplicationQuit() {
		if(!GoogleAnalyticsSettings.Instance.AutoAppQuitTracking) {
			return;
		}

		Client.CreateHit(GoogleAnalyticsHitType.APPVIEW);
		//Client.SetScreenName(GoogleAnalyticsSettings.Instance.AppName + " - Quit");
		Client.EndSession();
		Client.Send();
	}
	


	void HandleLog (string logString , string stackTrace, LogType type) {

		if(!GoogleAnalyticsSettings.Instance.AutoExceptionTracking) {
			return;
		}


		if(type == LogType.Exception) {
			Client.CreateHit (GoogleAnalyticsHitType.EXCEPTION);
			Client.SetExceptionDescription (logString);
			Client.SetScreenName (LoadedLevelName);
			Client.SetDocumentTitle (stackTrace);
			Client.SetIsFatalException (false);
			Client.Send ();
		}


		if(type == LogType.Error) {
			Client.CreateHit (GoogleAnalyticsHitType.EXCEPTION);
			Client.SetExceptionDescription (logString);
			Client.SetScreenName (LoadedLevelName);
			Client.SetDocumentTitle (stackTrace);
			Client.SetIsFatalException (false);
			Client.Send ();
		}

	}

	void OnReferalIntentReciver(string referrerString) {


		int urlBeginIndex = referrerString.LastIndexOf('?');


		if(urlBeginIndex != -1) {
			urlBeginIndex++;
			referrerString = referrerString.Substring(urlBeginIndex, referrerString.Length - urlBeginIndex);
		}

		Client.CreateHit (GoogleAnalyticsHitType.APPVIEW);

		string[] StringParams =  referrerString.Split('&');
		foreach(string kv in StringParams) {
			string[] KeyValue =  kv.Split('=');
			string key = KeyValue[0];
			string value = KeyValue[1];


			switch(key) {
			case "utm_campaign":
				Client.SetCampaignName(value);
				break;
			case "utm_source":
				Client.SetCampaignSource(value);
				break;

			case "utm_medium":
				Client.SetCampaignMedium(value);
				break;

			case "utm_term":
				string[] keywords = value.Split('+');
				foreach(string keyword in keywords) {
					Client.AddCampaignKeyword(keyword);
				}
				break;
			case "utm_content":
				Client.SetCampaignContent(value);
				break;
			case "gclid":
				Client.SetGoogleAdWordsID(value);
				break;
			}
		}



	
		Client.Send ();


	}

	//--------------------------------------
	//  GET / SET
	//--------------------------------------

	
	public static GoogleAnalyticsClient Client {
		get {
			if(cleint == null) {
				StartTracking();
			}

			return cleint;
		}
	}

	public static string ClientId {
		get {
			return _ClientId;
		}
	}	

	public static string LoadedLevelName {
		get {
			#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
			return Application.loadedLevelName;
			#else
			return SceneManager.GetActiveScene().name;
			#endif
		}
	}	


	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------

	public static void SubmitSystemInfo() {
		if(GoogleAnalyticsSettings.Instance.SubmitSystemInfoOnFirstLaunch) {
			if(!PlayerPrefs.HasKey(GOOGLE_ANALYTICS_SYSTEM_INFO_SUBMITED)) {
				PlayerPrefs.SetInt(GOOGLE_ANALYTICS_SYSTEM_INFO_SUBMITED, 1);


				Client.SendEventHit(SYSTEM_INFO, "deviceModel", SystemInfo.deviceModel);
				Client.SendEventHit(SYSTEM_INFO, "deviceName", SystemInfo.deviceName);
				Client.SendEventHit(SYSTEM_INFO, "deviceType", SystemInfo.deviceType.ToString());
				Client.SendEventHit(SYSTEM_INFO, "graphicsDeviceID", SystemInfo.graphicsDeviceID.ToString(), SystemInfo.graphicsDeviceID);
				Client.SendEventHit(SYSTEM_INFO, "graphicsDeviceVendorID", SystemInfo.graphicsDeviceVendorID.ToString(), SystemInfo.graphicsDeviceVendorID);
				Client.SendEventHit(SYSTEM_INFO, "graphicsDeviceName", SystemInfo.graphicsDeviceName);
				Client.SendEventHit(SYSTEM_INFO, "graphicsDeviceVendor", SystemInfo.graphicsDeviceVendor);
				Client.SendEventHit(SYSTEM_INFO, "graphicsDeviceVersion", SystemInfo.graphicsDeviceVersion);
				Client.SendEventHit(SYSTEM_INFO, "graphicsShaderLevel", SystemInfo.graphicsShaderLevel.ToString(), SystemInfo.graphicsShaderLevel);



				Client.SendEventHit(SYSTEM_INFO, "graphicsMemorySize", SystemInfo.graphicsMemorySize.ToString() + "MB", SystemInfo.graphicsMemorySize);
				Client.SendEventHit(SYSTEM_INFO, "systemMemorySize", SystemInfo.systemMemorySize.ToString() + "MB", SystemInfo.systemMemorySize);
				Client.SendEventHit(SYSTEM_INFO, "systemLanguage", Application.systemLanguage.ToString());


				Client.SendEventHit(SYSTEM_INFO, "operatingSystem", SystemInfo.operatingSystem);
				Client.SendEventHit(SYSTEM_INFO, "processorType", SystemInfo.processorType);
				Client.SendEventHit(SYSTEM_INFO, "processorCount", SystemInfo.processorCount.ToString(), SystemInfo.processorCount);


		

				Client.SendEventHit(SYSTEM_INFO, "supportsAccelerometer", SystemInfo.supportsAccelerometer ? "true" : "false", SystemInfo.supportsAccelerometer ? 1 : 0);
				Client.SendEventHit(SYSTEM_INFO, "supportsLocationService", SystemInfo.supportsLocationService ? "true" : "false", SystemInfo.supportsLocationService ? 1 : 0);
				Client.SendEventHit(SYSTEM_INFO, "supportsVibration", SystemInfo.supportsVibration ? "true" : "false", SystemInfo.supportsVibration ? 1 : 0);


				Client.SendEventHit(SYSTEM_INFO, "supportsRenderTextures", SystemInfo.supportsRenderTextures ? "true" : "false", SystemInfo.supportsRenderTextures ? 1 : 0);
				Client.SendEventHit(SYSTEM_INFO, "supportsImageEffects", SystemInfo.supportsImageEffects ? "true" : "false", SystemInfo.supportsImageEffects ? 1 : 0);
				Client.SendEventHit(SYSTEM_INFO, "supportsShadows", SystemInfo.supportsShadows ? "true" : "false", SystemInfo.supportsShadows ? 1 : 0);


			}
		}
	}


	public static void RestorePrefKeys() {
		PlayerPrefs.SetString(GOOGLE_ANALYTICS_CLIENTID_PREF_KEY, _ClientId);
		PlayerPrefs.SetInt(GOOGLE_ANALYTICS_SYSTEM_INFO_SUBMITED, 1);
	}


	public static void Send(string request) {
		#if UNITY_WEBPLAYER  
			#if !UNITY_EDITOR
				Application.ExternalCall("SendGA", Client.AnalyticsHost, request);
			#endif
		#else
			byte[] data = System.Text.Encoding.UTF8.GetBytes(request);
			if(GoogleAnalyticsSettings.Instance.IsRequetsCachingEnabled) {
				Instance.StartCoroutine(Instance.SendAnalytics(data, request));
			} else {
				SendSkipCache(request);
			}
			
		#endif
	}

	public static void SendSkipCache(string request) {
		byte[] data = System.Text.Encoding.UTF8.GetBytes(request);
		Client.GenerateWWW(data);
	}

		
		
	private IEnumerator SendAnalytics (byte[] data, string cache) {
		// Start a download of the given URL
		WWW www = Client.GenerateWWW(data);
		
		// Wait for download to complete
		yield return www;


		if(www.error != null) {
			GoogleAnalyticsRequestCache.SaveRequest(cache);
		} else {
			GoogleAnalyticsRequestCache.SendChashedRequests();
		}

	}

	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------

	private static void SendFirstScreenHit() {
		if(GoogleAnalyticsSettings.Instance.AutoLevelTracking) {

			CurrentLevelName = LoadedLevelName;

			Client.CreateHit(GoogleAnalyticsHitType.APPVIEW);
			Client.SetScreenResolution(Screen.currentResolution.width, Screen.currentResolution.height);
			Client.SetViewportSize(Screen.width, Screen.height);
			Client.SetUserLanguage(Application.systemLanguage.ToString());
			Client.SetScreenName(GoogleAnalyticsSettings.Instance.LevelPrefix + CurrentLevelName + GoogleAnalyticsSettings.Instance.LevelPostfix);
		
			Client.Send();
		}
	}

	private static void TrackNewLevel() {
		if(GoogleAnalyticsSettings.Instance.AutoLevelTracking) {
		
			if(!CurrentLevelName.Equals(LoadedLevelName)) {
				CurrentLevelName = LoadedLevelName;
				Client.SendScreenHit(GoogleAnalyticsSettings.Instance.LevelPrefix + CurrentLevelName + GoogleAnalyticsSettings.Instance.LevelPostfix);
			}
		}
	}
	


	private static void GenerateClientId() {
		if(PlayerPrefs.HasKey(GOOGLE_ANALYTICS_CLIENTID_PREF_KEY)) {
			_ClientId = PlayerPrefs.GetString(GOOGLE_ANALYTICS_CLIENTID_PREF_KEY);
		} else {
			#if UNITY_ANDROID || UNITY_BLACKBERRY
			_ClientId = RandomString(32);
			#else 
			_ClientId = RandomString(8) + SystemInfo.deviceUniqueIdentifier;
			#endif

			PlayerPrefs.SetString(GOOGLE_ANALYTICS_CLIENTID_PREF_KEY, _ClientId);
		}
	}


	private static System.Random random = new System.Random((int)System.DateTime.Now.Ticks);
	private static string RandomString(int size) {
		
		System.Text.StringBuilder builder = new System.Text.StringBuilder();
		char ch;
		for (int i = 0; i < size; i++) {
			ch = System.Convert.ToChar(System.Convert.ToInt32(System.Math.Floor(26 * random.NextDouble() + 65)));                 
			builder.Append(ch);
		}
		
		return builder.ToString();
	}







}
