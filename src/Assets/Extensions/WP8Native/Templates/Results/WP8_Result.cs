using UnityEngine;
using System.Collections;

public class WP8_Result  {
	
	protected bool _IsSucceeded = true;

	
	public WP8_Result(bool IsResultSucceeded) {
		_IsSucceeded = IsResultSucceeded;
	}
	
	public bool IsSucceeded {
		get {
			return _IsSucceeded;
		}
	}
	
	public bool IsFailed {
		get {
			return !_IsSucceeded;
		}
	}
}
