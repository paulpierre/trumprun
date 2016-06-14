using UnityEngine;
using System.Collections;

public class WSAImagePickedResult {
#if UNITY_WSA
	private WSANative.OnImagePickedResult _result;

	private Texture2D _image = null;

	private WSAImagePickedResult() {}

	public WSAImagePickedResult(WSANative.OnImagePickedResult result) {
		_result = result;
	}

	public WSANative.ImagePickedResult Code {
		get {
			return _result.Code;
		}
	}

	public string Error {
		get {
			return _result.Error;
		}
	}

	public Texture2D Image {
		get {
			WSAThreadHelper.QueueOnMainThread(() => {
				_image = new Texture2D(1, 1, TextureFormat.DXT5, false);
				_image.LoadImage(_result.ImageData);
			});
			return _image;
		}
	}
#endif
}
