////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WPN_PopUpExamples : WPNFeaturePreview {

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------

	void OnGUI() {

		UpdateToStartPos();
		
		GUI.Label(new Rect(StartX, StartY, Screen.width, 40), "Native Pop Ups", style);
		StartY+= YLableStep;
		
		
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Rate PopUp with events")) {
			WP8RateUsPopUp rate = WP8RateUsPopUp.Create("Like this game?", "Please rate to support future updates!");
			rate.OnComplete += onRatePopUpClose;

		}


		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Dialog PopUp")) {
			WP8Dialog dialog = WP8Dialog.Create("Dialog Titile", "Dialog message");
			dialog.OnComplete += onDialogClose;

		}
		
		
		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Message PopUp")) {
			WP8Message msg = WP8Message.Create("Message Titile", "Message message");
			msg.OnComplete += onMessageClose;
		}

		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Show Preloader")) {
			WP8NativeUtils.ShowPreloader();
			Invoke("HidePreloader", 2f);
		}


	}
	

	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------

	private void HidePreloader() {
		WP8NativeUtils.HidePreloader();
	}

		
	private void onRatePopUpClose(WP8DialogResult res)
    {
#if UNITY_WP8 
		WP8PopUps.PopUp.ShowMessageWindow("Result", res.ToString() + " button pressed");
#endif
    }
	
	private void onDialogClose(WP8DialogResult res) {
#if UNITY_WP8 
		WP8PopUps.PopUp.ShowMessageWindow("Result", res.ToString() + " button pressed");
#endif
    }
	
	private void onMessageClose(WP8DialogResult res) {
#if UNITY_WP8 
		WP8PopUps.PopUp.ShowMessageWindow("Result", "Message Closed");
#endif
	}
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------


}
