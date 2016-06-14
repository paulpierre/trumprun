////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WP8RateUsPopUp : WP8PopupBase {
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	public static WP8RateUsPopUp Create(string title, string message) {
		WP8RateUsPopUp popup = new GameObject("WP8RateUsPopUp").AddComponent<WP8RateUsPopUp>();
		popup.title = title;
		popup.message = message;
		
		popup.init();
		
		return popup;
	}
	
	
	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	
	public void init() {
		#if UNITY_WP8
		WP8PopUps.PopUp.ShowMessageWindow_OK_Cancel(message, title, OnOkDel, OnCancelDel);
		#endif
	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	
	
	public void OnOkDel() {
#if UNITY_WP8 
		WP8PopUps.PopUp.ShowRateWindow();

		OnComplete(WP8DialogResult.RATED);
		Destroy(gameObject);
#endif
	}
	
	public void OnCancelDel() {
		OnComplete(WP8DialogResult.DECLINED);
		Destroy(gameObject);
	}
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------


}
