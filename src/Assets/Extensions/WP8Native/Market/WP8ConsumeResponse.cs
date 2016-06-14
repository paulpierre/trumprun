using UnityEngine;
using System.Collections;

public class WP8ConsumeResponse {

	private WP8ConsumeCodes _code;

	private string _productId = string.Empty;
	private string _transactionId = string.Empty;
	
	public WP8ConsumeResponse(WP8ConsumeCodes code, string id) {
		_code = code;
		_productId = id;
	}
	
	public bool IsSucceeded {
		get {
			if(_code == WP8ConsumeCodes.Succeeded) {
				return true;
			} else {
				return false;
			}
		}
	}

	public string ProductID {
		get {
			return _productId;
		}
	}

	public WP8ConsumeCodes Code {
		get {
			return _code;
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
