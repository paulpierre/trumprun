using UnityEngine;
using System.Collections;
using System;

public class UM_Player  {



	private GK_Player _GK_Player;
	private GooglePlayerTemplate _GP_Player;

	public event Action<Texture2D> BigPhotoLoaded =  delegate {};
	public event Action<Texture2D> SmallPhotoLoaded =  delegate {};

	
	//--------------------------------------
	// Init
	//--------------------------------------

	public UM_Player(GK_Player gk, GooglePlayerTemplate gp) {
		_GK_Player = gk;
		_GP_Player = gp;

		if(_GK_Player != null) {
			_GK_Player.OnPlayerPhotoLoaded += HandleOnPlayerPhotoLoaded;
		}

		if(_GP_Player != null) {
			_GP_Player.BigPhotoLoaded += HandleBigPhotoLoaded;
			_GP_Player.SmallPhotoLoaded += HandleSmallPhotoLoaded;
		}
	}




	public void LoadBigPhoto() {
		switch(Application.platform) {

		case RuntimePlatform.Android:
			_GP_Player.LoadImage();
			break;
		case RuntimePlatform.IPhonePlayer:
			_GK_Player.LoadPhoto(GK_PhotoSize.GKPhotoSizeNormal);
			break;
		}
	}


	public void LoadSmallPhoto() {
		switch(Application.platform) {
			
		case RuntimePlatform.Android:
			_GP_Player.LoadIcon();
			break;
		case RuntimePlatform.IPhonePlayer:
			_GK_Player.LoadPhoto(GK_PhotoSize.GKPhotoSizeSmall);
			break;
			
		}
	}
	
	//--------------------------------------
	// Get / Set
	//--------------------------------------



	public string PlayerId {
		get {
			switch(Application.platform) {

			case RuntimePlatform.Android:
				return _GP_Player.playerId;
			case RuntimePlatform.IPhonePlayer:
				return _GK_Player.Id;

			}

			return string.Empty;
		}
	}


	public string Name {
		get {
			switch(Application.platform) {
				
			case RuntimePlatform.Android:
				return _GP_Player.name;
			case RuntimePlatform.IPhonePlayer:
				return _GK_Player.Alias;
			}

			return string.Empty;
		}
	}

	[System.Obsolete("Avatar is deprectaed, plase use SmallPhoto instead")]
	public Texture2D Avatar {
		get {
			switch(Application.platform) {
				
			case RuntimePlatform.Android:
				return _GP_Player.image;
			case RuntimePlatform.IPhonePlayer:
				return _GK_Player.SmallPhoto;
			}

			return null;
		}
	}

	public Texture2D SmallPhoto {
		get {
			switch(Application.platform) {
				
			case RuntimePlatform.Android:
				return _GP_Player.icon;
			case RuntimePlatform.IPhonePlayer:
				return _GK_Player.SmallPhoto;
			}
			
			return null;
		}
	}

	public Texture2D BigPhoto {
		get {
			switch(Application.platform) {
				
			case RuntimePlatform.Android:
				return _GP_Player.image;
			case RuntimePlatform.IPhonePlayer:
				return _GK_Player.BigPhoto;
			}
			
			return null;
		}
	}


	public GK_Player GameCenterPlayer {
		get {
			return _GK_Player;
		}
	}
	
	
	
	public GooglePlayerTemplate GooglePlayPlayer {
		get {
			return _GP_Player;
		}
	}


	//--------------------------------------
	// Android Events
	//--------------------------------------

	void HandleSmallPhotoLoaded (Texture2D tex) {
		SmallPhotoLoaded(tex);
	}
	
	void HandleBigPhotoLoaded (Texture2D tex) {
		BigPhotoLoaded(tex);
	}

	//--------------------------------------
	// IOS Events
	//--------------------------------------

	void HandleOnPlayerPhotoLoaded (GK_UserPhotoLoadResult res) {
		if(res.IsSucceeded)  {
			if(res.Size == GK_PhotoSize.GKPhotoSizeSmall) {
				SmallPhotoLoaded(res.Photo);
			} else {
				BigPhotoLoaded(res.Photo);
			}
		}
	}


} 
