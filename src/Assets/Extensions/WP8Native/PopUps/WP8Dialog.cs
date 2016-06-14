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

public class WP8Dialog : WP8PopupBase {


	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	
	public static WP8Dialog Create(string title, string message) {
		WP8Dialog dialog;
		dialog  = new GameObject("WP8Dialog").AddComponent<WP8Dialog>();
		dialog.title = title;
		dialog.message = message;
		dialog.init();
		
		return dialog;
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
		OnComplete(WP8DialogResult.YES);
		Destroy(gameObject);
	}
	
	public void OnCancelDel() {
		OnComplete(WP8DialogResult.NO);
		Destroy(gameObject);
	}
	
	
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------


}
