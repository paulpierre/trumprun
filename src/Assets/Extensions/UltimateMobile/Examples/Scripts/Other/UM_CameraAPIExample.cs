using UnityEngine;
using System.Collections;

public class UM_CameraAPIExample : BaseIOSFeaturePreview {

	public Texture2D hello_texture;
	public Texture2D darawTexgture = null;

	


	void OnGUI() {
		UpdateToStartPos();



		GUI.Label(new Rect(StartX, StartY, Screen.width, 40), "Camera And Gallery", style);
		
		StartY+= YLableStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth + 10, buttonHeight), "Save Screenshot To Camera Roll")) {
			UM_Camera.instance.OnImageSaved += OnImageSaved;
			UM_Camera.instance.SaveScreenshotToGallery();
		}


		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Save Texture To Camera Roll")) {
			UM_Camera.instance.OnImageSaved += OnImageSaved;
			UM_Camera.instance.SaveImageToGalalry(hello_texture);
		}


		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Get Image From Album")) {
			UM_Camera.instance.OnImagePicked += OnImage;
			UM_Camera.instance.GetImageFromGallery();
		}

		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Get Image From Camera")) {
			UM_Camera.instance.OnImagePicked += OnImage;
			UM_Camera.instance.GetImageFromCamera();
		}

		StartX = XStartPos;
		StartY+= YButtonStep;
		StartY+= YLableStep;
		GUI.Label(new Rect(StartX, StartY, Screen.width, 40), "PickedImage", style);
		StartY+= YLableStep;

		if(darawTexgture != null) {
			GUI.DrawTexture(new Rect(StartX, StartY, buttonWidth, buttonWidth), darawTexgture);
		}
	

	}


	void OnImageSaved (UM_ImageSaveResult result) {
		if(result.IsSucceeded) {
			//no image path for IOS
			new MobileNativeMessage("Image Saved", result.imagePath);
		} else {
			new MobileNativeMessage("Failed", "Image Save Failed");
		}

	}

	

	private void OnImage (UM_ImagePickResult result) {
		UM_Camera.instance.OnImageSaved -= OnImageSaved;
		if(result.IsSucceeded) {
			darawTexgture = result.image;
		}

		UM_Camera.instance.OnImagePicked -= OnImage;
	}
}
