using UnityEngine;
using System.Collections;

public static class SA_EditorTesting  {

	public static bool IsInsideEditor {
		get {
			bool IsInsideEditor = false;
			if(Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor) {
				IsInsideEditor = true;
			}

			return IsInsideEditor;
		}
	}

}
