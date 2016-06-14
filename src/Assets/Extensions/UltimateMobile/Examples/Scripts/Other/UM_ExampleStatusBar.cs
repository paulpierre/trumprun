using UnityEngine;
using System.Collections;

public class UM_ExampleStatusBar : SA_Singleton<UM_ExampleStatusBar> {

	public string _text;


	private float h = 50;
	private GUIStyle style;


	void Awake() {
		style = new GUIStyle();
		style.fontSize = 18;
		style.fontStyle = FontStyle.Italic;
		style.alignment = TextAnchor.MiddleRight;
		style.normal.textColor = Color.white;

		DontDestroyOnLoad(gameObject);

	}

	void OnGUI() {


		GUI.Label(new Rect(0, Screen.height - h, Screen.width - 30, h), _text, style);
	}

	public static string text {
		get {
			return instance._text;
		}

		set {
			instance._text = value;
		}
	}
}
