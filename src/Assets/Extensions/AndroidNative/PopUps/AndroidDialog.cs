////////////////////////////////////////////////////////////////////////////////
//  
// @module Android Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AndroidDialog : BaseAndroidPopup {
	

	public string yes;
	public string no;


	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	
	public static AndroidDialog Create(string title, string message) {
		return Create(title, message, "Yes", "No");
	}
		
	public static AndroidDialog Create(string title, string message, string yes, string no) {
		AndroidDialog dialog;
		dialog  = new GameObject("AndroidPopUp").AddComponent<AndroidDialog>();
		dialog.title = title;
		dialog.message = message;
		dialog.yes = yes;
		dialog.no = no;
		dialog.init();
		
		return dialog;
	}
	
	
	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	public void init() {
		AN_PoupsProxy.showDialog(title, message, yes, no);
	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	
	public void onPopUpCallBack(string buttonIndex) {
		int index = System.Convert.ToInt16(buttonIndex);

		
		switch(index) {
			case 0: 
				DispatchAction(AndroidDialogResult.YES);
				break;
			case 1: 
				DispatchAction(AndroidDialogResult.NO);
				break;
		}
		
		Destroy(gameObject);
	}
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------


}
