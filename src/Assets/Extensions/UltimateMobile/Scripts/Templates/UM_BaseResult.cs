using UnityEngine;
using System.Collections;

public class UM_BaseResult  {

	protected bool _IsSucceeded = true;


	public bool IsSucceeded {
		get {
			return _IsSucceeded;
		}  
		set {
			_IsSucceeded = value;
		}

	}

	public bool IsFailed {
		get {
			return !_IsSucceeded;
		}
	}
}
