using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(WP8NativeSettings))]
public class WP8NativeSettingsEditor : Editor {


	GUIContent SdkVersion   = new GUIContent("Plugin Version [?]", "This is Plugin version.  If you have problems or compliments please include this so we know exactly what version to look out for.");
	GUIContent SupportEmail = new GUIContent("Support [?]", "If you have any technical quastion, feel free to drop an e-mail");

//	private WP8NativeSettings settings;

	// Use this for initialization
	void Awake () {
	
	}
	
	public override void OnInspectorGUI () {
		//settings = WP8NativeSettings.Instance;
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

		AboutGUI ();
	}

	

	private void AboutGUI() {
		EditorGUILayout.HelpBox("About WP8Native", MessageType.None);
		EditorGUILayout.Space();

		SelectableLabelField(SdkVersion, WP8NativeSettings.VERSION_NUMBER);
		SelectableLabelField(SupportEmail, "stans.assets@gmail.com");
	}

	private void SelectableLabelField(GUIContent label, string value) {
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(label, GUILayout.Width(180), GUILayout.Height(16));
		EditorGUILayout.SelectableLabel(value, GUILayout.Height(16));
		EditorGUILayout.EndHorizontal();
	}



}
