using UnityEngine;
using System.Collections;

public class UM_ShareUtility : MonoBehaviour {

	public static void TwitterShare(string status) {
		TwitterShare(status, null);
	}
	
	public static void TwitterShare(string status, Texture2D texture) {
		switch(Application.platform) {
		case RuntimePlatform.Android:
			if(texture != null) {
				AndroidSocialGate.StartShareIntent("Share", status, texture, "twi");
			} else {
				AndroidSocialGate.StartShareIntent("Share", status, "twi");
			}


			break;
		case RuntimePlatform.IPhonePlayer:
			IOSSocialManager.instance.TwitterPost(status, null, texture);
			break;

		case RuntimePlatform.WP8Player:
			break;
		}
	}


	public static void InstagramShare(Texture2D texture) {
		InstagramShare(null, texture);
	}
	
	public static void InstagramShare(string status) {
		InstagramShare(status, null);
	}
	
	public static void InstagramShare(string status, Texture2D texture) {
		switch(Application.platform) {
		case RuntimePlatform.Android:
			if(texture != null) {
				AndroidSocialGate.StartShareIntent("Share", status, texture, "com.instagram.android");
			} else {
				AndroidSocialGate.StartShareIntent("Share", status, "com.instagram.android");
			}
			
			
			break;
		case RuntimePlatform.IPhonePlayer:
			
			IOSSocialManager.instance.TwitterPost(status, null, texture);
			break;
			
		case RuntimePlatform.WP8Player:
			break;
		}
	}


	
	public static void FacebookShare(string message) {
		FacebookShare(message, null);
	}
	
	public static void FacebookShare(string message, Texture2D texture) {
		switch(Application.platform) {
		case RuntimePlatform.Android:
			if(texture != null) {
				AndroidSocialGate.StartShareIntent("Share", message, texture, "facebook.katana");
			} else {
				AndroidSocialGate.StartShareIntent("Share", message, "facebook.katana");
			}

			break;
		case RuntimePlatform.IPhonePlayer:
			IOSSocialManager.instance.FacebookPost(message, null, texture);
			break;
		case RuntimePlatform.WP8Player:
			break;
		}
	}
	
	
	public static void ShareMedia(string caption, string message) {
		ShareMedia(caption, message, null);
	}
	
	public static void ShareMedia(string caption, string message, Texture2D texture) {
		switch(Application.platform) {
		case RuntimePlatform.Android:
			if(texture != null) {
				AndroidSocialGate.StartShareIntent("Share", message, texture);
			} else {
				AndroidSocialGate.StartShareIntent("Share", message);
			}

			break;
		case RuntimePlatform.IPhonePlayer:
			IOSSocialManager.instance.ShareMedia(message, texture);
			break;
		case RuntimePlatform.WP8Player:
			break;
		}
	}



	public static void SendMail(string subject, string body, string recipients) {
		SendMail(subject, body, recipients, null);
	}
	
	public static void SendMail(string subject, string body, string recipients, Texture2D texture) {
		
		switch(Application.platform) {
		case RuntimePlatform.Android:
			AndroidSocialGate.SendMail("Send Mail", body, subject, recipients, texture);
			break;
		case RuntimePlatform.IPhonePlayer:
			IOSSocialManager.instance.SendMail(subject, body, recipients, texture);
			break;
		case RuntimePlatform.WP8Player:
			break;
		}
		
	}


}
