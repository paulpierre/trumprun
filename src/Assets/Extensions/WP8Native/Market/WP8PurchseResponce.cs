using UnityEngine;
using System.Collections;

public class WP8PurchseResponce  {

	private WP8PurchaseCodes _code;

	private string _transactionId = string.Empty;
	private string _productId = string.Empty;
		
	public WP8PurchseResponce(WP8PurchaseCodes code, string id) {
		_code = code;
		_productId = id;
	}
		
	public bool IsSuccses {
		get {
			if(_code == WP8PurchaseCodes.Succeeded) {
				return true;
			} else {
				return false;
			}
		}
	}

	public string ProductId {
		get {
			return _productId;
		}
	}

	public string TransactionId {
		get {
			return _transactionId;
		}
		set {
			_transactionId = value;
		}
	}
}
