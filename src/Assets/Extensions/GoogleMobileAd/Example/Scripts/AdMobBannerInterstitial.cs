////////////////////////////////////////////////////////////////////////////////
//  
// @module Google Ads Unity SDK 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System.Collections;
using System.Collections.Generic;


#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2 
#else
using UnityEngine.SceneManagement;
#endif


//Attach the script to the empty gameobject on your sceneS
public class AdMobBannerInterstitial : MonoBehaviour {

	


	// --------------------------------------
	// Unity Events
	// --------------------------------------
	
	void Awake() {


		if(!GoogleMobileAd.IsInited) {
			GoogleMobileAd.Init();
		}



	}

	void Start() {
//		ShowBanner();
	}




	// --------------------------------------
	// PUBLIC METHODS
	// --------------------------------------

	public void ShowBanner() {
		if (!PlayerPrefs.HasKey ("noAds")) {
			GoogleMobileAd.StartInterstitialAd ();
		}
	}



	// --------------------------------------
	// GET / SET
	// --------------------------------------



	public string sceneBannerId {
		get {
			#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
			return Application.loadedLevelName + "_" + this.gameObject.name;
			#else
			return SceneManager.GetActiveScene().name + "_" + this.gameObject.name;
			#endif
		}
	}

	
}
