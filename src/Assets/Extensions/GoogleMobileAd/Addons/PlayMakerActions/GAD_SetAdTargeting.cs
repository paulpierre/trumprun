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
	public class GAD_SetAdTargeting : FsmStateAction {


		public FsmString[] keywords;
		public bool tagForChildDirectedTreatment = false;
		public GoogleGender gender = GoogleGender.Unknown;
		public bool setBirthday = false;
		public FsmInt day;
		public AndroidMonth month;
		public FsmInt year;


		public override void OnEnter() {

			foreach(FsmString k in keywords) {
				GoogleMobileAd.AddKeyword(k.Value);
			}
			GoogleMobileAd.SetGender(gender);
			GoogleMobileAd.TagForChildDirectedTreatment(tagForChildDirectedTreatment);
			if(setBirthday) {
				GoogleMobileAd.SetBirthday(year.Value, month, day.Value);
			}

			Finish();
		}



		
	}
}
