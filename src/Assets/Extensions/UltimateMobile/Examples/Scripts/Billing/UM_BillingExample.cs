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

public class UM_BillingExample : BaseIOSFeaturePreview {

	public const string CONSUMABLE_PRODUCT_ID 		= "coins_pack";// GPaymnetManagerExample.ANDROID_TEST_PURCHASED;	//"ConsumableExample";
	public const string NON_CONSUMABLE_PRODUCT_ID 	=	"coins_bonus";

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------



	

	void Awake() {
		UM_ExampleStatusBar.text = "Unified billing exmple scene loaded";

		UM_InAppPurchaseManager.OnPurchaseFlowFinishedAction += OnPurchaseFlowFinishedAction;
		UM_InAppPurchaseManager.OnBillingConnectFinishedAction += OnConnectFinished;
	}

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	void OnGUI() {
		UpdateToStartPos();
		
		GUI.Label(new Rect(StartX, StartY, Screen.width, 40), "In-App Purchases", style);

		StartY+= YLableStep;


		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Init")) {

			//subscribign on intit fisigh action
			UM_InAppPurchaseManager.OnBillingConnectFinishedAction += OnBillingConnectFinishedAction;
			UM_InAppPurchaseManager.instance.Init();
			UM_ExampleStatusBar.text = "Initializing billing...";
		}


		if(UM_InAppPurchaseManager.instance.IsInited) {
			GUI.enabled = true;
		} else  {
			GUI.enabled = false;
		}


		StartX = XStartPos;
		StartY+= YButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Buy Consumable Item")) {
			UM_InAppPurchaseManager.instance.Purchase(CONSUMABLE_PRODUCT_ID);

			UM_ExampleStatusBar.text = "Start purchsing " + CONSUMABLE_PRODUCT_ID + " product";
		}

		StartX += XButtonStep;


		bool e = GUI.enabled;
		string msg = "";
		if(UM_InAppPurchaseManager.instance.IsProductPurchased(NON_CONSUMABLE_PRODUCT_ID)) {
			msg = "Already purchased";
			GUI.enabled = false;
		} else {
			msg = "Not yet purchased";
		}
		
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Buy Non-Consumable Item \n" + msg)) {
			UM_ExampleStatusBar.text = "Start purchsing " + NON_CONSUMABLE_PRODUCT_ID + " product";

			UM_InAppPurchaseManager.instance.Purchase(NON_CONSUMABLE_PRODUCT_ID);
		}

		GUI.enabled = e;

		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Restore Purshases \n For IOS Only")) {
			IOSInAppPurchaseManager.instance.RestorePurchases();
		}



	}
	
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------

	private void OnConnectFinished(UM_BillingConnectionResult result) {

		if(result.isSuccess) {
			UM_ExampleStatusBar.text = "Billing init Success";
		} else  {
			UM_ExampleStatusBar.text = "Billing init Failed";
		}
	}

	private void OnPurchaseFlowFinishedAction (UM_PurchaseResult result) {
		UM_InAppPurchaseManager.OnPurchaseFlowFinishedAction -= OnPurchaseFlowFinishedAction;
		if(result.isSuccess) {
			UM_ExampleStatusBar.text = "Product " + result.product.id + " purchase Success";
		} else  {
			UM_ExampleStatusBar.text = "Product " + result.product.id + " purchase Failed";
		}
	}

	private void OnBillingConnectFinishedAction (UM_BillingConnectionResult result) {
		UM_InAppPurchaseManager.OnBillingConnectFinishedAction -= OnBillingConnectFinishedAction;
		if(result.isSuccess) {
			Debug.Log("Connected");
		} else {
			Debug.Log("Failed to connect");
		}
	}


	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------



}
