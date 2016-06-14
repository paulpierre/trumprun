using UnityEngine;
using System.Collections;

public class UM_PurchaseResult  {
	
	public bool isSuccess =  false;
	public UM_InAppProduct product =  new UM_InAppProduct();
	private int _ResponceCode = -1;
	
	
	
	public GooglePurchaseTemplate Google_PurchaseInfo = null;
	public IOSStoreKitResult IOS_PurchaseInfo = null;
	public WP8PurchseResponce WP8_PurchaseInfo = null;
	
	public void SetResponceCode(int code) {
		_ResponceCode = code;
	}
	
	public string TransactionId {
		get {
			if (Application.platform == RuntimePlatform.Android) {
				return Google_PurchaseInfo.orderId;
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
				return IOS_PurchaseInfo.TransactionIdentifier;
			} else if (Application.platform == RuntimePlatform.WP8Player) {
				return WP8_PurchaseInfo.TransactionId;
			}
			
			return string.Empty;
		}
	}
	
	public int ResponceCode {
		get {
			return _ResponceCode;
		}
	}
}
