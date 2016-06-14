//#define SA_DEBUG_MODE
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#if (UNITY_WP8 && !UNITY_EDITOR) || SA_DEBUG_MODE
using GoogleAdsWP8;

#endif

public class WP8AdMobController : SA_Singleton<WP8AdMobController>, GoogleMobileAdInterface {
	
	private bool _IsInited = false ;
	
	private Dictionary<int, WP8ADBanner> _banners; 
	private string _BannersUunitId;
	private string _InterstisialUnitId;


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
		_banners =  new Dictionary<int, WP8ADBanner>();


		#if (UNITY_WP8 && !UNITY_EDITOR) || SA_DEBUG_MODE
		AdManager.instance.Init(ad_unit_id);

		AdManager.instance.InterstisialOnLoad = OnInterstitialAdLoaded;
		AdManager.instance.InterstisialFailedToLoad = OnInterstitialAdFailedToLoad;
		AdManager.instance.InterstisialOnAdOpened = OnInterstitialAdOpened;
		AdManager.instance.InterstisialOnAdClosed = OnInterstitialAdClosed;
		AdManager.instance.InterstisialAdLeftApplication = OnInterstitialAdLeftApplication;

		AdManager.instance.BannerOnLoad = OnBannerAdLoaded;
		AdManager.instance.BannerFailedToLoad = OnBannerAdFailedToLoad;
		AdManager.instance.BannerOnAdOpened = OnBannerAdOpened;
		AdManager.instance.BannerOnAdClosed = OnBannerAdClosed;
		AdManager.instance.BannerAdLeftApplication = OnBannerAdLeftApplication;

		SetOrientation(Screen.orientation);

		#endif

	}

	public void SetOrientation (ScreenOrientation orientation) {
		#if (UNITY_WP8 && !UNITY_EDITOR) || SA_DEBUG_MODE

		int mode = 0;
		if (orientation == ScreenOrientation.Landscape || orientation == ScreenOrientation.LandscapeLeft || orientation == ScreenOrientation.LandscapeRight) {
			mode = 1;
		}
		if (orientation == ScreenOrientation.Portrait || orientation == ScreenOrientation.PortraitUpsideDown) {
			mode = 2;
		}


		AdManager.instance.SetOrientation(mode);
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
		
		#if (UNITY_WP8 && !UNITY_EDITOR) || SA_DEBUG_MODE
		AdManager.instance.ChangeBannersUnitID(ad_unit_id);
		#endif
	}
	
	public void SetInterstisialsUnitID(string ad_unit_id) {
		_InterstisialUnitId = ad_unit_id;
		#if (UNITY_WP8 && !UNITY_EDITOR) || SA_DEBUG_MODE
		AdManager.instance.ChangeInterstisialsUnitID(ad_unit_id);
		#endif
	}
	
	
	
	public GoogleMobileAdBanner CreateAdBanner(TextAnchor anchor, GADBannerSize size)  {
		if(!IsInited) {
			Debug.LogWarning ("CreateBannerAd shoudl be called only after Init function. Call ignored");
			return null;
		}

		WP8ADBanner bannner = new WP8ADBanner(anchor, size, GADBannerIdFactory.nextId);
		_banners.Add(bannner.id, bannner);
		
		return bannner;
		
	}
	
	
	public GoogleMobileAdBanner CreateAdBanner(int x, int y, GADBannerSize size)  {
		if(!IsInited) {
			Debug.LogWarning ("CreateBannerAd shoudl be called only after Init function. Call ignored");
			return null;
		}
		
		WP8ADBanner bannner = new WP8ADBanner(TextAnchor.MiddleCenter, size, GADBannerIdFactory.nextId);
		_banners.Add(bannner.id, bannner);
		
		return bannner;
	}
		
	public void DestroyBanner(int id) {
		
		if(_banners != null) {
			if(_banners.ContainsKey(id)) {
				
				WP8ADBanner banner = _banners[id];
				if(banner.IsLoaded) {
					_banners.Remove(id);
					#if (UNITY_WP8 && !UNITY_EDITOR) || SA_DEBUG_MODE
					AdManager.instance.DestroyBanner(id);
					#endif
				} else {
					banner.DestroyAfterLoad();
				}
			}
		}
		
	}
		
	public	void RecordInAppResolution(GADInAppResolution resolution) {
		#if (UNITY_WP8 && !UNITY_EDITOR) || SA_DEBUG_MODE
		////////////_GADReportPurchaseStatus((int) resolution);
		#endif
	}
	
	//Add a keyword for targeting purposes.
	public void AddKeyword(string keyword)  {
		if(!IsInited) {
			Debug.LogWarning ("AddKeyword shoudl be called only after Init function. Call ignored");
			return;
		}
		
		#if (UNITY_WP8 && !UNITY_EDITOR) || SA_DEBUG_MODE
		AdManager.instance.AddKeyword(keyword);
		#endif
		
	}
	
	//Causes a device to receive test ads. The deviceId can be obtained by viewing the logcat output after creating a new ad.
	public void AddTestDevice(string deviceId) {
		if(!IsInited) {
			Debug.LogWarning ("AddTestDevice shoudl be called only after Init function. Call ignored");
			return;
		}
		
		#if (UNITY_WP8 && !UNITY_EDITOR) || SA_DEBUG_MODE
		AdManager.instance.EnableForceTesting();
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
		
		#if (UNITY_WP8 && !UNITY_EDITOR) || SA_DEBUG_MODE
		///////////_GADAddTestDevices(string.Join(DEVICES_SEPARATOR, ids));
		#endif
	}
	
	
	//Set the user's gender for targeting purposes. This should be GADGenger.GENDER_MALE, GADGenger.GENDER_FEMALE, or GADGenger.GENDER_UNKNOWN
	public void SetGender(GoogleGender gender) {
		if(!IsInited) {
			Debug.LogWarning ("SetGender shoudl be called only after Init function. Call ignored");
			return;
		}
		
		#if (UNITY_WP8 && !UNITY_EDITOR) || SA_DEBUG_MODE
		AdManager.instance.SetGender((int)gender);
		#endif
	}
	
	
	public void SetBirthday(int year, AndroidMonth month, int day) {
		if(!IsInited) {
			Debug.LogWarning ("SetBirthday shoudl be called only after Init function. Call ignored");
			return;
		}
		
		#if (UNITY_WP8 && !UNITY_EDITOR) || SA_DEBUG_MODE
		AdManager.instance.SetBirthday(year, (int)month, day);
		#endif
	}
	
	public void TagForChildDirectedTreatment(bool tagForChildDirectedTreatment) {
		if(!IsInited) {
			Debug.LogWarning ("TagForChildDirectedTreatment shoudl be called only after Init function. Call ignored");
			return;
		}
		
		#if (UNITY_WP8 && !UNITY_EDITOR) || SA_DEBUG_MODE
		////////////////////_GADTagForChildDirectedTreatment(tagForChildDirectedTreatment);
		#endif
	}

	public void StartInterstitialAd() {
		if(!IsInited) {
			Debug.LogWarning ("StartInterstitialAd shoudl be called only after Init function. Call ignored");
			return;
		}
		
		#if (UNITY_WP8 && !UNITY_EDITOR) || SA_DEBUG_MODE
		AdManager.instance.StartInterstitialAd();
		#endif
	}
	
	public void LoadInterstitialAd() {
		if(!IsInited) {
			Debug.LogWarning ("LoadInterstitialAd shoudl be called only after Init function. Call ignored");
			return;
		}
		
		#if (UNITY_WP8 && !UNITY_EDITOR) || SA_DEBUG_MODE
		AdManager.instance.LoadInterstitialAd();

		#endif
	}
	
	public void ShowInterstitialAd() {
		if(!IsInited) {
			Debug.LogWarning ("ShowInterstitialAd shoudl be called only after Init function. Call ignored");
			return;
		}
		
		#if (UNITY_WP8 && !UNITY_EDITOR) || SA_DEBUG_MODE
		AdManager.instance.ShowInterstitialAd();
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
			
			foreach(KeyValuePair<int, WP8ADBanner> entry in _banners) {
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
		
		WP8ADBanner banner = GetBanner(id) as WP8ADBanner;
		if(banner != null) {
			banner.SetDimentions(w, h);
			banner.OnBannerAdLoaded();
		}
		
	}
	
	private void OnBannerAdFailedToLoad(string bannerID) {
		int id = System.Convert.ToInt32(bannerID);
		WP8ADBanner banner = GetBanner(id) as WP8ADBanner;
		if(banner != null) {
			banner.OnBannerAdFailedToLoad();
		}
	}
	
	private void OnBannerAdOpened(string bannerID) {
		int id = System.Convert.ToInt32(bannerID);
		WP8ADBanner banner = GetBanner(id) as WP8ADBanner;
		if(banner != null) {
			banner.OnBannerAdOpened();
		}
	}
	
	private void OnBannerAdClosed(string bannerID) {
		int id = System.Convert.ToInt32(bannerID);
		WP8ADBanner banner = GetBanner(id) as WP8ADBanner;
		if(banner != null) {
			banner.OnBannerAdClosed();
		}
	}
	
	private void OnBannerAdLeftApplication(string bannerID) {
		int id = System.Convert.ToInt32(bannerID);
		WP8ADBanner banner = GetBanner(id) as WP8ADBanner;
		if(banner != null) {
			banner.OnBannerAdLeftApplication();
		}
	}
	
	
	//--------------------------------------
	//  EVENTS INTERSTITIAL AD
	//--------------------------------------
	
	
	private void OnInterstitialAdLoaded(string data)  {
		_OnInterstitialLoaded();
	}
	
	private void OnInterstitialAdFailedToLoad(string data) {
		_OnInterstitialFailedLoading();
	}
	
	private void OnInterstitialAdOpened(string data) {
		_OnInterstitialOpened();
	}
	
	private void OnInterstitialAdClosed(string data) {
		_OnInterstitialClosed();
	}
	
	private void OnInterstitialAdLeftApplication(string data) {
		_OnInterstitialLeftApplication();
	}
	
	//--------------------------------------
	//  GENERAL EVENTS
	//--------------------------------------
	
	private void OnInAppPurchaseRequested(string productId) {
		_OnAdInAppRequest(productId);
	}

	
	

}
