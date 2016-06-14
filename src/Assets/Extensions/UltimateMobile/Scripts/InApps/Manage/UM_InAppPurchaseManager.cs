//#define ATC_SUPPORT_ENABLED

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#if ATC_SUPPORT_ENABLED
using CodeStage.AntiCheat.ObscuredTypes;
#endif

public class UM_InAppPurchaseManager : SA_Singleton<UM_InAppPurchaseManager> {
	
	
	//actions
	public static event Action<UM_BillingConnectionResult> OnBillingConnectFinishedAction = delegate {};
	public static event Action<UM_PurchaseResult> OnPurchaseFlowFinishedAction = delegate {};
	public static event Action<UM_BaseResult> OnPurchasesRestoreFinishedAction = delegate {};
	
	
	
	private bool _IsInited = false;
	
	
	public const string PREFS_KEY = "UM_InAppPurchaseManager";
	
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	
	void Awake() {
		DontDestroyOnLoad(gameObject);
		
		
		switch(Application.platform) {
			
		case RuntimePlatform.Android:
			//listening for purchase and consume events
			AndroidInAppPurchaseManager.ActionProductPurchased += Android_ActionProductPurchased;
			AndroidInAppPurchaseManager.ActionProductConsumed += Android_OnProductConsumed;
			
			//initilaizing store
			AndroidInAppPurchaseManager.ActionBillingSetupFinished += Android_OnBillingConnected;
			AndroidInAppPurchaseManager.ActionRetrieveProducsFinished += Android_OnRetrieveProductsFinised;
			
			break;
			
		case RuntimePlatform.IPhonePlayer:
			IOSInAppPurchaseManager.OnStoreKitInitComplete += IOS_OnStoreKitInitComplete;
			IOSInAppPurchaseManager.OnTransactionComplete  += IOS_OnTransactionComplete;
			IOSInAppPurchaseManager.OnRestoreComplete += IOS_OnRestoreComplete;
			break;
			
		case RuntimePlatform.WP8Player:
			
			WP8InAppPurchasesManager.OnInitComplete += WP8_OnInitComplete;
			WP8InAppPurchasesManager.OnPurchaseFinished += WP8_OnProductPurchased;
			
			break;
		}
		
	}
	
	
	public void Init() {
		
		if(_IsInited) {
			return;
		}
		
		
		switch(Application.platform) {
			
		case RuntimePlatform.OSXEditor:
		case RuntimePlatform.WindowsEditor:
			Invoke("FakeConnectEvent", 1f);
			break;
			
		case RuntimePlatform.Android:
			foreach(UM_InAppProduct product in UltimateMobileSettings.Instance.InAppProducts) {
				AndroidInAppPurchaseManager.Instance.AddProduct(product.AndroidId);
			}
			AndroidInAppPurchaseManager.instance.LoadStore();
			break;
			
		case RuntimePlatform.IPhonePlayer:
			foreach(UM_InAppProduct product in UltimateMobileSettings.Instance.InAppProducts) {
				IOSInAppPurchaseManager.instance.AddProductId(product.IOSId);
			}
			IOSInAppPurchaseManager.instance.LoadStore();
			break;
			
			
		case RuntimePlatform.WP8Player:
			WP8InAppPurchasesManager.Instance.Init();
			break;
			
			
		}
	}
	
	
	//--------------------------------------
	// Methods
	//--------------------------------------
	
	public void Subscribe(string subscriptionId) {
		
		UM_InAppProduct p = UltimateMobileSettings.Instance.GetProductById(subscriptionId);
		
		
		if(p != null) {
			switch(Application.platform) {
				
			case RuntimePlatform.OSXEditor:
			case RuntimePlatform.WindowsEditor:
				FakePurchaseEvent(p);
				break;
				
				
			case RuntimePlatform.Android:
				AndroidInAppPurchaseManager.instance.Subscribe(p.AndroidId);
				break;
				
			case RuntimePlatform.IPhonePlayer:
				
				IOSInAppPurchaseManager.instance.BuyProduct(p.IOSId);
				break;
				
				
			case RuntimePlatform.WP8Player:
				WP8InAppPurchasesManager.Instance.Purchase(p.WP8Id);
				break;
			}
			
			
		} else {
			SendNoTemplateEvent();
			Debug.LogWarning("UM_InAppPurchaseManager subscription not found: " + subscriptionId);
		}
		
	}	
	
	public void Purchase(string productId) {
		
		UM_InAppProduct p = UltimateMobileSettings.Instance.GetProductById(productId);
		
		
		if(p != null) {
			switch(Application.platform) {
				
			case RuntimePlatform.OSXEditor:
			case RuntimePlatform.WindowsEditor:
				FakePurchaseEvent(p);
				break;
				
				
			case RuntimePlatform.Android:
				AndroidInAppPurchaseManager.instance.Purchase(p.AndroidId);
				break;
				
			case RuntimePlatform.IPhonePlayer:
				
				IOSInAppPurchaseManager.instance.BuyProduct(p.IOSId);
				break;
				
				
			case RuntimePlatform.WP8Player:
				WP8InAppPurchasesManager.Instance.Purchase(p.WP8Id);
				break;
			}
			
			
		} else {
			SendNoTemplateEvent();
			Debug.LogWarning("UM_InAppPurchaseManager product not found: " + productId);
		}
		
	}
	
	public bool IsProductPurchased(string id) {
		return IsProductPurchased(UltimateMobileSettings.Instance.GetProductById(id));
	}
	
	
	public bool IsProductPurchased(UM_InAppProduct product) {
		if(product == null) {
			return false;
		}
		
		switch(Application.platform) {
			
		case RuntimePlatform.OSXEditor:
		case RuntimePlatform.WindowsEditor:
			#if ATC_SUPPORT_ENABLED
			return ObscuredPrefs.HasKey(PREFS_KEY + product.id);
			#else
			return PlayerPrefs.HasKey(PREFS_KEY + product.id);
			#endif
			
		case RuntimePlatform.Android:

			if(AndroidInAppPurchaseManager.instance.IsInventoryLoaded) {
				return AndroidInAppPurchaseManager.Instance.Inventory.IsProductPurchased(product.AndroidId);
			} else {
				#if ATC_SUPPORT_ENABLED
				return ObscuredPrefs.HasKey(PREFS_KEY + product.id);
				#else
				return PlayerPrefs.HasKey(PREFS_KEY + product.id);
				#endif
				
			}
			
		case RuntimePlatform.IPhonePlayer:
			#if ATC_SUPPORT_ENABLED
			return ObscuredPrefs.HasKey(PREFS_KEY + product.id);
			#else
			return PlayerPrefs.HasKey(PREFS_KEY + product.id);
			#endif
			
			
		case RuntimePlatform.WP8Player:
			if(WP8InAppPurchasesManager.Instance.IsInitialized) {
				return product.WP8Template.IsPurchased;
			} else {
				#if ATC_SUPPORT_ENABLED
				return ObscuredPrefs.HasKey(PREFS_KEY + product.id);
				#else
				return PlayerPrefs.HasKey(PREFS_KEY + product.id);
				#endif
				
				
			}
		}
		
		return false;
	}
	
	
	public void DeleteNonConsumablePurchaseRecord(UM_InAppProduct product) {
		#if ATC_SUPPORT_ENABLED
		ObscuredPrefs.DeleteKey(PREFS_KEY + product.id);
		#else
		PlayerPrefs.DeleteKey(PREFS_KEY + product.id);
		#endif
	}
	
	public void RestorePurchases() {
		switch(Application.platform) {
			
			
		case RuntimePlatform.OSXEditor:
		case RuntimePlatform.WindowsEditor:
			foreach(UM_InAppProduct p in InAppProducts) {
				FakePurchaseEvent(p);
			}
			Invoke("FakeRestoreEvent", 1f);
			break;
			
			
		case RuntimePlatform.IPhonePlayer:
			IOSInAppPurchaseManager.instance.RestorePurchases();
			break;
			
		}
	}
	
	
	
	public List<UM_InAppProduct> InAppProducts {
		get {
			return UltimateMobileSettings.Instance.InAppProducts;
		}
	}
	
	public UM_InAppProduct GetProductById(string id) {
		return UltimateMobileSettings.Instance.GetProductById(id);
	}
	
	
	public UM_InAppProduct GetProductByIOSId(string id) {
		return UltimateMobileSettings.Instance.GetProductByIOSId(id);
	}
	
	
	public UM_InAppProduct GetProductByAndroidId(string id) {
		return UltimateMobileSettings.Instance.GetProductByAndroidId(id);
	}
	
	public UM_InAppProduct GetProductByWp8Id(string id) {
		return UltimateMobileSettings.Instance.GetProductByWp8Id(id);
	}
	
	
	//--------------------------------------
	// Get / SET
	//--------------------------------------
	
	public bool IsInited {
		get {
			return _IsInited;
		}
	}
	
	public bool IsPurchasingAvailable {
		get {
			return IsInited;
		}
	}
	
	
	
	//--------------------------------------
	// WP8 Listners
	//--------------------------------------
	
	private void WP8_OnInitComplete(WP8_InAppsInitResult result) {
		
		_IsInited = true;
		
		UM_BillingConnectionResult r =  new UM_BillingConnectionResult();
		r.message = "Inited";
		r.isSuccess = true;
		
		
		foreach(UM_InAppProduct product in UltimateMobileSettings.Instance.InAppProducts) {
			
			
			WP8ProductTemplate tpl =  WP8InAppPurchasesManager.Instance.GetProductById(product.WP8Id);
			if(tpl != null) {
				product.SetTemplate(tpl);
				if(product.WP8Template.IsPurchased && !product.IsConsumable) {
					SaveNonConsumableItemPurchaseInfo(product);
				}
			}
			
		}
		
		OnBillingConnectFinishedAction(r);
	}
	
	private void WP8_OnProductPurchased(WP8PurchseResponce resp) {
		UM_InAppProduct p = UltimateMobileSettings.Instance.GetProductByWp8Id(resp.ProductId);
		if(p != null) {
			UM_PurchaseResult r =  new UM_PurchaseResult();
			r.product = p;
			r.WP8_PurchaseInfo = resp;
			
			SendPurchaseEvent(r);
		} else {
			SendNoTemplateEvent();
		}
	}
	
	//--------------------------------------
	// IOS Listners
	//--------------------------------------
	
	
	private void IOS_OnTransactionComplete (IOSStoreKitResult responce) {
		
		UM_InAppProduct p = UltimateMobileSettings.Instance.GetProductByIOSId(responce.ProductIdentifier);
		if(p != null) {
			UM_PurchaseResult r =  new UM_PurchaseResult();
			r.product = p;
			r.IOS_PurchaseInfo = responce;
			SendPurchaseEvent(r);
		} else {
			SendNoTemplateEvent();
		}
		
		
	}
	
	private void IOS_OnStoreKitInitComplete (ISN_Result res) {
		
		UM_BillingConnectionResult r =  new UM_BillingConnectionResult();
		_IsInited = res.IsSucceeded;
		r.isSuccess = res.IsSucceeded;
		if(res.IsSucceeded) {
			r.message = "Inited";
			
			foreach(UM_InAppProduct product in UltimateMobileSettings.Instance.InAppProducts) {
				
				IOSProductTemplate tpl = IOSInAppPurchaseManager.instance.GetProductById(product.IOSId); 
				if(tpl != null) {
					product.SetTemplate(tpl);
				}
				
			}
			
			OnBillingConnectFinishedAction(r);
		} else {
			
			if(res.Error != null) {
				r.message = res.Error.Description;
			}
			
			OnBillingConnectFinishedAction(r);
		}
		
	}
	
	void IOS_OnRestoreComplete (IOSStoreKitRestoreResult res) {
		Debug.Log("IOS_OnRestoreComplete");
		
		UM_BaseResult result =  new UM_BaseResult();
		result.IsSucceeded = res.IsSucceeded;
		
		OnPurchasesRestoreFinishedAction(result);
	}	
	
	//--------------------------------------
	// Android Listners
	//--------------------------------------
	
	
	
	
	void Android_ActionProductPurchased (BillingResult result) {
		UM_InAppProduct p = UltimateMobileSettings.Instance.GetProductByAndroidId(result.purchase.SKU);
		
		if(p != null) {
			if(p.IsConsumable && result.isSuccess) {
				AndroidInAppPurchaseManager.instance.Consume(result.purchase.SKU);
			} else {
				
				UM_PurchaseResult r =  new UM_PurchaseResult();
				r.isSuccess = result.isSuccess;
				r.product = p;
				r.SetResponceCode(result.response);
				r.Google_PurchaseInfo = result.purchase;
				
				SendPurchaseEvent(r);
			}
		} else {
			SendNoTemplateEvent();
		}
	}	
	
	private void Android_OnProductConsumed(BillingResult result) {
		
		
		UM_InAppProduct p = UltimateMobileSettings.Instance.GetProductByAndroidId(result.purchase.SKU);
		if(p != null) {
			UM_PurchaseResult r =  new UM_PurchaseResult();
			r.isSuccess = result.isSuccess;
			r.product = p;
			r.SetResponceCode(result.response);
			r.Google_PurchaseInfo = result.purchase;
			SendPurchaseEvent(r);
		} else {
			SendNoTemplateEvent();
		}
	}
	
	
	private void Android_OnBillingConnected(BillingResult result) {
		
		
		
		if(result.isSuccess) {
			//Store connection is Successful. Next we loading product and customer purchasing details
			AndroidInAppPurchaseManager.ActionBillingSetupFinished -= Android_OnBillingConnected;
			AndroidInAppPurchaseManager.instance.RetrieveProducDetails();
			
		} else {
			UM_BillingConnectionResult r =  new UM_BillingConnectionResult();
			r.isSuccess = false;
			r.message = result.message;
			OnBillingConnectFinishedAction(r);
		}
		
		
		
		
		
	}
	
	private void Android_OnRetrieveProductsFinised(BillingResult result) {
		
		
		AndroidInAppPurchaseManager.ActionRetrieveProducsFinished -= Android_OnRetrieveProductsFinised;
		
		UM_BillingConnectionResult r =  new UM_BillingConnectionResult();
		r.message = result.message;
		r.isSuccess = result.isSuccess;
		
		_IsInited = r.isSuccess;

		if(r.isSuccess) {
			foreach(UM_InAppProduct product in UltimateMobileSettings.Instance.InAppProducts) {
				GoogleProductTemplate tpl = AndroidInAppPurchaseManager.Instance.Inventory.GetProductDetails(product.AndroidId);
				if(tpl != null) {
					product.SetTemplate(tpl);
					if(product.IsConsumable && AndroidInAppPurchaseManager.Instance.Inventory.IsProductPurchased(product.AndroidId)) {
						AndroidInAppPurchaseManager.instance.Consume(product.AndroidId);
					}

					if(!product.IsConsumable && AndroidInAppPurchaseManager.Instance.Inventory.IsProductPurchased(product.AndroidId)) {
						SaveNonConsumableItemPurchaseInfo(product);
					}
				}
				
			}
		}
		
		
		OnBillingConnectFinishedAction(r);
		
	}
	
	
	//--------------------------------------
	// Private Methods
	//--------------------------------------
	
	private void SendPurchaseEvent(UM_PurchaseResult result) {
		
		
		switch(Application.platform) {
			
			
		case RuntimePlatform.OSXEditor:
		case RuntimePlatform.WindowsEditor:
		case RuntimePlatform.Android:
			break;
		case RuntimePlatform.IPhonePlayer:
			
			switch(result.IOS_PurchaseInfo.State) {
			case InAppPurchaseState.Purchased:
			case InAppPurchaseState.Restored:
				result.isSuccess = true;
				break;
			case InAppPurchaseState.Deferred:
			case InAppPurchaseState.Failed:
				result.isSuccess = false;
				break;
			}
			break;
		case RuntimePlatform.WP8Player:
			result.isSuccess = result.WP8_PurchaseInfo.IsSuccses;
			break;
		}
		
		
		if(!result.product.IsConsumable && result.isSuccess) {
			Debug.Log("UM: Purchase saved to PlayerPrefs");
			SaveNonConsumableItemPurchaseInfo(result.product);
		}
		
		OnPurchaseFlowFinishedAction(result);
	}
	
	private void SaveNonConsumableItemPurchaseInfo(UM_InAppProduct product) {
		#if ATC_SUPPORT_ENABLED
		ObscuredPrefs.SetInt(PREFS_KEY + product.id, 1);
		#else
		PlayerPrefs.SetInt(PREFS_KEY + product.id, 1);
		#endif
	}
	
	
	private void SendNoTemplateEvent() {
		Debug.LogWarning("UM: Product tamplate not found");
		UM_PurchaseResult r =  new UM_PurchaseResult();
		OnPurchaseFlowFinishedAction(r);
	}
	
	
	//--------------------------------------
	// Editor Fake events
	//--------------------------------------
	
	private void FakeRestoreEvent() {
		UM_BaseResult result =  new UM_BaseResult();
		result.IsSucceeded = true;
		OnPurchasesRestoreFinishedAction(result);
	}
	
	
	private void FakeConnectEvent() {
		_IsInited = true;
		
		UM_BillingConnectionResult r =  new UM_BillingConnectionResult();
		r.message = "Inited";
		r.isSuccess = true;
		OnBillingConnectFinishedAction(r);
	}
	
	
	private void FakePurchaseEvent(UM_InAppProduct product) {
		UM_PurchaseResult r =  new UM_PurchaseResult();
		r.isSuccess = true;
		r.product = product;
		SendPurchaseEvent(r);
	}
	
}
