using UnityEngine;
using System.Collections;

#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
#else
using UnityEngine.SceneManagement;
#endif

public class WP8FeaturesExample : WPNFeaturePreview {

	public static WP8NativePreviewBackButton back = null;

	void Awake() {
		if(back == null) {
			back = WP8NativePreviewBackButton.Create();
		}
	}


	void OnGUI() {
		UpdateToStartPos();
		GUI.Label(new Rect(StartX, StartY, Screen.width, 40), "Basic Features", style);
		
		StartY+= YLableStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "In App Purchasing")) {
			#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
			Application.LoadLevel("In_AppPurchases");
			#else
			SceneManager.LoadScene("In_AppPurchases");
			#endif
		}
		
		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Native Pop Ups")) {
			#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
			Application.LoadLevel("PopUpExample");
			#else
			SceneManager.LoadScene("PopUpExample");
			#endif
		}

		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Multimedia")) {
			#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
			Application.LoadLevel("Multimedia");
			#else
			SceneManager.LoadScene("Multimedia");
			#endif
		}
	}

}
