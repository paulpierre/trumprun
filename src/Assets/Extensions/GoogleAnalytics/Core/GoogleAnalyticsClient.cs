using System;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class GoogleAnalyticsClient  {


	private const string PROTOCOL_VERSION = "v=1";
	private const string HTTP_URL = "http://www.google-analytics.com/collect";
	private const string HTTPS_URL = "https://ssl.google-analytics.com/collect";
	
	public string TrackingID;
	public string ClientID;
	public string AppName;
	public string AppVersion;



	#if UNITY_IPHONE || UNITY_WP8 || UNITY_METRO 

		#if UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 

		private Hashtable Headers = new Hashtable();

		#else

		private  Dictionary<string, string> Headers = new Dictionary<string, string>();

		#endif

	#endif

	private string DefaultHitData;
	private StringBuilder builder = new StringBuilder(256, 8192);
	private GoogleAnalyticsHitType currentHitType;
	private string DataSendUrl;


	

	public GoogleAnalyticsClient(string anonymousClientID)  {

#if UNITY_EDITOR
		GoogleAnalyticsSettings.Instance.UpdateVersionAndName();
#endif


		ClientID = EscapeString(anonymousClientID, true);

		AppName 		= EscapeString(GoogleAnalyticsSettings.Instance.AppName, true);
		AppVersion 		= EscapeString(GoogleAnalyticsSettings.Instance.AppVersion, true);


		if(GoogleAnalyticsSettings.Instance.UseHTTPS) {
			DataSendUrl = HTTPS_URL;
		} else {
			DataSendUrl = HTTP_URL;
		}


		GenerateHeaders(GoogleAnalyticsSettings.Instance.GetCurentProfile().TrackingID);
	}

	public void GenerateHeaders(string trackingId) {

		TrackingID = EscapeString(trackingId);
		builder.Length = 0;
		
		
		builder.Append(PROTOCOL_VERSION);
		builder.Append("&tid="); 
		builder.Append(TrackingID);
		builder.Append("&cid="); 
		builder.Append(ClientID);


		builder.Append("&an=");
		builder.Append(AppName);
		builder.Append("&av=");
		builder.Append(AppVersion);

		
		
		DefaultHitData = builder.ToString();
		builder.Length = 0;
		
		
		
		#if UNITY_IPHONE
		
		
		if(Application.platform == RuntimePlatform.IPhonePlayer) {
			
			string model = SystemInfo.deviceModel;
			string opSys = SystemInfo.operatingSystem;
			string deviceType = "iPhone";
			string osVersion = "7.0.1";
			if (model.StartsWith("iPhone")) {
				deviceType = "iPhone";
				osVersion = opSys.Replace('.', '_');
			} else if (model.StartsWith("iPad")) {
				deviceType = "iPad";
				osVersion = opSys.Substring(opSys.IndexOf("OS")).Replace('.', '_');
			} else if (model.StartsWith("iPod")) {
				deviceType = "iPod";
				osVersion = opSys.Replace('.', '_');
			}
			
			
			builder.Append("Mozilla/5.0 (");
			builder.Append(deviceType);
			builder.Append("; CPU ");
			builder.Append(osVersion);
			builder.Append(" like Mac OS X)");
			
			string userAgent = builder.ToString();
			builder.Length = 0;
			
			Headers.Add("User-Agent", userAgent);
			
		}
		
		#endif
		
		
		#if UNITY_WP8 || UNITY_METRO
		
		
		//"Mozilla/5.0 (compatible; MSIE 10.0; Windows Phone 8.0; Trident/6.0; IEMobile/10.0; ARM; Touch; NOKIA; Lumia 920)"
		// Mozilla/5.0 (compatible; MSIE 10.0; Windows Phone 8.0; Trident/6.0; IEMobile/10.0; ARM; Touch; NOKIA; Lumia 920)
		// Mozilla/5.0 (compatible; MSIE 10.0; Windows Phone 8.0; Trident/6.0; ARM; Touch; IEMobile/10.0; <Manufacturer>; <Device> [;<Operator>])
		
		
		string model = SystemInfo.deviceModel;
		string deviceType = "compatible";
		string osVersion = SystemInfo.operatingSystem; //"Windows Phone 8.0";
		
		
		builder.Append("Mozilla/5.0 (");
		builder.Append(deviceType);
		builder.Append("; MSIE 10.0; ");
		builder.Append(osVersion);
		builder.Append("; Trident/6.0; IEMobile/10.0; ARM; Touch; " + model + " )");
		
		string userAgent = builder.ToString();
		builder.Length = 0;
		
		
		
		Headers.Add("User-Agent", userAgent);
		
		#endif
	}




	//--------------------------------------
	//  GENERAL PROTOCOL
	//--------------------------------------

	//When present, the IP address of the sender will be anonymized
	public void SetAnonymizeIP() {
		builder.Append("&aip=0");
	}

	public void SetQueueTime(int time) {
		builder.Append("&qt=");
		builder.Append(time);
	}


	//--------------------------------------
	//  Session
	//--------------------------------------

	public void StartSession() {
		builder.Append("&sc=start");
	}

	public void EndSession() {
		builder.Append("&sc=end");
	}

	public void IPOverride (string ip) {
		AppendData("uip", ip);
	}

	public void UserAgentOverride (string userAgent) {
		AppendData("ua", userAgent);
	}



	//--------------------------------------
	//  Traffic Sources
	//--------------------------------------


	public void SetDocumentReferrer(string url) {
		AppendData("dr", url, "Document Referrer", 2048);
	}


	public void SetCampaignName(string name) {
		AppendData("cn", name, "Campaign Name", 100);
	}

	public void SetCampaignSource(string source) {
		AppendData("cs", source, "Campaign Source", 100);
	}

	public void SetCampaignMedium(string medium) {
		AppendData("cm", medium, "Campaign Medium", 50);
	}

	public void AddCampaignKeyword(string keyword) {
		AppendData("ck", keyword, "Campaign Keyword", 500);
	}

	public void SetCampaignContent(string content) {
		AppendData("cc", content, "Campaign Content", 500);
	}


	public void SetCampaignID(string id) {
		AppendData("ci", id, "Campaign ID", 500);
	}

	public void SetGoogleAdWordsID(string id) {
		AppendData("gclid", id);
	}



	public void SetGoogleDisplayAdsID(string id) {
		AppendData("dclid", id);
	}



	//--------------------------------------
	//  SYSTEM INFO PROTOCOL
	//--------------------------------------

	public void SetScreenResolution(int width, int height) {
		builder.Append("&sr=");
		builder.Append(width);
		builder.Append('x');
		builder.Append(height);
	}

	public void SetViewportSize(int width, int height) {
		builder.Append("&vp=");
		builder.Append(width);
		builder.Append('x');
		builder.Append(height);
	}


	public void SetDocumentEncoding(string encoding) {
		AppendData("de", encoding);
	}


	public void SetScreenColors(string colorsBuffer) {
		AppendData("sd", colorsBuffer);
	}

	public void SetUserLanguage(string lang) {
		AppendData("ul", lang);
	}

	public void SetJavaEnablede(bool isEnabled) {
		string data = "0";
		if(isEnabled) {
			data = "1";
		}
		AppendData("je", data);
	}


	public void SetFlashVersion(string version) {
		AppendData("fl", version);
	}


	//--------------------------------------
	//  HIT PROTOCOL
	//--------------------------------------

	public void SetHitType(GoogleAnalyticsHitType hit) {
		AppendData("t", hit.ToString().ToLower());
	}

	public void SetNoInteractionHit() {
		builder.Append("&ni=1");
	}


	//--------------------------------------
	//  CONTENT INFORMATION PROTOCOL
	//--------------------------------------

	public void SetDocumentlocationURL(string url) {
		AppendData("dl", url, "Document location URL", 2048);
	}


	public void SetDocumentHostName(string host) {
		AppendData("dh", host, "Document Host Name", 100);
	}

	public void SetDocumentPath(string path) {
		AppendData("dp", path, "Document Path", 2048);
	}

	public void SetDocumentTitle(string title) {
		AppendData("dt", title, "Document Title", 1500);
	}

	public void SetScreenName(string name) {
		AppendData("cd", name, "Screen Name", 2048);
	}

	public void SetLinkID(string id) {
		AppendData("linkid", id);
	}


	//--------------------------------------
	//  APP TRACKING PROTOCOL
	//--------------------------------------

	public void SetApplicationName(string name) {
		AppendData("an", name, "Application Name", 100);
	}

	public void SetApplicationVersion(string version) {
		AppendData("av", version, "Application Version", 100);
	}

	public void SetApplicationInstallerID(string identifier) {
		AppendData("aiid", identifier, "Application Installer ID", 150);
	}



	//--------------------------------------
	//  EVENT TRACKING PROTOCOL
	//--------------------------------------

	
	public void SetEventCategory(string ec) {
		AppendData("ec", ec, "Event Category", 150, GoogleAnalyticsHitType.EVENT);
	}

	public void SetEventAction(string ea) {
		AppendData("ea", ea, "Event Action", 500, GoogleAnalyticsHitType.EVENT);
	}

	public void SetEventLabel(string el) {
		AppendData("el", el, "Event Label", 500, GoogleAnalyticsHitType.EVENT);
	}

	public void SetEventValue(int val) {
		AppendData("ev", val.ToString(), "", 0, GoogleAnalyticsHitType.EVENT);
	}


	//--------------------------------------
	//  E-COMMERCE PROTOCOL
	//--------------------------------------


	public void SetTransactionID(string ti) {
		AppendData("ti", ti, "Transaction ID", 500, GoogleAnalyticsHitType.TRANSACTION, GoogleAnalyticsHitType.ITEM);
	}

	public void SetTransactionAffiliation(string ta) {
		AppendData("ta", ta, "Transaction Affiliation", 500, GoogleAnalyticsHitType.TRANSACTION);
	}

	public void SetTransactionRevenue(float tr) {
		AppendData("tr", FloatToCurrency(tr), "", 0,  GoogleAnalyticsHitType.TRANSACTION);
	}

	public void SetTransactionShipping(float ts) {
		AppendData("ts", FloatToCurrency(ts), "", 0,  GoogleAnalyticsHitType.TRANSACTION);
	}

	public void SetTransactionTax(float tt) {
		AppendData("tt", FloatToCurrency(tt), "", 0,  GoogleAnalyticsHitType.TRANSACTION);
	}

	public void SetTransactionCouponCode(string tcc) {
		AppendData("tcc", tcc);
	}

	public void SetItemName(string name) {
		AppendData("in", name, "Item Name", 500,  GoogleAnalyticsHitType.ITEM);
	}

	public void SetItemPrice(float ip) {
		AppendData("ip", FloatToCurrency(ip), "", 0,  GoogleAnalyticsHitType.ITEM);
	}

	public void SetItemQuantity(int iq) {
		AppendData("iq", iq.ToString(), "", 0,  GoogleAnalyticsHitType.ITEM);
	}

	public void SetItemCode(string ic) {
		AppendData("ic", ic, "Item Code", 500,  GoogleAnalyticsHitType.ITEM);
	}

	public void SetItemCategory(string iv) {
		AppendData("iv", iv, "Item Category", 500,  GoogleAnalyticsHitType.ITEM);
	}
	

	public void SetCurrencyCode(string cu) {
		AppendData("cu", cu, "Currency Code", 10,  GoogleAnalyticsHitType.ITEM, GoogleAnalyticsHitType.TRANSACTION);
	}




	//--------------------------------------
	// Enhanced E-CommerceL
	//--------------------------------------

	public void SetProductSKU(int productIndex, string sku) {
		AppendData("pr" + productIndex.ToString() + "id", sku, "Product SKU", 500);
	}

	public void SetSetProductName(int productIndex, string name) {
		AppendData("pr" + productIndex.ToString() + "nm", name, "Product Name", 500);
	}

	public void SetProductBrand(int productIndex, string brand) {
		AppendData("pr" + productIndex.ToString() + "br", brand, "Product Brand ", 500);
	}

	public void SetProductCategory(int productIndex, string category) {
		AppendData("pr" + productIndex.ToString() + "ca", category, "Product Category ", 500);
	}

	public void SetProductVariant(int productIndex, string variant) {
		AppendData("pr" + productIndex.ToString() + "va", variant, "Product Variant ", 500);
	}

	public void SetProductPrice(int productIndex, float prise) {
		AppendData("pr" + productIndex.ToString() + "pr", FloatToCurrency(prise));
	}

	public void SetProductQuantity(int productIndex, int quantit) {
		AppendData("pr" + productIndex.ToString() + "qt", quantit.ToString());
	}



	public void SetProductCouponCode(int productIndex, string couponCode) {
		AppendData("pr" + productIndex.ToString() + "cc", couponCode, "Product Coupon Code", 500);
	}

	public void SetProductPosition(int productIndex, int pos) {
		AppendData("pr" + productIndex.ToString() + "ps", pos.ToString());
	}

	public void SetProductCustomDimension(int productIndex, int index, string val) {
		AppendData("pr" + productIndex.ToString() + "cd" + index.ToString(), val);
	}

	public void SetProductCustomMetric(int productIndex, int index, int metric) {
		AppendData("pr" + productIndex.ToString() + "cm" + index.ToString(), metric.ToString());
	}

	public void SetProductAction(string pa) {
		AppendData("pa", pa);
	}
	
	public void SetProductActionList(string val) {
		AppendData("pal", val);
	}


	public void SetCheckoutStep(int cos) {
		AppendData("cos", cos.ToString());
	}

	public void SetCheckoutStepOption(string col) {
		AppendData("col", col);
	}

	/*
	public void ProductImpressionListName(int productIndex, string val) {
		AppendData("il" + productIndex.ToString() + "nm", val);
	}

	public void ProductImpressionListName(int productIndex, string val) {
		AppendData("il" + productIndex.ToString() + "nm", val);
	}
	*/


	//--------------------------------------
	// SOCIAL INTERACTIONS PROTOCOL
	//--------------------------------------

	public void SetSocialNetwork(string sn) {
		AppendData("sn", sn, "Social Network", 50,  GoogleAnalyticsHitType.SOCIAL);
	}

	public void SetSocialAction(string action) {
		AppendData("sa", action, "Social Action", 50,  GoogleAnalyticsHitType.SOCIAL);
	}

	public void SetSocialActionTarget(string target) {
		AppendData("st", target, "Social Action Target", 2048,  GoogleAnalyticsHitType.SOCIAL);
	}


	//--------------------------------------
	// TIMING PROTOCOL
	//--------------------------------------

	public void SetUserTimingCategory(string category) {
		AppendData("utc", category, "User Timing Category", 150,  GoogleAnalyticsHitType.TIMING);
	}

	public void SetUserTimingVariableName(string name) {
		AppendData("utv", name, "User Timing Variable Name", 500,  GoogleAnalyticsHitType.TIMING);
	}

	public void SetUserTimingTime(int time) {
		AppendData("utt", time.ToString(), "User Timing Time", 0,  GoogleAnalyticsHitType.TIMING);
	}

	public void SetUserTimingLabel(string label) {
		AppendData("utl", label, "User Timing Label", 500,  GoogleAnalyticsHitType.TIMING);
	}

	public void SetPageLoadTime(int time) {
		AppendData("plt", time.ToString(), "Page Load Time", 0,  GoogleAnalyticsHitType.TIMING);
	}

	public void SetDNSTime(int time) {
		AppendData("dns", time.ToString(), "DNS Time", 0,  GoogleAnalyticsHitType.TIMING);
	}

	public void SetPageDownloadTime(int time) {
		AppendData("pdt", time.ToString(), "", 0,  GoogleAnalyticsHitType.TIMING);
	}

	public void SetRedirectResponseTime(int time) {
		AppendData("rrt", time.ToString(), "", 0,  GoogleAnalyticsHitType.TIMING);
	}

	public void SetTCPConnectTime(int time) {
		AppendData("tcp", time.ToString(), "", 0,  GoogleAnalyticsHitType.TIMING);
	}


	public void SetServerResponseTime(int time) {
		AppendData("srt", time.ToString(), "", 0,  GoogleAnalyticsHitType.TIMING);
	}

	//--------------------------------------
	// Promotion 
	//--------------------------------------

	public void SetPromotionID(int index, string id) {
		AppendData("promo" + index.ToString() + "id", id);
	}

	public void SetPromotionName(int index, string nm) {
		AppendData("promo" + index.ToString() + "nm",  nm);
	}

	public void SetPromotionCreative(int index, string cr) {
		AppendData("promo" + index.ToString() + "cr", cr);
	}

	public void SetPromotionPosition(int index, string ps) {
		AppendData("promo" + index.ToString() + "ps", ps);
	}

	public void SetPromotionAction(string promoa) {
		AppendData("promoa", promoa);
	}

	

	//--------------------------------------
	// EXCEPTIONS PROTOCOL
	//--------------------------------------

	public void SetExceptionDescription(string description) {
		AppendData("exd", description, "Exception Description", 150,  GoogleAnalyticsHitType.EXCEPTION);
	}

	public void SetIsFatalException(bool isFatal) {
		string data = "0";
		if(isFatal) {
			data = "1";
		}
		AppendData("exf", data, "", 0,  GoogleAnalyticsHitType.EXCEPTION);
	}


	public void SetNonInteractionFlag() {
		AppendData("ni", "1");
	}

	
	//--------------------------------------
	// Custom Dimensions / Metrics PROTOCOL
	//--------------------------------------


	public void SetCustomDimension(int index, string value) {
		AppendData("cd" + index.ToString(), value, "Custom Dimension", 150);
	}

	public void SetCustomMetric(int index, int value) {
		AppendData("cm" + index.ToString(), value.ToString());
	}


	//--------------------------------------
	// CONTENT EXPERIMENTS PROTOCOL
	//--------------------------------------

	public void SetExperimentID(string id) {
		AppendData("xid", id, "Experiment ID", 40);
	}

	public void SetExperimentVariant(string variant) {
		AppendData("xvar", variant);
	}


	//--------------------------------------
	//  METHODS API
	//--------------------------------------



	

	public void SendPageHit(string host, string page, string title, string description = "", string linkId = "") {
		CreateHit(GoogleAnalyticsHitType.PAGEVIEW);
		SetDocumentHostName(host);
		SetDocumentPath(page);
		SetDocumentTitle(title);

		if(!description.Equals(string.Empty)) {
			SetScreenName(description);
		}

		if(!linkId.Equals(string.Empty)) {
			SetLinkID(linkId);
		}

		Send();
	}


	public void SendEventHit(string category, string action, string label = "", int val = -1, bool trackLevelName = false) {
		CreateHit(GoogleAnalyticsHitType.EVENT);
		SetEventCategory(category);
		SetEventAction(action);

	
		if(trackLevelName) {
			SetScreenName(GoogleAnalyticsSettings.Instance.LevelPrefix + GoogleAnalytics.LoadedLevelName + GoogleAnalyticsSettings.Instance.LevelPostfix);
		}


		
		if(!label.Equals(string.Empty)) {
			SetEventLabel(label);
		}
		
		if(val != -1) {
			SetEventValue(val);
		}
		
		Send();
	}


	public void SendTransactionHit(string id, string affiliation = "", string currencyCode = "",  float revenue = 0f, float shipping = 0f, float tax = 0f) {
		CreateHit(GoogleAnalyticsHitType.TRANSACTION);


		SetTransactionID(id);

		if(!string.IsNullOrEmpty(affiliation)) {
			SetTransactionAffiliation(affiliation);
		}

		if(!string.IsNullOrEmpty(currencyCode)) {
			SetCurrencyCode(currencyCode);
		}

		if(revenue != 0f) {
			SetTransactionRevenue(revenue);
		}

		if(shipping != 0f) {
			SetTransactionShipping(shipping);
		}

		if(tax != 0f) {
			SetTransactionTax(tax);
		}

		
		Send();
	}


	public void SendItemHit(string transactionId, string name, string SKU, float price, int quantity = 1, string category = "", string currencyCode = "") {
		CreateHit(GoogleAnalyticsHitType.ITEM);
		
		SetTransactionID(transactionId);
		SetItemName(name);
		SetItemCode(SKU);
		SetItemPrice(price);
		SetItemQuantity(quantity);

		if( !string.IsNullOrEmpty(category) ) {
			SetItemCategory(category);
		}
		
		if(!string.IsNullOrEmpty(currencyCode)) {
			SetCurrencyCode(currencyCode);
		}

		
		Send();
	}


	public void SendSocialHit(string action, string network, string target) {
		CreateHit(GoogleAnalyticsHitType.SOCIAL);
		SetSocialAction(action);
		SetSocialNetwork(network);
		SetSocialActionTarget(target);

		Send();
	}

	public void SendExceptionHit(string description, bool IsFatal = false) {
		CreateHit(GoogleAnalyticsHitType.EXCEPTION);
		SetExceptionDescription(description);

		if(IsFatal) {
			SetIsFatalException(IsFatal);
		}
		
		Send();
	}


	public void SendUserTimingHit(string category, string variable, int time, string label) {
	
		CreateHit(GoogleAnalyticsHitType.TIMING);
		SetUserTimingCategory(category);
		SetUserTimingVariableName(variable);
		SetUserTimingTime(time);
		SetUserTimingLabel(label);

		Send();

	}

	public void SendScreenHit(string screenName) {
		CreateHit(GoogleAnalyticsHitType.APPVIEW);
		SetScreenName(screenName);
		Send();
	}

	
	public void CreateHit(GoogleAnalyticsHitType hit) {
		currentHitType = hit;
		
		builder.Length = 0;
		builder.Append(DefaultHitData);
		SetHitType(hit);
	}

	public void Send() {

		if(GoogleAnalyticsSettings.Instance.IsDisabled) {
			return;
		}

		#if UNITY_EDITOR

		if(!GoogleAnalyticsSettings.Instance.EditorAnalytics) {
			return;
		}

		#endif

		//anti Cache
		builder.Append("&z=");
		builder.Append(UnityEngine.Random.Range(0, System.Int32.MaxValue) ^ 3424);


		string stringData = builder.ToString();
		builder.Length = 0;

		GoogleAnalytics.Send(stringData);

	}


	public WWW GenerateWWW(byte[] data) {
		#if UNITY_IPHONE || UNITY_WP8 || UNITY_METRO
		return new WWW(DataSendUrl, data, Headers);
		#else


		return new WWW(DataSendUrl, data);
		#endif
	}




	
	//--------------------------------------
	//  Get / Set
	//--------------------------------------

	public string AnalyticsHost {
		get {
			return DataSendUrl;
		}

	}

	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------

	
	public void AppendData(string protocolToken, string val) {
		AppendData(protocolToken, val, "", 0);
	}



	public void AppendData(string protocolToken, string val, string action, int maxSize, params GoogleAnalyticsHitType[] supportedHitTypes) {


		if(supportedHitTypes.Length > 0) {

			bool isActionSupprted = false;

			foreach(GoogleAnalyticsHitType h in supportedHitTypes) {
				if(h == currentHitType) {
					isActionSupprted = true;
				}
			}

			if(!isActionSupprted) {
				Debug.LogWarning("Google Analytics: " + action + " not supported for hit type  " + currentHitType.ToString());
				return;
			}

		}

		string data = EscapeString(val);
		builder.Append("&");
		builder.Append(protocolToken);
		builder.Append("=");
		builder.Append(data);

		if(maxSize > 0) {
			CheckDataLength(action, data, maxSize);
		}
	}




	private string FloatToCurrency(float val) {
		return val.ToString("n2");
	}

	private void CheckDataLength(string action, string data, int maxLength) {

		if(string.IsNullOrEmpty(data)) {
			Debug.LogWarning("Google Analytics: " + action + " data is null");
			return;
		}

		if(data.Length > maxLength) {
			Debug.LogWarning("Google Analytics: " + action + " is too long, max size " + maxLength + " bytes (" + data + ")");
		}
	}
	


	private string EscapeString(string original) {
		return EscapeString(original, false);
	}
	
	private string EscapeString(string original, bool forced) {
		if(forced) {
			return WWW.EscapeURL(original);
		} else {
			if(GoogleAnalyticsSettings.Instance.StringEscaping) {
				return WWW.EscapeURL(original);
			} else {
				return original;
			}
		}
	}

}