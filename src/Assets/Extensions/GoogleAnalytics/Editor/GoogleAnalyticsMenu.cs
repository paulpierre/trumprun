////////////////////////////////////////////////////////////////////////////////
//  
// @module V2D
// @author Osipov Stanislav lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using System.Collections;

public class GoogleAnalyticsMenu : EditorWindow {
	
	//--------------------------------------
	//  GENERAL
	//--------------------------------------

	#if UNITY_EDITOR

	[MenuItem("Window/Stan's Assets/Google Analytics/Edit Settings", false, 1)]
	public static void Edit() {
		Selection.activeObject = GoogleAnalyticsSettings.Instance;
	}

	[MenuItem("Window/Stan's Assets/Google Analytics/Create Analytics GameObject")]
	public static void Create() {
		GameObject an = new GameObject("Google Analytics");
		an.AddComponent<GoogleAnalytics>();
		Selection.activeObject = an;
	}

	//--------------------------------------
	//  Getting Started
	//--------------------------------------

	[MenuItem("Window/Stan's Assets/Google Analytics/Plugin Documentation/Getting Started/Setup")]
	public static void GAGTSetup() {
		string url = "https://unionassets.com/google-analytics-sdk/get-started-with-analytics-78";
		Application.OpenURL(url);
	}
		
	[MenuItem("Window/Stan's Assets/Google Analytics/Plugin Documentation/Getting Started/Tracking Options")]
	public static void GAGTTrackingOptions() {
		string url = "https://unionassets.com/google-analytics-sdk/plugin-set-up-80";
		Application.OpenURL(url);
	}

	//--------------------------------------
	//  Implementation
	//--------------------------------------

	[MenuItem("Window/Stan's Assets/Google Analytics/Plugin Documentation/Implementation/Using Basic Features Without Scripting")]
	public static void GAIUsingBasicFeaturesWithoutScripting() {
		string url = "https://unionassets.com/google-analytics-sdk/using-basic-features-without-scripting-265";
		Application.OpenURL(url);
	}

	[MenuItem("Window/Stan's Assets/Google Analytics/Plugin Documentation/Implementation/Scripting API")]
	public static void GAIScriptingAPI() {
		string url = "https://unionassets.com/google-analytics-sdk/plugin-set-up-82";
		Application.OpenURL(url);
	}

	[MenuItem("Window/Stan's Assets/Google Analytics/Plugin Documentation/Implementation/Web Player")]
	public static void GAIWebPlayer() {
		string url = "https://unionassets.com/google-analytics-sdk/web-player-83";
		Application.OpenURL(url);
	}

	[MenuItem("Window/Stan's Assets/Google Analytics/Plugin Documentation/Implementation/Campaign Measurement")]
	public static void GAICampaignMeasurement() {
		string url = "https://unionassets.com/google-analytics-sdk/campaign-measurement--468";
		Application.OpenURL(url);
	}

	[MenuItem("Window/Stan's Assets/Google Analytics/Plugin Documentation/Implementation/Advanced Fatures")]
	public static void GAIAdvancedFatures() {
		string url = "https://unionassets.com/google-analytics-sdk/advanced-fatures-270";
		Application.OpenURL(url);
	}

	//--------------------------------------
	//  MORE
	//--------------------------------------

	[MenuItem("Window/Stan's Assets/Google Analytics/Plugin Documentation/More/Released Apps with the plugin")]
	public static void GAMReleasedAppsWithThePlugin() {
		string url = "https://unionassets.com/google-analytics-sdk/released-apps-with-the-plugin-85";
		Application.OpenURL(url);
	}


	[MenuItem("Window/Stan's Assets/Google Analytics/Plugin Documentation/More/Playmaker")]
	public static void GAMPlaymaker() {
		string url = "https://unionassets.com/google-analytics-sdk/actions-list-84";
		Application.OpenURL(url);
	}

	[MenuItem("Window/Stan's Assets/Google Analytics/Plugin Documentation/More/Using Plugins with Java Script")]
	public static void GAMUsingPluginsWithJavaScript() {
		string url = "https://unionassets.com/google-analytics-sdk/plugin-set-up-82#measuring-refunds";
		Application.OpenURL(url);
	}















	[MenuItem("Window/Stan's Assets/Google Analytics/Google Documentation/Measurement Protocol Developer Guide")]
	public static void ProtocolDocumentation() {
		string url = "https://developers.google.com/analytics/devguides/collection/protocol/v1/devguide";
		Application.OpenURL(url);
	}


	[MenuItem("Window/Stan's Assets/Google Analytics/Google Documentation/Measurement Protocol Parameter Reference")]
	public static void ParamDocumentation() {
		string url = "https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters";
		Application.OpenURL(url);
	}





	[MenuItem("Window/Stan's Assets/Google Analytics/Discussions/Unity Forum")]
	public static void UnityForum() {
		string url = "http://goo.gl/B7YHzf";
		Application.OpenURL(url);
	}

	[MenuItem("Window/Stan's Assets/Google Analytics/Discussions/PlayMaker Forum")]
	public static void PlayMakerForum() {
		string url = "http://goo.gl/0bLwcT";
		Application.OpenURL(url);
	}

	[MenuItem("Window/Stan's Assets/Google Analytics/Support")]
	public static void Support() {
		string url = "http://goo.gl/QqSmBM";
		Application.OpenURL(url);
	}
	

	#endif

}
