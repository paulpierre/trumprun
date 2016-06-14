using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;

public class GoogleAnalyticsEdiotrCore  {


	[PostProcessBuild]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
		if(GoogleAnalyticsSettings.Instance.IsTestingModeEnabled) {
			Debug.LogWarning("WARNING: Google Analytics Test Mode Enabled!");
		}
	}
}
