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
	public class GAD_CreateBanner : FsmStateAction {


		public FsmInt bannerId;
		public bool ShowBannerOnLoad = true;
		public TextAnchor anchor;
		public GADBannerSize size;

		public bool CreateWithCoords = false;
		public FsmInt XCoord;
		public FsmInt YCoord;

		[Tooltip("Event fired when Ad is started")]
		public FsmEvent successEvent;
		
		[Tooltip("Event fired when Ad is failed to load")]
		public FsmEvent failEvent;


		private GoogleMobileAdBanner _banner ;

		public override void OnEnter() {

			if(CreateWithCoords) {
				_banner = GoogleMobileAd.CreateAdBanner(XCoord.Value, YCoord.Value, size);
			} else {
				_banner = GoogleMobileAd.CreateAdBanner(anchor, size);
			}


			_banner.ShowOnLoad = ShowBannerOnLoad;
			bannerId.Value = _banner.id;

			_banner.OnLoadedAction += OnReady;
			_banner.OnFailedLoadingAction += OnFail;

	
			#if UNITY_EDITOR
				OnReady(null);
			#endif

		}


		private void OnReady(GoogleMobileAdBanner banner) {

			_banner.OnLoadedAction -= OnReady;
			_banner.OnFailedLoadingAction -= OnFail;


			Fsm.Event(successEvent);
			Finish();
		}

		private void OnFail(GoogleMobileAdBanner banner) {

			_banner.OnLoadedAction -= OnReady;
			_banner.OnFailedLoadingAction -= OnFail;


			Fsm.Event(failEvent);
			Finish();
		}
		
	}
}
