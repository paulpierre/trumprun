using UnityEngine;
using System.Collections;

[System.Serializable]
public class UM_Achievement  {

	//Editor Only
	public bool IsOpen = true;

	[SerializeField]
	public string id = "new achievment";

	[SerializeField]
	private int _Steps = 1; // defaults to 1 for standard (non-incremental) achievements

	[SerializeField]
	private string _Description = string.Empty;

	[SerializeField]
	private Texture2D _Texture;

	public bool IsIncremental;
	public string IOSId = string.Empty;
	public string AndroidId = string.Empty;

	public string Description {
		get {
			return _Description;
		}
		set {
			_Description = value;
		}
	}
	
	public Texture2D Texture {
		get {
			return _Texture;
		}
		set {
			_Texture = value;
		}
	}

	public int Steps {
		get {
			return _Steps;
		}
		set {
			_Steps = value;
		}
	}
}
