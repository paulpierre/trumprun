using UnityEngine;
using System.Collections;

public class GoogleAnalyticsUseExample : MonoBehaviour {


	void Start () {

		//This call will be ignored of you already have GoogleAnalytics gameobject on your scene, like in the example scene
		//However if you do not want to create additional object in your initial scene
		//you may use this code to initialize analytics
		//WARNING: if you do not have GoogleAnalytics gamobect and you skip StartTracking call, GoogleAnalytics gameobect will be
		//initialized on first GoogleAnalytics.Client call
		GoogleAnalytics.StartTracking();
	}
	

	void OnGUI () {
		if(GUI.Button(new Rect(10, 10, 150, 50), "Page Hit")) {
			GoogleAnalytics.Client.SendPageHit("mydemo.com ", "/home", "homepage");
		}
		
		
		if(GUI.Button(new Rect(10, 70, 150, 50), "Event Hit")) {
			GoogleAnalytics.Client.SendEventHit("video", "play", "holiday", 300);
		}

		
		if(GUI.Button(new Rect(10, 130, 150, 50), "Transaction Hit")) {
			GoogleAnalytics.Client.SendTransactionHit("12345", "westernWear", "EUR", 50.00f, 32.00f, 12.00f);
		}

		if(GUI.Button(new Rect(10, 190, 150, 50), "Item Hit")) {
			GoogleAnalytics.Client.SendItemHit("12345", "sofa", "u3eqds43", 300f, 2, "furniture", "EUR");
		}

		if(GUI.Button(new Rect(190, 10, 150, 50), "Social Hit")) {
			GoogleAnalytics.Client.SendSocialHit("like", "facebook", "/home ");
		}

		if(GUI.Button(new Rect(190, 70, 150, 50), "Exception Hit")) {
			GoogleAnalytics.Client.SendExceptionHit("IOException", true);
		}

		if(GUI.Button(new Rect(190, 130, 150, 50), "Timing Hit")) {
			GoogleAnalytics.Client.SendUserTimingHit("jsonLoader", "load", 5000, "jQuery");
		}

		if(GUI.Button(new Rect(190, 190, 150, 50), "Screen Hit")) {
			GoogleAnalytics.Client.SendScreenHit("MainMenu");
		}





	
	}

	public void CustomBuildersExamples() {

		//Page Tracking
		GoogleAnalytics.Client.CreateHit(GoogleAnalyticsHitType.PAGEVIEW);
		GoogleAnalytics.Client.SetDocumentHostName("mydemo.com");
		GoogleAnalytics.Client.SetDocumentPath("/home");
		GoogleAnalytics.Client.SetDocumentTitle("homepage");

		GoogleAnalytics.Client.Send();


		//Event Tracking
		GoogleAnalytics.Client.CreateHit(GoogleAnalyticsHitType.EVENT);
		GoogleAnalytics.Client.SetEventCategory("video");
		GoogleAnalytics.Client.SetEventAction("play");
		GoogleAnalytics.Client.SetEventLabel("holiday");
		GoogleAnalytics.Client.SetEventValue(300);

		GoogleAnalytics.Client.Send();



		//Measuring Purchases
		GoogleAnalytics.Client.CreateHit(GoogleAnalyticsHitType.PAGEVIEW);
		GoogleAnalytics.Client.SetDocumentHostName("mydemo.com");
		GoogleAnalytics.Client.SetDocumentPath("/receipt");
		GoogleAnalytics.Client.SetDocumentTitle("Receipt Page");

		GoogleAnalytics.Client.SetTransactionID("T12345");
		GoogleAnalytics.Client.SetTransactionAffiliation("Google Store - Online");
		GoogleAnalytics.Client.SetTransactionRevenue(37.39f);
		GoogleAnalytics.Client.SetTransactionTax(2.85f);
		GoogleAnalytics.Client.SetTransactionShipping(5.34f);
		GoogleAnalytics.Client.SetTransactionCouponCode("SUMMER2013");

		GoogleAnalytics.Client.SetProductAction("purchase");
		GoogleAnalytics.Client.SetProductSKU(1, "P12345");
		GoogleAnalytics.Client.SetSetProductName(1, "Android Warhol T-Shirt");
		GoogleAnalytics.Client.SetProductCategory(1, "Apparel");
		GoogleAnalytics.Client.SetProductBrand(1, "Google");
		GoogleAnalytics.Client.SetProductVariant(1, "Black");
		GoogleAnalytics.Client.SetProductPosition(1, 1);

		GoogleAnalytics.Client.Send();



	

		//Measuring Refunds

		// Refund an entire transaction and send with a non-interaction event.
		GoogleAnalytics.Client.CreateHit(GoogleAnalyticsHitType.EVENT);
		GoogleAnalytics.Client.SetEventCategory("Ecommerce");
		GoogleAnalytics.Client.SetEventAction("Refund");
		GoogleAnalytics.Client.SetNonInteractionFlag();
		GoogleAnalytics.Client.SetTransactionID("T12345");
		GoogleAnalytics.Client.SetProductAction("refund");

		GoogleAnalytics.Client.Send();


		// Refund a single product.
		GoogleAnalytics.Client.CreateHit(GoogleAnalyticsHitType.EVENT);
		GoogleAnalytics.Client.SetEventCategory("Ecommerce");
		GoogleAnalytics.Client.SetEventAction("Refund");
		GoogleAnalytics.Client.SetNonInteractionFlag();
		GoogleAnalytics.Client.SetTransactionID("T12345");
		GoogleAnalytics.Client.SetProductAction("refund");
		GoogleAnalytics.Client.SetProductSKU(1, "P12345");
		GoogleAnalytics.Client.SetProductQuantity(1, 1);

		GoogleAnalytics.Client.Send();




		// Measuring Checkout Steps
		GoogleAnalytics.Client.CreateHit(GoogleAnalyticsHitType.PAGEVIEW);
		GoogleAnalytics.Client.SetDocumentHostName("mydemo.com");
		GoogleAnalytics.Client.SetDocumentPath("/receipt");
		GoogleAnalytics.Client.SetDocumentTitle("Receipt Page");
		
		GoogleAnalytics.Client.SetTransactionID("T12345");
		GoogleAnalytics.Client.SetTransactionAffiliation("Google Store - Online");
		GoogleAnalytics.Client.SetTransactionRevenue(37.39f);
		GoogleAnalytics.Client.SetTransactionTax(2.85f);
		GoogleAnalytics.Client.SetTransactionShipping(5.34f);
		GoogleAnalytics.Client.SetTransactionCouponCode("SUMMER2013");
		
		GoogleAnalytics.Client.SetProductAction("purchase");
		GoogleAnalytics.Client.SetProductSKU(1, "P12345");
		GoogleAnalytics.Client.SetSetProductName(1, "Android Warhol T-Shirt");
		GoogleAnalytics.Client.SetProductCategory(1, "Apparel");
		GoogleAnalytics.Client.SetProductBrand(1, "Google");
		GoogleAnalytics.Client.SetProductVariant(1, "Black");
		GoogleAnalytics.Client.SetProductPrice(1, 29.90f);
		GoogleAnalytics.Client.SetProductQuantity(1, 1);
		GoogleAnalytics.Client.SetCheckoutStep(1);
		GoogleAnalytics.Client.SetCheckoutStepOption("Visa");

		GoogleAnalytics.Client.Send();


		//Measuring Checkout Options
		GoogleAnalytics.Client.CreateHit(GoogleAnalyticsHitType.EVENT);
		GoogleAnalytics.Client.SetEventCategory("Checkout");
		GoogleAnalytics.Client.SetEventAction("Option");
		GoogleAnalytics.Client.SetProductAction("checkout_option");
		GoogleAnalytics.Client.SetCheckoutStep(2);
		GoogleAnalytics.Client.SetCheckoutStepOption("FedEx");

		GoogleAnalytics.Client.Send();



		//Measuring Internal Promotions

		//Promotion Impressions
		GoogleAnalytics.Client.CreateHit(GoogleAnalyticsHitType.PAGEVIEW);
		GoogleAnalytics.Client.SetDocumentHostName("mydemo.com");
		GoogleAnalytics.Client.SetDocumentPath("/home");
		GoogleAnalytics.Client.SetDocumentTitle("homepage");
		GoogleAnalytics.Client.SetPromotionID(1, "PROMO_1234");
		GoogleAnalytics.Client.SetPromotionName(1,"Summer Sale");
		GoogleAnalytics.Client.SetPromotionCreative(1, "summer_banner2");
		GoogleAnalytics.Client.SetPromotionPosition(1, "banner_slot1");
		
		GoogleAnalytics.Client.Send();


		//Promotion Clicks
		GoogleAnalytics.Client.CreateHit(GoogleAnalyticsHitType.EVENT);
		GoogleAnalytics.Client.SetEventCategory("Internal Promotions");
		GoogleAnalytics.Client.SetEventAction("click");
		GoogleAnalytics.Client.SetEventLabel("Summer Sale");
		GoogleAnalytics.Client.SetPromotionAction("click");
		GoogleAnalytics.Client.SetPromotionID(1, "PROMO_1234");
		GoogleAnalytics.Client.SetPromotionName(1,"Summer Sale");
		GoogleAnalytics.Client.SetPromotionCreative(1, "summer_banner2");
		GoogleAnalytics.Client.SetPromotionPosition(1, "banner_slot1");

		
		GoogleAnalytics.Client.Send();
		

		//Ecommerce Tracking



		//Transaction Hit
		GoogleAnalytics.Client.CreateHit(GoogleAnalyticsHitType.TRANSACTION);
		GoogleAnalytics.Client.SetTransactionID("12345");
		GoogleAnalytics.Client.SetTransactionAffiliation("westernWear");
		GoogleAnalytics.Client.SetTransactionRevenue(50);
		GoogleAnalytics.Client.SetTransactionShipping(32f);
		GoogleAnalytics.Client.SetTransactionTax(12f);
		GoogleAnalytics.Client.SetCurrencyCode("EUR");

		GoogleAnalytics.Client.Send();



		//Item Hit
		GoogleAnalytics.Client.CreateHit(GoogleAnalyticsHitType.ITEM);
		GoogleAnalytics.Client.SetTransactionID("12345");
		GoogleAnalytics.Client.SetItemName("sofa");
		GoogleAnalytics.Client.SetItemPrice(300f);
		GoogleAnalytics.Client.SetItemQuantity(2);
		GoogleAnalytics.Client.SetItemCode("u3eqds43");
		GoogleAnalytics.Client.SetItemCategory("furniture");
		GoogleAnalytics.Client.SetCurrencyCode("EUR");

		GoogleAnalytics.Client.Send();

			
		//Social Interactions
		GoogleAnalytics.Client.CreateHit(GoogleAnalyticsHitType.SOCIAL);     
		GoogleAnalytics.Client.SetSocialAction("like");     				
		GoogleAnalytics.Client.SetSocialNetwork("facebook"); 
		GoogleAnalytics.Client.SetSocialActionTarget("/home  ");

		GoogleAnalytics.Client.Send();


		//Exception Tracking
		GoogleAnalytics.Client.CreateHit(GoogleAnalyticsHitType.EXCEPTION);  
		GoogleAnalytics.Client.SetExceptionDescription("IOException");
		GoogleAnalytics.Client.SetIsFatalException(true);

		GoogleAnalytics.Client.Send();



		//User Timing Tracking
		GoogleAnalytics.Client.CreateHit(GoogleAnalyticsHitType.TIMING); 
		GoogleAnalytics.Client.SetUserTimingCategory("jsonLoader");
		GoogleAnalytics.Client.SetUserTimingVariableName("load");
		GoogleAnalytics.Client.SetUserTimingTime(5000);
		GoogleAnalytics.Client.SetUserTimingLabel("jQuery");

		GoogleAnalytics.Client.SetDNSTime(100);
		GoogleAnalytics.Client.SetPageDownloadTime(20);
		GoogleAnalytics.Client.SetRedirectResponseTime(32);
		GoogleAnalytics.Client.SetTCPConnectTime(56);
		GoogleAnalytics.Client.SetServerResponseTime(12);

		GoogleAnalytics.Client.Send();






		//Custom builder example

		//Simple Page hit
		GoogleAnalytics.Client.CreateHit(GoogleAnalyticsHitType.PAGEVIEW);
		GoogleAnalytics.Client.SetDocumentHostName("mydemo.com");
		GoogleAnalytics.Client.SetDocumentPath("/home");
		GoogleAnalytics.Client.SetDocumentTitle("homepage");
		
		GoogleAnalytics.Client.Send();
		
		//Constructing Same page hit without plugin methods
		GoogleAnalytics.Client.CreateHit(GoogleAnalyticsHitType.PAGEVIEW);
		GoogleAnalytics.Client.AppendData("dh", "mydemo.com");
		GoogleAnalytics.Client.AppendData("dp", "/home");
		GoogleAnalytics.Client.AppendData("dt", "homepage");

		GoogleAnalytics.Client.Send();

	


	}
}
