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
	public class GAD_StartInterstisialAd : FsmStateAction {

		[Tooltip("Event fired when Ad is started")]
		public FsmEvent successEvent;
		
		[Tooltip("Event fired when Ad is failed to load")]
		public FsmEvent failEvent;
		
		public override void OnEnter() {
			GoogleMobileAd.StartInterstitialAd();
			GoogleMobileAd.OnInterstitialLoaded += OnReady;
			GoogleMobileAd.OnInterstitialFailedLoading += OnFail;


			#if UNITY_EDITOR
				OnReady();
			#endif
		}
		
		
		private void OnReady() {
			
			GoogleMobileAd.OnInterstitialLoaded -= OnReady;
			GoogleMobileAd.OnInterstitialFailedLoading -= OnFail;
			
			Fsm.Event(successEvent);
			Finish();
		}
		
		private void OnFail() {
			
			GoogleMobileAd.OnInterstitialLoaded -= OnReady;
			GoogleMobileAd.OnInterstitialFailedLoading -= OnFail;
			
			Fsm.Event(failEvent);
			Finish();
		}
		
	}
}
