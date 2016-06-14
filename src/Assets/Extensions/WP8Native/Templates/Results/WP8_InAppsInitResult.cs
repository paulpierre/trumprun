using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WP8_InAppsInitResult : WP8_Result {

	public WP8_InAppsInitResult(int statusCode): base(statusCode == WP8InAppPurchasesManager.RESULT_OK) {
	}

	[System.Obsolete("This property is deprecated. Use 'Products' property instead")]
	public List<WP8ProductTemplate> products {
		get {
			return Products;
		}
	}

	public List<WP8ProductTemplate> Products {
		get {
			return WP8InAppPurchasesManager.Instance.Products;
		}
	}
}
