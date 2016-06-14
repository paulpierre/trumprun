using UnityEngine;
using System.Collections;

public class WP8NativeUtils  {


	public static void ShowPreloader() {
#if UNITY_WP8 
		WP8PopUps.PopUp.ShowPreLoader(100);
#endif
	}

	public static void HidePreloader() {
#if UNITY_WP8 
		WP8PopUps.PopUp.HidePreLoader();
#endif
	}

}
