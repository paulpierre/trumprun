using UnityEngine;
using System;
using System.Collections;

public class AndroidSocialGate : SA_Singleton<AndroidSocialGate> {

	public static event Action<bool, string> OnShareIntentCallback = delegate {};

	public static void StartGooglePlusShare(string text, Texture2D texture = null) {
		AN_SocialSharingProxy.StartGooglePlusShareIntent(text, texture == null ? string.Empty : System.Convert.ToBase64String(texture.EncodeToPNG()));
	}

	public static void StartShareIntent(string caption, string message, string packageNamePattern = "") {
		StartShareIntentWithSubject(caption, message, "", packageNamePattern);
	}

	public static void StartShareIntent(string caption, string message, Texture2D texture,  string packageNamePattern = "") {
		StartShareIntentWithSubject(caption, message, "", texture, packageNamePattern);
	}

	public static void StartShareIntentWithSubject(string caption, string message, string subject, string packageNamePattern = "") {
		AN_SocialSharingProxy.StartShareIntent(caption, message, subject, packageNamePattern);
	}

	public static void StartShareIntentWithSubject(string caption, string message, string subject, Texture2D texture,  string packageNamePattern = "") {

		byte[] val = texture.EncodeToPNG();
		string bytesString = System.Convert.ToBase64String (val);

		AN_SocialSharingProxy.StartShareIntent(caption, message, subject, bytesString, packageNamePattern);
	}

	public static void SendMail(string caption, string message,  string subject, string recipients, Texture2D texture = null) {
		if(texture != null) {
			byte[] val = texture.EncodeToPNG();
			string mdeia = System.Convert.ToBase64String (val);
			AN_SocialSharingProxy.SendMailWithImage(caption, message, subject, recipients, mdeia);
		} else {
			AN_SocialSharingProxy.SendMail(caption, message, subject, recipients);
		}
	}
}

