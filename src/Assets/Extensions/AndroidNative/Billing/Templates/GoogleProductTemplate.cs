////////////////////////////////////////////////////////////////////////////////
//  
// @module Android Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

[System.Serializable]
public class GoogleProductTemplate  {

	//Editor Only
	public bool IsOpen = true;

	public string SKU = string.Empty;


	private string _OriginalJson = string.Empty;


	[SerializeField]
	private string _LocalizedPrice = "0.99 $";

	[SerializeField]
	private long   _PriceAmountMicros = 990000;

	[SerializeField]
	private string _PriceCurrencyCode = "USD";

	
	[SerializeField]
	private string _Description = string.Empty;

	[SerializeField]
	private string _Title =  "New Product";

	[SerializeField]
	private Texture2D _Texture;

	[SerializeField]
	private AN_InAppType _ProductType = AN_InAppType.Consumable;

	[System.Obsolete("originalJson is deprectaed, please use OriginalJson instead")]
	public string originalJson {
		get {
			return _OriginalJson;
		} 
		
		set {
			_OriginalJson = value;
		}
	}

	public string OriginalJson {
		get {
			return _OriginalJson;
		} 
		
		set {
			_OriginalJson = value;
		}
	}

	[System.Obsolete("price is deprectaed, please use Price instead")]
	public float price {
		get {
			return Price;
		} 

	}

	public float Price {
		get {
			return _PriceAmountMicros / 1000000f;
		} 
		

	}

	[System.Obsolete("priceAmountMicros is deprectaed, please use PriceAmountMicros instead")]
	public long priceAmountMicros  {
		get {
			return _PriceAmountMicros;
		}

		set {
			_PriceAmountMicros = value;
		}
	}

	public long PriceAmountMicros  {
		get {
			return _PriceAmountMicros;
		}
		
		set {
			_PriceAmountMicros = value;
		}
	}



	[System.Obsolete("priceCurrencyCode is deprectaed, please use PriceCurrencyCode instead")]
	public string priceCurrencyCode  {
		get {
			return _PriceCurrencyCode;
		}

		set {
			_PriceCurrencyCode = value;
		}
	}


	public string PriceCurrencyCode  {
		get {
			return _PriceCurrencyCode;
		}
		
		set {
			_PriceCurrencyCode = value;
		}
	}

	public string LocalizedPrice {
		get {
			return _LocalizedPrice;
		}

		set {
			_LocalizedPrice = value;
		}
	}


	[System.Obsolete("description is deprectaed, please use Description instead")]
	public string description {
		get {
			return _Description;
		}
		
		set {
			_Description = value;
		}
	}

	public string Description {
		get {
			return _Description;
		}
		
		set {
			_Description = value;
		}
	}

	[System.Obsolete("title is deprectaed, please use Title instead")]
	public string title {
		get {
			return _Title;
		}
		
		set {
			_Title = value;
		}
	}

	public string Title {
		get {
			return _Title;
		}
		
		set {
			_Title = value;
		}
	}

	public Texture2D Texture {
		get {
			return _Texture;
		}
		
		set {
			_Texture = value;
		}
	}

	public AN_InAppType ProductType {
		get {
			return _ProductType;
		}
		
		set {
			_ProductType =  value;
		}
	}
}
