using UnityEngine;
using System.Text;
using System.Collections;

public class WPN_BillingManagerExample : MonoBehaviour {


	public const string YOUR_DURABLE_PRODUCT_ID_CONSTANT 		= "item2";
	public const string YOUR_CONSUMABLE_PRODUCT_ID_CONSTANT		= "item1";



	private static bool _IsInited = false;

	public static string _status = string.Empty;

	public static void Init() {
		WP8InAppPurchasesManager.OnInitComplete += HandleOnInitComplete;
		WP8InAppPurchasesManager.OnPurchaseFinished += HandleOnPurchaseFinished;	

		WP8InAppPurchasesManager.Instance.Init();
	}

	public static void Purchase(string productId) {
		WP8InAppPurchasesManager.Instance.Purchase(productId);
	}

#if UNITY_WSA
	public static void Consume (string productId) {
		WP8InAppPurchasesManager.Instance.Consume(productId);
	}
#endif

	public static bool IsInited {
		get {
			return _IsInited;
		}
	}



	private static void HandleOnPurchaseFinished(WP8PurchseResponce responce) {
	
		if(responce.IsSuccses) {
			//Unlock logic for product with id recponce.productId should be here
			WP8Dialog.Create("Purchase Succse", "Product: " + responce.ProductId);
		} else {
			//Purchase fail logic for product with id recponce.productId should be here
			WP8Dialog.Create("Purchase Failed", "Product: " + responce.ProductId);
		}
	}


	private static void HandleOnInitComplete(WP8_InAppsInitResult result) {

		if(result.IsFailed) {
			_status = "[Billing Init] Status" + result.IsSucceeded;

			return;
		}


		_IsInited = true;

		WP8InAppPurchasesManager.OnInitComplete -= HandleOnInitComplete;

		StringBuilder builder = new StringBuilder();
		foreach(WP8ProductTemplate product in WP8InAppPurchasesManager.Instance.Products) {
			if(product.Type == WP8PurchaseProductType.Durable) {
				if(product.IsPurchased) {
					//The Durable product was purchased, we should check here 
					//if the content is unlocked for our Durable product.

					Debug.Log("Product " + product.Name + " is purchased");
				}
			}

			builder.AppendLine(string.Format("[PRODUCT] {0} {1} {2} {3}", product.ProductId, product.Name, product.Type.ToString(), product.Price));
		}
		_status = builder.ToString();

		WP8Dialog.Create("market Initted", "Total products avaliable: " + WP8InAppPurchasesManager.Instance.Products.Count);
	}




}

