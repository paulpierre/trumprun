////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////


using UnityEngine;
using System.Collections;

public class UM_SocialExample : MonoBehaviour {

	private GUIStyle style;
	private GUIStyle style2;

	public Texture2D textureForPost;


	void Awake() {
		InitStyles();
	}



	private void InitStyles () {
		style =  new GUIStyle();
		style.normal.textColor = Color.white;
		style.fontSize = 16;
		style.fontStyle = FontStyle.BoldAndItalic;
		style.alignment = TextAnchor.UpperLeft;
		style.wordWrap = true;
		
		
		style2 =  new GUIStyle();
		style2.normal.textColor = Color.white;
		style2.fontSize = 12;
		style2.fontStyle = FontStyle.Italic;
		style2.alignment = TextAnchor.UpperLeft;
		style2.wordWrap = true;
	}


	void OnGUI() {
		
		float StartY = 20;
		float StartX = 10;
		GUI.Label(new Rect(StartX, StartY, Screen.width, 40), "Twitter", style);
		
		StartY+= 40;
		if(GUI.Button(new Rect(StartX, StartY, 150, 50), "Post")) {
			UM_ShareUtility.TwitterShare("Titter posting test");
		}
		
		StartX += 170;
		if(GUI.Button(new Rect(StartX, StartY, 150, 50), "Post Screehshot")) {
			StartCoroutine(PostTwitterScreenshot());
		}

		
		StartY+= 80;
		StartX = 10;
		GUI.Label(new Rect(StartX, StartY, Screen.width, 40), "Facebook", style);


		StartY+= 40;
		if(GUI.Button(new Rect(StartX, StartY, 150, 50), "Post")) {
			UM_ShareUtility.FacebookShare("Facebook posting test");

		}

		StartX += 170;
		
		if(GUI.Button(new Rect(StartX, StartY, 150, 50), "Post Screehshot")) {
			StartCoroutine(PostFBScreenshot());
		}


		StartX += 170;
		
		if(GUI.Button(new Rect(StartX, StartY, 150, 50), "Post Image")) {
			UM_ShareUtility.FacebookShare("Hello world", textureForPost);
		}



		StartY+= 80;
		StartX = 10;
		GUI.Label(new Rect(StartX, StartY, Screen.width, 40), "Mail", style);
		
		
		StartY+= 40;
		if(GUI.Button(new Rect(StartX, StartY, 150, 50), "Send Mail")) {
			UM_ShareUtility.SendMail( "My E-mail Subject", "This is my text to share", "mail1@gmail.com, mail2@gmail.com");
			
		}
		
		StartX += 170;
		
		if(GUI.Button(new Rect(StartX, StartY, 150, 50), "Send Mail with image")) {
			UM_ShareUtility.SendMail( "My E-mail Subject", "This is my text to share <br> <strong> html text </strong>", "mail1@gmail.com, mail2@gmail.com", textureForPost);
		}
		



		StartY+= 80;
		StartX = 10;
		GUI.Label(new Rect(StartX, StartY, Screen.width, 40), "Native Sharing", style);
		
		
		StartY+= 40;
		if(GUI.Button(new Rect(StartX, StartY, 150, 50), "Text")) {

			UM_ShareUtility.ShareMedia("Title", "Some text to share");
		}
		
		StartX += 170;
		
		if(GUI.Button(new Rect(StartX, StartY, 150, 50), "Screehshot")) {
			StartCoroutine(PostScreenshot());
		}





	}



	private IEnumerator PostScreenshot() {
		
		yield return new WaitForEndOfFrame();
		// Create a texture the size of the screen, RGB24 format
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D( width, height, TextureFormat.RGB24, false );
		// Read screen contents into the texture
		tex.ReadPixels( new Rect(0, 0, width, height), 0, 0 );
		tex.Apply();
		
		UM_ShareUtility.ShareMedia("Title", "Some text to share", tex);
		
		Destroy(tex);
		
	}

	private IEnumerator PostTwitterScreenshot() {

		yield return new WaitForEndOfFrame();
		// Create a texture the size of the screen, RGB24 format
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D( width, height, TextureFormat.RGB24, false );
		// Read screen contents into the texture
		tex.ReadPixels( new Rect(0, 0, width, height), 0, 0 );
		tex.Apply();

		UM_ShareUtility.TwitterShare("My app ScreehShot", tex);

		
		Destroy(tex);
		
	}

	private IEnumerator PostFBScreenshot() {
		
		
		yield return new WaitForEndOfFrame();
		// Create a texture the size of the screen, RGB24 format
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D( width, height, TextureFormat.RGB24, false );
		// Read screen contents into the texture
		tex.ReadPixels( new Rect(0, 0, width, height), 0, 0 );
		tex.Apply();


		UM_ShareUtility.FacebookShare("My app ScreehShot", tex);
		
		Destroy(tex);
		
	}





}

