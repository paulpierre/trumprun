using UnityEngine;
using System.Collections;

public class UM_ImageSaveResult : UM_BaseResult {

	private string _imagePath;

	public UM_ImageSaveResult(string path, bool res) {
		_imagePath = path;
		_IsSucceeded = res;
	}

	public string imagePath {
		get {
			return _imagePath;
		}
	}
}
