using UnityEngine;
using System;
using System.Collections;

public class WSAMultimediaManager {
#if UNITY_WSA
	public static Action<WSAImagePickedResult> OnImagePickedAction = delegate {};
	public static Action<WSANative.OnImageSavedResult> OnImageSavedAction = delegate {};
#endif

	public static void PickImageFromGallery() {
#if UNITY_WSA
		WSANative.ImagePicker.OnImagePicked += OnImagePickedCallback;
		WSANative.ImagePicker.PickImageFromGallery();
#endif
	}

#if UNITY_WSA
	private static void OnImagePickedCallback (WSANative.OnImagePickedResult result) {
		WSANative.ImagePicker.OnImagePicked -= OnImagePickedCallback;

		WSAThreadHelper.QueueOnMainThread(() => {
			WSAImagePickedResult res = new WSAImagePickedResult(result);
			OnImagePickedAction(res);
		});
	}
#endif

	public static void SaveScreenshot() {
		SA_ScreenShotMaker.instance.OnScreenshotReady += ScreenshotReady;
		SA_ScreenShotMaker.instance.GetScreenshot();
	}

#if UNITY_WSA
	private static void OnImageSavedCallback (WSANative.OnImageSavedResult result) {
		Debug.Log("[OnImageSavedCallback]");
		WSANative.ImagePicker.OnImageSaved -= OnImageSavedCallback;
		OnImageSavedAction(result);
	}
#endif

	private static void ScreenshotReady(Texture2D screenshot) {
		Debug.Log("[ScreenshotReady]");
		SA_ScreenShotMaker.instance.OnScreenshotReady -= ScreenshotReady;

#if UNITY_WSA
		WSANative.ImagePicker.OnImageSaved += OnImageSavedCallback;
		WSANative.ImagePicker.SaveImageToGallery("screenshot007.jpg", screenshot.EncodeToJPG(100));
#endif
	}

}
