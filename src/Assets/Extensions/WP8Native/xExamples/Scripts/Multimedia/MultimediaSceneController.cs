using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MultimediaSceneController : MonoBehaviour {

	public Image SampleImage;

	public Texture2D SampleTexture;

	void Start(){
		SampleImage.sprite = Sprite.Create(SampleTexture, new Rect(0.0f, 0.0f, SampleTexture.width, SampleTexture.height), new Vector2(0.5f, 0.5f));
	}

	public void PickImage() {
#if UNITY_WSA
		WSAMultimediaManager.OnImagePickedAction += ImagePickedCallback;
		WSAMultimediaManager.PickImageFromGallery();
#endif
	}

#if UNITY_WSA
	private void ImagePickedCallback(WSAImagePickedResult result) {
		WSAMultimediaManager.OnImagePickedAction -= ImagePickedCallback;

		if (result.Code == WSANative.ImagePickedResult.Success) {
			WSAThreadHelper.QueueOnMainThread(() => {
				Debug.Log("ImagePickedCallback " + result.Code + " " + result.Error + " " + result.Image);
				SampleImage.sprite = Sprite.Create(result.Image, new Rect(0.0f, 0.0f, result.Image.width, result.Image.height), new Vector2(0.5f, 0.5f));
			});
		} else {
			Debug.Log("ImagePickedCallback " + result.Code + " " + result.Error);
		}
	}
#endif

	public void SaveScreenshot() {
#if UNITY_WSA
		WSAMultimediaManager.OnImageSavedAction += ImageSavedCallback;
		WSAMultimediaManager.SaveScreenshot();
#endif
	}

#if UNITY_WSA
	private void ImageSavedCallback(WSANative.OnImageSavedResult result) {
		Debug.Log("[ImageSavedCallback] " + result.Code + " " + result.Error);
	}
#endif
}
