using UnityEngine;
using System.Collections;

public class UM_PushRegistrationResult : UM_BaseResult {

	private string _deviceId;
	
	public UM_PushRegistrationResult(string id, bool res) {
		_deviceId = id;
		_IsSucceeded = res;
	}
	
	public string deviceId {
		get {
			return _deviceId;
		}
	}
}
