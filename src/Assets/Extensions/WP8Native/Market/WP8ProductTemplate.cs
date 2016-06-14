using UnityEngine;
using System;
using System.Collections;

public class WP8ProductTemplate  {

	/// <summary>
	/// Gets the description for the in-app product.
	/// </summary>
	public string Description { get; set; }
	
	/// <summary>
	/// Gets the URI of the image associated with the in-app product.
	/// </summary>
	public string ImgURI { get; set; }
	
	/// <summary>
	/// Gets the descriptive name of the in-app product that is displayed customers in the current market.
	/// </summary>
	public string Name { get; set; }
	
	/// <summary>
	/// Gets the in-app product ID.
	/// </summary>
	public string ProductId { get; set; }
	
	/// <summary>
	/// Gets the purchase price for the in-app product with the appropriate formatting for the current market.
	/// </summary>
	public string Price { get; set; }
	
	/// <summary>
	/// Gets the transaction ID for the Consumable in-app product, which was bought, but not fulfilled
	/// </summary>
	public string TransactionID { get; set; }
	
	/// <summary>
	/// Gets the type of this in-app product. Possible values are defined by ProductType.
	/// </summary>
	public WP8PurchaseProductType Type { get; set; }
	
	/// <summary>
	/// Gets a value that indicates whether the in-app product is purchased.
	/// </summary>
	[System.Obsolete("This property is obsolete. Use 'IsPurchased' property instead")]
	public bool isPurchased {
		get {
			return IsPurchased;
		}
		set {
			IsPurchased = value;
		}
	}

	/// <summary>
	/// Gets a value that indicates whether the in-app product is purchased.
	/// </summary>
	public bool IsPurchased { get; set; }

	private Texture2D _texture;

	[System.Obsolete("This property is obsolete. Use 'Texture' property instead")]
	public Texture2D texture {
		get {
			return _texture;
		}
	}
	
	public Texture2D Texture {
		get {
			return _texture;
		}
	}

	public event Action<Texture2D> ProdcutImageLoaded =  delegate {};

	public void LoadProductImage() {
		
		if(_texture != null) {
			ProdcutImageLoaded(_texture);
			return;
		}
		
		
		WPN_TextureLoader loader = WPN_TextureLoader.Create();
		loader.TextureLoaded += HandleTextureLoaded;
		loader.LoadTexture(ImgURI);
	}

	private void HandleTextureLoaded(Texture2D texture) {
		_texture = texture;
		ProdcutImageLoaded(_texture);

	}

}
