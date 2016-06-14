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

#if (UNITY_IPHONE && !UNITY_EDITOR && !CODE_DISABLED) || SA_DEBUG_MODE
using System.Runtime.InteropServices;
#endif


public class IOSADBanner :  GoogleMobileAdBanner  {


	#if (UNITY_IPHONE && !UNITY_EDITOR && !CODE_DISABLED) || SA_DEBUG_MODE
	
	[DllImport ("__Internal")]
	private static extern void _GADCreateBannerAd(int gravity, int size, int id);

	[DllImport ("__Internal")]
	private static extern void _GADCreateBannerAdPos(int x, int y, int size, int id);
	
	[DllImport ("__Internal")]
	private static extern void _GADShowAd(int id);
	
	[DllImport ("__Internal")]
	private static extern void _GADHideAd(int id);

	[DllImport ("__Internal")]
	private static extern void _GADRefresh(int id);

	[DllImport ("__Internal")]
	private static extern void _GADSetPosition(int gravity, int bannerId);

	[DllImport ("__Internal")]
	private static extern void _GADSetPositionXY(int x, int y, int bannerId);
	

	
	#endif

	private int _id;
	private GADBannerSize _size;
	private TextAnchor _anchor;
	
	private bool _IsLoaded = false;
	private bool _IsOnScreen = false;
	private bool firstLoad = true;
	
	private bool _ShowOnLoad = true;
	private bool destroyOnLoad = false;

	private int _width 	= 0;
	private int _height = 0;


	private Action<GoogleMobileAdBanner> _OnLoadedAction 				= delegate {};
	private Action<GoogleMobileAdBanner> _OnFailedLoadingAction 		= delegate {};
	private Action<GoogleMobileAdBanner> _OnOpenedAction 				= delegate {};
	private Action<GoogleMobileAdBanner> _OnClosedAction 				= delegate {};
	private Action<GoogleMobileAdBanner> _OnLeftApplicationAction 	= delegate {};
	
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	
	public IOSADBanner(TextAnchor anchor, GADBannerSize size, int id) {

		_id = id;
		_size = size;
		_anchor = anchor;


		#if (UNITY_IPHONE && !UNITY_EDITOR && !CODE_DISABLED) || SA_DEBUG_MODE
		_GADCreateBannerAd(gravity, (int) _size, _id);
		#endif



		
	}
	
	public IOSADBanner(int x, int y, GADBannerSize size, int id) {


		_id = id;
		_size = size;

		#if (UNITY_IPHONE && !UNITY_EDITOR && !CODE_DISABLED) || SA_DEBUG_MODE
		_GADCreateBannerAdPos(x, y, (int) _size, _id);

		#endif
		
	}
	
	
	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	public void Hide() { 



		if(!_IsOnScreen) {
			return;
		}
		
		_IsOnScreen = false;


		#if (UNITY_IPHONE && !UNITY_EDITOR && !CODE_DISABLED) || SA_DEBUG_MODE
		_GADHideAd(_id);
		#endif
	}
	
	
	public void Show() { 



		if(_IsOnScreen) {
			return;
		}
		
		_IsOnScreen = true;


		#if (UNITY_IPHONE && !UNITY_EDITOR && !CODE_DISABLED) || SA_DEBUG_MODE
		_GADShowAd(_id);
		#endif
	}
	
	
	public void Refresh() { 
		#if (UNITY_IPHONE && !UNITY_EDITOR && !CODE_DISABLED) || SA_DEBUG_MODE
		
		if(!_IsLoaded) {
			return;
		}
		

		_GADRefresh(_id);
		
		#endif
	}


	public void SetBannerPosition(int x, int y) {
		#if (UNITY_IPHONE && !UNITY_EDITOR && !CODE_DISABLED) || SA_DEBUG_MODE
		
		if(!_IsLoaded) {
			return;
		}
		
		_GADSetPositionXY(x, y, _id);
		
		#endif
	}

	public void SetBannerPosition(TextAnchor anchor) {

		#if (UNITY_IPHONE && !UNITY_EDITOR && !CODE_DISABLED) || SA_DEBUG_MODE
		
		if(!_IsLoaded) {
			return;
		}


		_anchor = anchor;
		_GADSetPosition(gravity, id);
		
		#endif
	}

	//If user whant destoy banner before it gots loaded
	public void DestroyAfterLoad() {
		destroyOnLoad = true;
		ShowOnLoad = false;
	}


	public void SetDimentions(int w, int h) {
		_width = Mathf.FloorToInt(w * (Screen.dpi / 160f));
		_height =Mathf.FloorToInt(h * (Screen.dpi / 160f)); 
	}
	
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	public int id {
		get {
			return _id;
		}
	}

	
	public int width {
		get {
			return _width;
		}
	}
	
	public int height {
		get {
			return _height;
		}
	}

	
	public GADBannerSize size {
		get {
			return _size;
		}
	}
	
	
	public bool IsLoaded {
		get {
			return _IsLoaded;
		}
	}
	
	public bool IsOnScreen {
		get {
			return _IsOnScreen;
		}
	}
	
	public bool ShowOnLoad {
		get {
			return _ShowOnLoad;
		} 
		
		set {
			_ShowOnLoad = value;
		}
	}
	
	public TextAnchor anchor {
		get {
			return _anchor;
		}
	}
	
	
	public int gravity {
		get {
			switch(_anchor) {
			case TextAnchor.LowerCenter:
				return GoogleGravity.BOTTOM | GoogleGravity.CENTER;
			case TextAnchor.LowerLeft:
				return GoogleGravity.BOTTOM | GoogleGravity.LEFT;
			case TextAnchor.LowerRight:
				return GoogleGravity.BOTTOM | GoogleGravity.RIGHT;
				
			case TextAnchor.MiddleCenter:
				return GoogleGravity.CENTER;
			case TextAnchor.MiddleLeft:
				return GoogleGravity.CENTER | GoogleGravity.LEFT;
			case TextAnchor.MiddleRight:
				return GoogleGravity.CENTER | GoogleGravity.RIGHT;
				
			case TextAnchor.UpperCenter:
				return GoogleGravity.TOP | GoogleGravity.CENTER;
			case TextAnchor.UpperLeft:
				return GoogleGravity.TOP | GoogleGravity.LEFT;
			case TextAnchor.UpperRight:
				return GoogleGravity.TOP | GoogleGravity.RIGHT;
			}
			
			return GoogleGravity.TOP;
		}
	}
	
	//--------------------------------------
	//  Actions
	//--------------------------------------
	
	
	public Action<GoogleMobileAdBanner> OnLoadedAction {
		get {
			return _OnLoadedAction;
		}
		set {
			_OnLoadedAction = value;
		}
	}
	
	public Action<GoogleMobileAdBanner> OnFailedLoadingAction {
		get {
			return _OnFailedLoadingAction;
		}
		set {
			_OnFailedLoadingAction = value;
		}
	}
	
	public Action<GoogleMobileAdBanner> OnOpenedAction {
		get {
			return _OnOpenedAction;
		}
		set {
			_OnOpenedAction = value;
		}
	}
	
	public Action<GoogleMobileAdBanner> OnClosedAction {
		get {
			return _OnClosedAction;
		}
		set {
			_OnClosedAction = value;
		}
	}
	
	public Action<GoogleMobileAdBanner> OnLeftApplicationAction {
		get {
			return _OnLeftApplicationAction;
		}
		set {
			_OnLeftApplicationAction = value;
		}
	}
	
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	
	
	
	public void OnBannerAdLoaded()  {
		if(destroyOnLoad) {
			IOSAdMobController.instance.DirectBannerDestory(id);
			return;
		}

		_IsLoaded = true;
		if(ShowOnLoad && firstLoad) {
			Show();
			firstLoad = false;
		}

		_OnLoadedAction(this);
	}
	
	public void OnBannerAdFailedToLoad() {
		_OnFailedLoadingAction(this);
	}
	
	public void OnBannerAdOpened() {
		_OnOpenedAction(this);
	}
	
	public void OnBannerAdClosed() {
		_OnClosedAction(this);
	}
	
	public void OnBannerAdLeftApplication() {
		_OnLeftApplicationAction(this);
	}
}

