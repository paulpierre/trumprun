
using UnityEngine;
using UnityEditor;
using System.Collections;

[InitializeOnLoad]
public class UM_Init  {



	
	static UM_Init () {

		#if UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8

		if(!UMSettingEditor.IsInstalled) {
			EditorApplication.update += OnEditorLoaded;
		} else {
			if(!UMSettingEditor.IsUpToDate) {
				EditorApplication.update += OnEditorLoaded;
			}
		}
		#endif
	}
	
	private static void OnEditorLoaded() {
		
		EditorApplication.update -= OnEditorLoaded;
		Debug.LogWarning("Ultimate Mobile Plugin Install Required. Opening Plugin settings...");
		Selection.activeObject = UltimateMobileSettings.Instance;
	}
	
}
