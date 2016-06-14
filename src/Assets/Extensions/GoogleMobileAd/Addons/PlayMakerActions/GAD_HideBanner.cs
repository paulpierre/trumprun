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
	public class GAD_HideBanner : FsmStateAction {

		public FsmInt bannerId;

		public override void OnEnter() {
			GoogleMobileAdBanner banner =  GoogleMobileAd.GetBanner(bannerId.Value);
			if(banner != null) {
				banner.Hide();
			}

			Finish();
		}

	}
}
