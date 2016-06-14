using UnityEngine;
using System.Collections;

#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
#else
using UnityEngine.SceneManagement;
#endif

public class WP8NativePreviewBackButton : WPNFeaturePreview {
	
	private string initalSceneName = "scene";

	public static WP8NativePreviewBackButton Create() {
		return new GameObject("BackButton").AddComponent<WP8NativePreviewBackButton>();
	}

	void Awake() {
		DontDestroyOnLoad(gameObject);
		#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
		initalSceneName = Application.loadedLevelName;
		#else
		initalSceneName = SceneManager.GetActiveScene().name;
		#endif
	}

	void OnGUI() {
		float bw = 120;
		float x = Screen.width - bw * 1.2f ;
		float y = bw * 0.2f;

		#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
		if(!Application.loadedLevelName.Equals(initalSceneName)) {
		#else
		if(!SceneManager.GetActiveScene().name.Equals(initalSceneName)) {
		#endif

			Color customColor = GUI.color;
			GUI.color = Color.green;

			if(GUI.Button(new Rect(x, y, bw, bw * 0.4f), "Back")) {
				#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
				Application.LoadLevel(initalSceneName);
				#else
				SceneManager.LoadScene(initalSceneName);
				#endif
			}

			GUI.color = customColor;
		}
	}
}
