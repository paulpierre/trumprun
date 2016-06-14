using UnityEngine;
using System.Collections;

public class UM_Error  {

	protected int _Code;
	protected string _Description;


	public UM_Error(int code, string description) {
		_Code = code;
		_Description = description;
	} 
	

	public int Code {
		get {
			return _Code;
		}
	}
	
	public string Description {
		get {
			return _Description;
		}
	}
}
