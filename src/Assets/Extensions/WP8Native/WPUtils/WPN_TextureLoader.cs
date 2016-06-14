////////////////////////////////////////////////////////////////////////////////
//  
// @module Common Android Native Lib
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////


using UnityEngine;
using System;
using System.Collections;

public class WPN_TextureLoader : MonoBehaviour {

	private string _url;

	public event Action<Texture2D> TextureLoaded =  delegate {};

	public static WPN_TextureLoader Create() {
		return new GameObject("WPN_TextureLoader").AddComponent<WPN_TextureLoader>();
	}

	public void LoadTexture(string url) {
		_url = url;
		StartCoroutine(LoadCoroutin());
	}


	private IEnumerator LoadCoroutin () {
		// Start a download of the given URL
		WWW www = new WWW (_url);

		// Wait for download to complete
		yield return www;

		TextureLoaded(www.texture);

		Destroy(gameObject);
	}

}
