using UnityEngine;
using System.Collections;

public class CustomInterstisialExample : MonoBehaviour {






	void Start () {
		GoogleMobileAd.Init();

		GoogleMobileAd.OnInterstitialLoaded += HandleOnInterstitialLoaded;
		GoogleMobileAd.OnInterstitialClosed += OnInterstisialsClosed;
		GoogleMobileAd.OnInterstitialOpened += OnInterstisialsOpen;


		//loadin ad:
		GoogleMobileAd.LoadInterstitialAd ();
	}

	void HandleOnInterstitialLoaded () {
		//ad loaded, strting ad
		GoogleMobileAd.ShowInterstitialAd ();
	}


	private void OnInterstisialsOpen() {
		//pausing the game
	}

	private void OnInterstisialsClosed() {
		//un-pausing the game
	}

}
