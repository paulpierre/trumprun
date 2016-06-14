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
	public class GAD_BannerEvents : FsmStateAction {

		public FsmInt bannerId;
		 
		public FsmEvent OnLoadedEvent;
		public FsmEvent OnFailedToLoadEvent;

		public FsmEvent OnOpenEvent;
		public FsmEvent OnCloseEvent;
		public FsmEvent OnLeftApplicationEvent;

		
		public override void OnEnter() {
			GoogleMobileAdBanner banner = GoogleMobileAd.GetBanner(bannerId.Value);
			if(banner == null) {
				return;
			}

			banner.OnLoadedAction += OnLoaded;
			banner.OnFailedLoadingAction += OnFailedToLoad;
	
			banner.OnOpenedAction += OnOpen;
			banner.OnClosedAction += OnClose;
			banner.OnLeftApplicationAction += OnLeftApplication;

		}


		private void OnLoaded(GoogleMobileAdBanner banner) {
			Fsm.Event(OnLoadedEvent);
		}

		private void OnFailedToLoad(GoogleMobileAdBanner banner) {
			Fsm.Event(OnFailedToLoadEvent);
		}


		private void OnOpen(GoogleMobileAdBanner banner) {
			Fsm.Event(OnOpenEvent);
		}

		private void OnClose(GoogleMobileAdBanner banner) {
			Fsm.Event(OnCloseEvent);
		}

		private void OnLeftApplication(GoogleMobileAdBanner banner) {
			Fsm.Event(OnLeftApplicationEvent);
		}
		

		
	}
}
