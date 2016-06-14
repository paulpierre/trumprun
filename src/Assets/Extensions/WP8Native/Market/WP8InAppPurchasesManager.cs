using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WP8InAppPurchasesManager {
	
	private static WP8InAppPurchasesManager _instance = null;

	private bool _IsInitialized = false;
	
	public static event Action<WP8_InAppsInitResult> 	OnInitComplete   	= delegate {};
	public static event Action<WP8PurchseResponce> 		OnPurchaseFinished 	= delegate {};
	public static event Action<WP8ConsumeResponse>		OnConsumeFinished	= delegate {};

	public const string SPLITTER1 = "|";
	public const string SPLITTER2 = "|%|";

	public const int RESULT_OK = 0;
	public const int RESULT_ERROR = 1;

	private List<WP8ProductTemplate> _products =  new List<WP8ProductTemplate>();

#if UNITY_WP8
	private List<WP8PurchseResponce> _defferedPurchases = new List<WP8PurchseResponce>();
#endif

	[System.Obsolete("This property is obsolete. Use 'Instance' property instead")]
	public static WP8InAppPurchasesManager instance {
		get {			
			return Instance;
		}
	}

	public static WP8InAppPurchasesManager Instance {
		get {
			if(_instance == null) {
				_instance =  new WP8InAppPurchasesManager();
			}
			
			return _instance;
		}
	}

	[System.Obsolete("This method is obsolete. Use 'Init' method instead")]
	public void init() {
		Init ();
	}

	public void Init() {
		#if UNITY_WP8
		WP8Native.InAppPurchases.BillingInit(InitCallback, DefferedPurchaseCallback);
		#elif UNITY_WSA
		WSANative.InAppPurchases.BillingInit(InitCallback);
		#endif
	}

	[System.Obsolete("This method is obsolete. Use 'Purchase' method instead")]
	public void purchase(string productId) {
		Purchase(productId);
	}

	public void Purchase(string productID) {
		#if UNITY_WP8
		WP8Native.InAppPurchases.Purchase(productID, PurchaseCallback);
		#elif UNITY_WSA
		WSANative.InAppPurchases.Purchase(productID, PurchaseCallback);
		#endif
	}

#if UNITY_WSA
	public void Consume(string productID) {
		foreach (WP8ProductTemplate product in _products) {
			if (product.ProductId.Equals(productID)) {
				WSANative.InAppPurchases.Consume(product.ProductId, product.TransactionID, ConsumeCallback);
				return;
			}
		}
		Debug.Log(string.Format("[WP8InAppPurchasesManager:Consume] Product {0} NOT found", productID));
	}
#endif

	[System.Obsolete("This property is obsolete. Use 'Products' property instead")]
	public List<WP8ProductTemplate> products  {
		get {
			return Products;
		}
	}

	public List<WP8ProductTemplate> Products  {
		get {
			return _products;
		}
	}

	public bool IsInitialized {
		get {
			return _IsInitialized;
		}
	}

	public WP8ProductTemplate GetProductById(string id) {
		foreach(WP8ProductTemplate p in _products) {
			if(p.ProductId.Equals(id)) {
				return p;
			}
		}

		return null;
	} 
		
	private void InitCallback(string res) {

		string[] data = res.Split(new string[] { SPLITTER2 }, StringSplitOptions.None);
		int status = Int32.Parse(data[0]);		
		WP8_InAppsInitResult result = new WP8_InAppsInitResult(status);
		if (result.IsSucceeded)
		{
			for (int i = 1; i < data.Length; i++)
			{
				System.Diagnostics.Debug.WriteLine("Product RAW Data: {0}", data[i]);
				
				string[] productInfo = data[i].Split(new string[] { SPLITTER1 }, StringSplitOptions.None);
				
				WP8ProductTemplate product = new WP8ProductTemplate();
				product.ImgURI = productInfo[0];
				product.Name = productInfo[1];
				product.ProductId = productInfo[2];
				product.Price = productInfo[3];
				product.Type = (WP8PurchaseProductType)Enum.Parse(typeof(WP8PurchaseProductType), productInfo[4]);
				product.Description = productInfo[5];
				product.IsPurchased = bool.Parse(productInfo[6]);
				product.TransactionID = productInfo[7];
				
				_products.Add(product);
			}

			_IsInitialized = true;
		}

		OnInitComplete(result);

#if UNITY_WP8
		//Deffered purchases event dispatch
		foreach (WP8PurchseResponce defferedPurchase in _defferedPurchases) {
			OnPurchaseFinished(defferedPurchase);
		}
		_defferedPurchases.Clear();
#endif
	}
	
	private void PurchaseCallback(string result) {
		string[] data = result.Split(new string[] { SPLITTER1 }, StringSplitOptions.None);
		int code = Int32.Parse(data[0]);
		WP8PurchseResponce response = null;

		if (code == RESULT_OK) {
			response = new WP8PurchseResponce((WP8PurchaseCodes)Enum.Parse(typeof(WP8PurchaseCodes), data[1]), data[2]);
			response.TransactionId = data[3];
		} else {
			response = new WP8PurchseResponce((WP8PurchaseCodes)Enum.Parse(typeof(WP8PurchaseCodes), data[2]), data[1]);
		}

		OnPurchaseFinished(response);
	}

	private void ConsumeCallback(string result) {
		string[] data = result.Split(new string[] { SPLITTER1 }, StringSplitOptions.None);
		int code = Int32.Parse(data[0]);
		WP8ConsumeResponse response = null;

		if (code == RESULT_OK) {
			response = new WP8ConsumeResponse((WP8ConsumeCodes) Enum.Parse(typeof(WP8ConsumeCodes), data[1]), data[2]);
			response.TransactionId = data[3];
		} else {
			response = new WP8ConsumeResponse((WP8ConsumeCodes) Enum.Parse(typeof(WP8ConsumeCodes), data[2]), data[1]);
		}

		OnConsumeFinished(response);
	}

	private void DefferedPurchaseCallback(string result) {
		string[] data = result.Split(new string[] { SPLITTER1 }, StringSplitOptions.None);
		int code = Int32.Parse(data[0]);
		WP8PurchseResponce response = null;
		
		if (code == RESULT_OK) {
			response = new WP8PurchseResponce((WP8PurchaseCodes) Enum.Parse(typeof(WP8PurchaseCodes), data[1]), data[2]);
			response.TransactionId = data[3];
		} else {
			response = new WP8PurchseResponce((WP8PurchaseCodes) Enum.Parse(typeof(WP8PurchaseCodes), data[2]), data[1]);
		}
		#if UNITY_WP8
		_defferedPurchases.Add(response);
		#endif
	}
}
