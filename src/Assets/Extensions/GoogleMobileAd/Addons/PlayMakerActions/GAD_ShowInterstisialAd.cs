////////////////////////////////////////////////////////////////////////////////
//  
// @module Android Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;



namespace HutongGames.PlayMaker.Actions {
	
	[ActionCategory("Google Mobile Ad")]
	public class GAD_ShowInterstisialAd : FsmStateAction {

		[Tooltip("Event fired when Ad is started")]
		public FsmEvent successEvent;
		
		[Tooltip("Event fired when Ad is failed to load")]
		public FsmEvent failEvent;
		
		public override void OnEnter() {
			if (!PlayerPrefs.HasKey ("noAds")) {
				if (!PlayerPrefs.HasKey ("deathcount")) {
					PlayerPrefs.SetInt ("deathcount", 1);

				}
					int deathcount = PlayerPrefs.GetInt ("deathcount");

					if (deathcount % 2 == 0) {
						GoogleMobileAd.ShowInterstitialAd ();
					}
				PlayerPrefs.SetInt ("deathcount", deathcount + 1);

			}
			Finish();
		}

	}
}
