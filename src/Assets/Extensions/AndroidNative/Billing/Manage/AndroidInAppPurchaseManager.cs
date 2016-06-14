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

public class AndroidInAppPurchaseManager : SA_Singleton<AndroidInAppPurchaseManager> {
	

	//Actions
	public static event Action<BillingResult>  ActionProductPurchased   = delegate {};
	public static event Action<BillingResult>  ActionProductConsumed    = delegate {};
	
	public static event Action<BillingResult>  ActionBillingSetupFinished   = delegate {};
	public static event Action<BillingResult>  ActionRetrieveProducsFinished = delegate {};

	private string _processedSKU;
	private AndroidInventory _inventory;


	private bool _IsConnectingToServiceInProcess 	= false;
	private bool _IsProductRetrievingInProcess 		= false;

	private bool _IsConnected = false;
	private bool _IsInventoryLoaded = false;


	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	void Awake() {
		DontDestroyOnLoad(gameObject);
		_inventory = new AndroidInventory ();
	}


	//--------------------------------------
	// PUBLIC METHODS
	//--------------------------------------


	//Fill your products befor loading store
	[System.Obsolete("addProduct is deprectaed, plase use AddProduct instead")]
	public void addProduct(string SKU) {
		AddProduct(SKU);
	}

	public void AddProduct(string SKU) {
		GoogleProductTemplate template = new GoogleProductTemplate(){SKU = SKU};
		AddProduct(template);
	}

	public void AddProduct(GoogleProductTemplate template) {

		bool IsPordcutAlreadyInList = false;
		int replaceIndex = 0;
		foreach(GoogleProductTemplate p in _inventory.Products) {
			if(p.SKU.Equals(template.SKU)) {
				IsPordcutAlreadyInList = true;
				replaceIndex = _inventory.Products.IndexOf(p);
				break;
			}
		}
		
		if(IsPordcutAlreadyInList) {
			_inventory.Products[replaceIndex] = template;
		} else {
			_inventory.Products.Add(template);
		}
	}

	[System.Obsolete("retrieveProducDetails is deprectaed, plase use RetrieveProducDetails instead")]
	public void retrieveProducDetails() {
		RetrieveProducDetails ();
	}

	public void RetrieveProducDetails() {
		_IsProductRetrievingInProcess = true;
		AN_BillingProxy.RetrieveProducDetails();
	}


	[System.Obsolete("purchase is deprectaed, plase use Purchase instead")]
	public void purchase(string SKU) {
		Purchase(SKU);
	}

	[System.Obsolete("purchase is deprectaed, plase use Purchase instead")]
	public void purchase(string SKU, string DeveloperPayload) {
		Purchase (SKU, DeveloperPayload);
	}

	public void Purchase(string SKU) {
		Purchase(SKU, "");
	}

	public void Purchase(string SKU, string DeveloperPayload) {
		_processedSKU = SKU;
		AN_SoomlaGrow.PurchaseStarted(SKU);
		AN_BillingProxy.Purchase (SKU, DeveloperPayload);

	}

	[System.Obsolete("subscribe is deprectaed, plase use Subscribe instead")]
	public void subscribe(string SKU) {
		Subscribe (SKU);
	}

	[System.Obsolete("subscribe is deprectaed, plase use Subscribe instead")]
	public void subscribe(string SKU, string DeveloperPayload) {
		Subscribe (SKU, DeveloperPayload);
	}

	public void Subscribe(string SKU) {
		Subscribe(SKU, "");
	}

	public void Subscribe(string SKU, string DeveloperPayload) {
		_processedSKU = SKU;
		AN_SoomlaGrow.PurchaseStarted(SKU);
		AN_BillingProxy.Subscribe (SKU, DeveloperPayload);
	}

	[System.Obsolete("consume is deprectaed, plase use Consume instead")]
	public void consume(string SKU) {
		Consume (SKU);
	}

	public void Consume(string SKU) {
		_processedSKU = SKU;
		AN_BillingProxy.Consume (SKU);
	}

	[System.Obsolete("loadStore is deprectaed, plase use LoadStore instead")]
	public void loadStore() {
		LoadStore ();
	}

	[System.Obsolete("loadStore is deprectaed, plase use LoadStore instead")]
	public void loadStore(string base64EncodedPublicKey) {

		LoadStore (base64EncodedPublicKey);
	}

	public void LoadStore() {
		if(AndroidNativeSettings.Instance.IsBase64KeyWasReplaced) {
			LoadStore(AndroidNativeSettings.Instance.base64EncodedPublicKey);
			_IsConnectingToServiceInProcess = true;
		} else {
			Debug.LogError("Replace base64EncodedPublicKey in Androdi Native Setting menu");
		}
	}

	public void LoadStore(string base64EncodedPublicKey) {
		
		foreach(GoogleProductTemplate pid in AndroidNativeSettings.Instance.InAppProducts) {
			AddProduct(pid.SKU);
		}
		
		string ids = "";
		int len = AndroidNativeSettings.Instance.InAppProducts.Count;
		for(int i = 0; i < len; i++) {
			if(i != 0) {
				ids += ",";
			}
			
			ids += AndroidNativeSettings.Instance.InAppProducts[i].SKU;
		}
		
		AN_BillingProxy.Connect (ids, base64EncodedPublicKey);

	}



	//--------------------------------------
	// GET / SET
	//--------------------------------------

	[System.Obsolete("inventory is deprectaed, plase use Inventory instead")]
	public AndroidInventory inventory {
		get {
			return _inventory;
		}
	}

	public AndroidInventory Inventory {
		get {
			return _inventory;
		}
	}

	public bool IsConnectingToServiceInProcess {
		get {
			return _IsConnectingToServiceInProcess;
		}
	}

	public bool IsProductRetrievingInProcess {
		get {
			return _IsProductRetrievingInProcess;
		}
	}

	public bool IsConnected {
		get {
			return _IsConnected;
		}
	}

	public bool IsInventoryLoaded {
		get {
			return _IsInventoryLoaded;
		}
	}
	

	//--------------------------------------
	// EVENTS
	//--------------------------------------



	public void OnPurchaseFinishedCallback(string data) {
		Debug.Log(data);
		string[] storeData;
		storeData = data.Split(AndroidNative.DATA_SPLITTER [0]);

		int resp = System.Convert.ToInt32 (storeData[0]);
		GooglePurchaseTemplate purchase = new GooglePurchaseTemplate ();


		if(resp == BillingResponseCodes.BILLING_RESPONSE_RESULT_OK) {

			purchase.SKU 						= storeData[2];
			purchase.packageName 				= storeData[3];
			purchase.developerPayload 			= storeData[4];
			purchase.orderId 	       			= storeData[5];
			purchase.SetState(storeData[6]);
			purchase.token 	        			= storeData[7];
			purchase.signature 	        		= storeData[8];
			purchase.time						= System.Convert.ToInt64(storeData[9]);
			purchase.originalJson 				= storeData[10];

			if(_inventory != null) {
				_inventory.addPurchase (purchase);
			}

		} else {
			purchase.SKU 						= _processedSKU;
		}


		//Soomla Analytics
		if(resp == BillingResponseCodes.BILLING_RESPONSE_RESULT_OK) {
			GoogleProductTemplate tpl = Inventory.GetProductDetails(purchase.SKU);
			if(tpl != null) {
				AN_SoomlaGrow.PurchaseFinished(tpl.SKU, tpl.PriceAmountMicros, tpl.PriceCurrencyCode);
			} else {
				AN_SoomlaGrow.PurchaseFinished(purchase.SKU, 0, "USD");
			}
		} else {


			if(resp == BillingResponseCodes.BILLINGHELPERR_USER_CANCELLED) {
				AN_SoomlaGrow.PurchaseCanceled(purchase.SKU);
			} else {
				AN_SoomlaGrow.PurchaseError();
			}
		}

		BillingResult result = new BillingResult (resp, storeData [1], purchase);


		ActionProductPurchased(result);
	}


	public void OnConsumeFinishedCallBack(string data) {
		string[] storeData;
		storeData = data.Split(AndroidNative.DATA_SPLITTER [0]);

		int resp = System.Convert.ToInt32 (storeData[0]);
		GooglePurchaseTemplate purchase = null;


		if(resp == BillingResponseCodes.BILLING_RESPONSE_RESULT_OK) {
			purchase = new GooglePurchaseTemplate ();
			purchase.SKU 				= storeData[2];
			purchase.packageName 		= storeData[3];
			purchase.developerPayload 	= storeData[4];
			purchase.orderId 	        = storeData[5];
			purchase.SetState(storeData[6]);
			purchase.token 	        		= storeData[7];
			purchase.signature 	        	= storeData[8];
			purchase.time					= System.Convert.ToInt64(storeData[9]);
			purchase.originalJson 	        = storeData[10];

			if(_inventory != null) {
				_inventory.removePurchase (purchase);
			}

		}

		BillingResult result = new BillingResult (resp, storeData [1], purchase);

		ActionProductConsumed(result);
	}



	public void OnBillingSetupFinishedCallback(string data) {
		string[] storeData;
		storeData = data.Split(AndroidNative.DATA_SPLITTER [0]);

		int resp = System.Convert.ToInt32 (storeData[0]);


		_IsConnected = true;
		_IsConnectingToServiceInProcess = false;
		BillingResult result = new BillingResult (resp, storeData [1]);

		AN_SoomlaGrow.SetPurhsesSupportedState(result.isSuccess);

		ActionBillingSetupFinished(result);
	}


	public void OnQueryInventoryFinishedCallBack(string data) {
		string[] storeData;
		storeData = data.Split(AndroidNative.DATA_SPLITTER [0]);

		int resp = System.Convert.ToInt32 (storeData[0]);

		BillingResult result = new BillingResult (resp, storeData [1]);

		_IsInventoryLoaded = true;
		_IsProductRetrievingInProcess = false;

		ActionRetrieveProducsFinished(result);
	}



	public void OnPurchasesRecive(string data) {
		if(data.Equals(string.Empty)) {
			Debug.Log("InAppPurchaseManager, no purchases avaiable");
			return;
		}

		string[] storeData;
		storeData = data.Split(AndroidNative.DATA_SPLITTER [0]);

		for(int i = 0; i < storeData.Length; i+=9) {
			GooglePurchaseTemplate tpl =  new GooglePurchaseTemplate();
			tpl.SKU 				= storeData[i];
			tpl.packageName 		= storeData[i + 1];
			tpl.developerPayload 	= storeData[i + 2];
			tpl.orderId 	        = storeData[i + 3];
			tpl.SetState(storeData[i + 4]);
			tpl.token 	        	= storeData[i + 5];
			tpl.signature 	        = storeData[i + 6];
			tpl.time 	        	= System.Convert.ToInt64(storeData[i + 7]); 
			tpl.originalJson 	    = storeData[i + 8];

			_inventory.addPurchase (tpl);
		}

		Debug.Log("InAppPurchaseManager, total purchases loaded: " + _inventory.Purchases.Count);

	}


	public void OnProducttDetailsRecive(string data) {
		if(data.Equals(string.Empty)) {
			Debug.Log("InAppPurchaseManager, no products avaiable");
			return;
		}

		string[] storeData;
		storeData = data.Split(AndroidNative.DATA_SPLITTER [0]);


		for(int i = 0; i < storeData.Length; i+=7) {
			GoogleProductTemplate tpl =  _inventory.GetProductDetails(storeData[i]);
			if(tpl == null) {
				tpl =  new GoogleProductTemplate();
				tpl.SKU = storeData[i];
				_inventory.Products.Add(tpl);
			}

			tpl.LocalizedPrice 		  		= storeData[i + 1];
			tpl.Title 	      				= storeData[i + 2];
			tpl.Description   				= storeData[i + 3];
			tpl.PriceAmountMicros 	      	= System.Convert.ToInt64(storeData[i + 4]);
			tpl.PriceCurrencyCode   		= storeData[i + 5];
			tpl.OriginalJson   				= storeData[i + 6];


			Debug.Log("Prodcut originalJson: " + tpl.OriginalJson);
		}

		Debug.Log("InAppPurchaseManager, total products loaded: " + _inventory.Products.Count);
	}


}
