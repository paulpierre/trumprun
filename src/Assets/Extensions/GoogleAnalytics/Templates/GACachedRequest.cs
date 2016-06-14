using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GACachedRequest  {

	private long  _TimeCreated;
	private string _RequestBody;


	public GACachedRequest() {

	}

	public GACachedRequest(string body, long ticks)   {
		_RequestBody = body;
		_TimeCreated = ticks;
	}
	
	public long TimeCreated {
		get {
			return _TimeCreated;
		} 

		set {
			_TimeCreated = value;
		}
	}

	public string RequestBody {
		get {
			return _RequestBody;
		}

		set {
			_RequestBody = value;
		}
	}

	public string Delay {
		get {
			System.DateTime CreatedTime = new System.DateTime(TimeCreated);
			double ms = System.DateTime.Now.Subtract(CreatedTime).TotalMilliseconds;

			long LongRep = System.Convert.ToInt64(ms);
			return LongRep.ToString();
		}
	}

	public List<string> DataForJSON {
		get {
			List<string> l = new List<string>();
			l.Add(RequestBody);
			l.Add(TimeCreated.ToString());

			return l;
		}

	
	}
}
