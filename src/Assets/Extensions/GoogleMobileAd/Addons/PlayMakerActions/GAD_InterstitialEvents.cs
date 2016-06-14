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
	public class GAD_InterstitialEvents : FsmStateAction {

		 
		public FsmEvent OnLoadedEvent;
		public FsmEvent OnFailedToLoadEvent;

		public FsmEvent OnOpenEvent;
		public FsmEvent OnCloseEvent;
		public FsmEvent OnLeftApplicationEvent;


		public override void OnEnter() {
			GoogleMobileAd.OnInterstitialLoaded += OnLoaded;
			GoogleMobileAd.OnInterstitialFailedLoading += OnFailedToLoad;

			GoogleMobileAd.OnInterstitialOpened += OnOpen;
			GoogleMobileAd.OnInterstitialClosed += OnClose;
			GoogleMobileAd.OnInterstitialLeftApplication += OnLeftApplication;
		}


		private void OnLoaded() {
			Fsm.Event(OnLoadedEvent);
		}

		private void OnFailedToLoad() {
			Fsm.Event(OnFailedToLoadEvent);
		}


		private void OnOpen() {
			Fsm.Event(OnOpenEvent);
		}

		private void OnClose() {
			Fsm.Event(OnCloseEvent);
		}

		private void OnLeftApplication() {
			Fsm.Event(OnLeftApplicationEvent);
		}
		

		
	}
}
