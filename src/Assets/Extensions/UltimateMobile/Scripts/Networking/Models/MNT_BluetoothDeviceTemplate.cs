using UnityEngine;
using System.Collections;

public class MNT_BluetoothDeviceTemplate {
	private string _name = string.Empty;
	private string _address = string.Empty;

	public MNT_BluetoothDeviceTemplate(string name, string address) {
		_name = name;
		_address = address;
	}

	public string Name {
		get {
			return _name;
		}
	}

	public string Address {
		get {
			return _address;
		}
	}
}
