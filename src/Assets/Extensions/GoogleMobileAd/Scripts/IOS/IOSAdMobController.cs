//#define CODE_DISABLED
//#define SA_DEBUG_MODE
////////////////////////////////////////////////////////////////////////////////
//  
// @module Google Ads Unity SDK 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#if (UNITY_IPHONE && !UNITY_EDITOR && !CODE_DISABLED) || SA_DEBUG_MODE
using System.Runtime.InteropServices;
#endif

public class IOSAdMobController : SA_Singleton<IOSAdMobController>, GoogleMobileAdInterface {

	private bool _IsInited = false ;
	
	private Dictionary<int, IOSADBanner> _banners; 
	private string _BannersUunitId;
	private string _InterstisialUnitId;






	#if (UNITY_IPHONE && !UNITY_EDITOR && !CODE_DISABLED) || SA_DEBUG_MODE
	[DllImport ("__Internal")]
	private static extern void _initGoogleAd(string unit_id);

	[DllImport ("__Internal")]
	private static extern void _GADChangeBannersUnitID(string unit_id);

	[DllImport ("__Internal")]
	private static extern void _GADChangeInterstisialsUnitID(string unit_id);

	[DllImport ("__Internal")]
	private static extern void _GADAddKeyWord(string keyword);

	[DllImport ("__Internal")]
	private static extern void _GADAddTestDevice(string uid);

	[DllImport ("__Internal")]
	private static extern void _GADAddTestDevices(string uids);

	[DllImport ("__Internal")]
	private static extern void _GADSetGender(int gender);

	[DllImport ("__Internal")]
	private static extern void _GADSetBirthday (int day, int month, int year);
	
	[DllImport ("__Internal")]
	private static extern void _GADTagForChildDirectedTreatment (bool val);
		

	[DllImport ("__Internal")]
	private static extern void _GADDestroyBanner(int id);


	[DllImport ("__Internal")]
	private static extern void _GADStartInterstitialAd();
	
	[DllImport ("__Internal")]
	private static extern void _GADLoadInterstitialAd();
	
	[DllImport ("__Internal")]
	private static extern void _GADShowInterstitialAd();

	[DllImport ("__Internal")]
	private static extern void _GADReportPurchaseStatus(int status);

	#endif


	//Actions
	private Action _OnInterstitialLoaded 			= delegate {};
	private Action _OnInterstitialFailedLoading 	= delegate {};
	private Action _OnInterstitialOpened 			= delegate {};
	private Action _OnInterstitialClosed 			= delegate {};
	private Action _OnInterstitialLeftApplication  	= delegate {};
	private Action<string> _OnAdInAppRequest		= delegate {};
		

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	void Awake() {
		DontDestroyOnLoad(gameObject);
	}
	
	public void Init(string ad_unit_id) {


		if(_IsInited) {
			Debug.LogWarning ("Init shoudl be called only once. Call ignored");
			return;
		}

		_IsInited = true;
		_BannersUunitId = ad_unit_id;
		_InterstisialUnitId = ad_unit_id;
		_banners =  new Dictionary<int, IOSADBanner>();

		#if (UNITY_IPHONE && !UNITY_EDITOR && !CODE_DISABLED) || SA_DEBUG_MODE
		_initGoogleAd(ad_unit_id);
		#endif

	}


	public void Init(string banners_unit_id, string interstisial_unit_id) {
		if(_IsInited) {
			Debug.LogWarning ("Init shoudl be called only once. Call ignored");
			return;
		}
		
		Init(banners_unit_id);
		SetInterstisialsUnitID(interstisial_unit_id);
	}

	public void SetBannersUnitID(string ad_unit_id) {
		_BannersUunitId = ad_unit_id;

		#if (UNITY_IPHONE && !UNITY_EDITOR && !CODE_DISABLED) || SA_DEBUG_MODE
			_GADChangeBannersUnitID(ad_unit_id);
		#endif
	}
	
	public void SetInterstisialsUnitID(string ad_unit_id) {
		_InterstisialUnitId = ad_unit_id;
		#if (UNITY_IPHONE && !UNITY_EDITOR && !CODE_DISABLED) || SA_DEBUG_MODE
			_GADChangeInterstisialsUnitID(ad_unit_id);
		#endif
	}



	public GoogleMobileAdBanner CreateAdBanner(TextAnchor anchor, GADBannerSize size)  {
		if(!IsInited) {
			Debug.LogWarning ("CreateBannerAd shoudl be called only after Init function. Call ignored");
			return null;
		}
		
		IOSADBanner bannner = new IOSADBanner(anchor, size, GADBannerIdFactory.nextId);
		_banners.Add(bannner.id, bannner);
		
		return bannner;
		
	}


	public GoogleMobileAdBanner CreateAdBanner(int x, int y, GADBannerSize size)  {
		if(!IsInited) {
			Debug.LogWarning ("CreateBannerAd shoudl be called only after Init function. Call ignored");
			return null;
		}
		
		IOSADBanner bannner = new IOSADBanner(x, y, size, GADBannerIdFactory.nextId);
		_banners.Add(bannner.id, bannner);
		
		return bannner;
	}


	public void DestroyBanner(int id) {

		if(_banners != null) {
			if(_banners.ContainsKey(id)) {
				
				IOSADBanner banner = _banners[id];
				if(banner.IsLoaded) {
					_banners.Remove(id);
					#if (UNITY_IPHONE && !UNITY_EDITOR && !CODE_DISABLED) || SA_DEBUG_MODE
						_GADDestroyBanner(id);
					#endif
				} else {
					banner.DestroyAfterLoad();
				}
			}
		}

	}

	public void DirectBannerDestory(int id) {
		#if (UNITY_IPHONE && !UNITY_EDITOR && !CODE_DISABLED) || SA_DEBUG_MODE
		_GADDestroyBanner(id);
		#endif
	}

	

	public	void RecordInAppResolution(GADInAppResolution resolution) {
		#if (UNITY_IPHONE && !UNITY_EDITOR && !CODE_DISABLED) || SA_DEBUG_MODE
		_GADReportPurchaseStatus((int) resolution);
		#endif
	}

	//Add a keyword for targeting purposes.
	public void AddKeyword(string keyword)  {
		if(!IsInited) {
			Debug.LogWarning ("AddKeyword shoudl be called only after Init function. Call ignored");
			return;
		}

		#if (UNITY_IPHONE && !UNITY_EDITOR && !CODE_DISABLED) || SA_DEBUG_MODE
			_GADAddKeyWord(keyword);
		#endif

	}

	//Causes a device to receive test ads. The deviceId can be obtained by viewing the logcat output after creating a new ad.
	public void AddTestDevice(string deviceId) {
		if(!IsInited) {
			Debug.LogWarning ("AddTestDevice shoudl be called only after Init function. Call ignored");
			return;
		}
		
		#if (UNITY_IPHONE && !UNITY_EDITOR && !CODE_DISABLED) || SA_DEBUG_MODE
			_GADAddTestDevice(deviceId);
		#endif
	}


	private const string DEVICES_SEPARATOR = ",";
	//Causes a device to receive test ads. The deviceId can be obtained by viewing the logcat output after creating a new ad.
	public void AddTestDevices(params string[] ids) {
		if(!IsInited) {
			Debug.LogWarning ("AddTestDevice shoudl be called only after Init function. Call ignored");
			return;
		}
		
		if(ids.Length == 0) {
			return;
		}


		#if (UNITY_IPHONE && !UNITY_EDITOR && !CODE_DISABLED) || SA_DEBUG_MODE
		Debug.Log(string.Join(DEVICES_SEPARATOR, ids));
		_GADAddTestDevices(string.Join(DEVICES_SEPARATOR, ids));
		#endif
	}

	
	//Set the user's gender for targeting purposes. This should be GADGenger.GENDER_MALE, GADGenger.GENDER_FEMALE, or GADGenger.GENDER_UNKNOWN
	public void SetGender(GoogleGender gender) {
		if(!IsInited) {
			Debug.LogWarning ("SetGender shoudl be called only after Init function. Call ignored");
			return;
		}
		
		#if (UNITY_IPHONE && !UNITY_EDITOR && !CODE_DISABLED) || SA_DEBUG_MODE
			_GADSetGender((int) gender);
		#endif
	}
	

	public void SetBirthday(int year, AndroidMonth month, int day) {
		if(!IsInited) {
			Debug.LogWarning ("SetBirthday shoudl be called only after Init function. Call ignored");
			return;
		}

		#if (UNITY_IPHONE && !UNITY_EDITOR && !CODE_DISABLED) || SA_DEBUG_MODE
			_GADSetBirthday(day, (int) month, year);
		#endif
	}

	public void TagForChildDirectedTreatment(bool tagForChildDirectedTreatment) {
		if(!IsInited) {
			Debug.LogWarning ("TagForChildDirectedTreatment shoudl be called only after Init function. Call ignored");
			return;
		}

		#if (UNITY_IPHONE && !UNITY_EDITOR && !CODE_DISABLED) || SA_DEBUG_MODE
			_GADTagForChildDirectedTreatment(tagForChildDirectedTreatment);
		#endif
	}



	public void StartInterstitialAd() {
		if(!IsInited) {
			Debug.LogWarning ("StartInterstitialAd shoudl be called only after Init function. Call ignored");
			return;
		}
		
		#if (UNITY_IPHONE && !UNITY_EDITOR && !CODE_DISABLED) || SA_DEBUG_MODE
			_GADStartInterstitialAd();
		#endif
	}
	
	public void LoadInterstitialAd() {
		if(!IsInited) {
			Debug.LogWarning ("LoadInterstitialAd shoudl be called only after Init function. Call ignored");
			return;
		}
		
		#if (UNITY_IPHONE && !UNITY_EDITOR && !CODE_DISABLED) || SA_DEBUG_MODE
			_GADLoadInterstitialAd();
		#endif
	}
	
	public void ShowInterstitialAd() {
		if(!IsInited) {
			Debug.LogWarning ("ShowInterstitialAd shoudl be called only after Init function. Call ignored");
			return;
		}
		
		#if (UNITY_IPHONE && !UNITY_EDITOR && !CODE_DISABLED) || SA_DEBUG_MODE
			_GADShowInterstitialAd();
		#endif
	}




	//--------------------------------------
	//  GET / SET
	//--------------------------------------

	public GoogleMobileAdBanner GetBanner(int id) {
		if(_banners.ContainsKey(id)) {
			return _banners[id];
		} else {
			Debug.LogWarning("Banner id: " + id.ToString() + " not found");
			return null;
		}
	}
	
	
	
	public List<GoogleMobileAdBanner> banners {
		get {
			
			List<GoogleMobileAdBanner> allBanners =  new List<GoogleMobileAdBanner>();
			if(_banners ==  null) {
				return allBanners;
			}
			
			foreach(KeyValuePair<int, IOSADBanner> entry in _banners) {
				allBanners.Add(entry.Value);
			}
			
			return allBanners;
			
			
		}
	}


	public bool IsInited {
		get {
			return _IsInited;
		}
	}
	
	public string BannersUunitId {
		get {
			return _BannersUunitId;
		}
	}
	
	public string InterstisialUnitId {
		get {
			return _InterstisialUnitId;
		}
	}

	
	//--------------------------------------
	//  Actions 
	//--------------------------------------
	
	public Action OnInterstitialLoaded {
		get {
			return _OnInterstitialLoaded;
		}
		
		set {
			_OnInterstitialLoaded = value;
		}
	}
	
	public Action OnInterstitialFailedLoading {
		get {
			return _OnInterstitialFailedLoading;
		}
		
		set {
			_OnInterstitialFailedLoading = value;
		}
	}
	
	
	public Action OnInterstitialOpened {
		get {
			return _OnInterstitialOpened;
		}
		
		set {
			_OnInterstitialOpened = value;
		}
	}
	
	public Action OnInterstitialClosed {
		get {
			return _OnInterstitialClosed;
		}
		
		set {
			_OnInterstitialClosed = value;
		}
	}
	
	
	public Action OnInterstitialLeftApplication {
		get {
			return _OnInterstitialLeftApplication;
		}
		
		set {
			_OnInterstitialLeftApplication = value;
		}
	}
	
	
	public Action<string> OnAdInAppRequest {
		get {
			return _OnAdInAppRequest;
		}
		
		set {
			_OnAdInAppRequest = value;
		}
	}

	//--------------------------------------
	//  EVENTS BANNER AD
	//--------------------------------------
	
	private void OnBannerAdLoaded(string data)  {
		string[] bannerData;
		bannerData = data.Split(AndroidNative.DATA_SPLITTER [0]);

		int id = System.Convert.ToInt32(bannerData[0]);
		int w = System.Convert.ToInt32(bannerData[1]);
		int h = System.Convert.ToInt32(bannerData[2]);

		IOSADBanner banner = GetBanner(id) as IOSADBanner;
		if(banner != null) {
			banner.SetDimentions(w, h);
			banner.OnBannerAdLoaded();
		}
		
	}
	
	private void OnBannerAdFailedToLoad(string bannerID) {
		int id = System.Convert.ToInt32(bannerID);
		IOSADBanner banner = GetBanner(id) as IOSADBanner;
		if(banner != null) {
			banner.OnBannerAdFailedToLoad();
		}
	}
	
	private void OnBannerAdOpened(string bannerID) {
		int id = System.Convert.ToInt32(bannerID);
		IOSADBanner banner = GetBanner(id) as IOSADBanner;
		if(banner != null) {
			banner.OnBannerAdOpened();
		}
	}
	
	private void OnBannerAdClosed(string bannerID) {
		int id = System.Convert.ToInt32(bannerID);
		IOSADBanner banner = GetBanner(id) as IOSADBanner;
		if(banner != null) {
			banner.OnBannerAdClosed();
		}
	}
	
	private void OnBannerAdLeftApplication(string bannerID) {
		int id = System.Convert.ToInt32(bannerID);
		IOSADBanner banner = GetBanner(id) as IOSADBanner;
		if(banner != null) {
			banner.OnBannerAdLeftApplication();
		}
	}


	//--------------------------------------
	//  EVENTS INTERSTITIAL AD
	//--------------------------------------
	
	
	private void OnInterstitialAdLoaded()  {
		_OnInterstitialLoaded();
	}
	
	private void OnInterstitialAdFailedToLoad() {
		_OnInterstitialFailedLoading();
	}
	
	private void OnInterstitialAdOpened() {
		_OnInterstitialOpened();
	}
	
	private void OnInterstitialAdClosed() {
		_OnInterstitialClosed();
	}
	
	private void OnInterstitialAdLeftApplication() {
		_OnInterstitialLeftApplication();
	}

	//--------------------------------------
	//  GENERAL EVENTS
	//--------------------------------------
	
	private void OnInAppPurchaseRequested(string productId) {
		_OnAdInAppRequest(productId);
	}




}
