using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GoogleAnalyticsRequestCache  {

	private const string DATA_SPLITTER = "|";
	private const string RQUEST_DATA_SPLITTER = "%rps%";

	private const string GA_DATA_CACHE_KEY = "GoogleAnalyticsRequestCache";

	public static void SaveRequest(string cache) {


		GACachedRequest r = new GACachedRequest(cache, DateTime.Now.Ticks);

		List<GACachedRequest> current = CurrenCachedRequests;
		current.Add(r);

		Debug.Log(current.Count);
		CacheRequests(current);

	}

	public static void SendChashedRequests() {

		List<GACachedRequest> current = CurrenCachedRequests;
		foreach(GACachedRequest request in current) {
			string HitRequest = request.RequestBody;
			if(GoogleAnalyticsSettings.Instance.IsQueueTimeEnabled) {
				HitRequest += "&qt" + request.Delay;
				GoogleAnalytics.SendSkipCache(HitRequest);
			}

		}

			
		Clear();
	}


	public static void Clear() {
		PlayerPrefs.DeleteKey(GA_DATA_CACHE_KEY);
	}

	public static string SavedData {
		get {
			if(PlayerPrefs.HasKey(GA_DATA_CACHE_KEY)) {
				return PlayerPrefs.GetString(GA_DATA_CACHE_KEY);
			} else {
				return string.Empty;
			}
		}

		set {
			PlayerPrefs.SetString(GA_DATA_CACHE_KEY, value);
		}
	}

	public static void CacheRequests(List<GACachedRequest> requests) {
		List<List<string>> cache =  new List<List<string>>();

		foreach(GACachedRequest r  in requests) { 
			List<string> data =  new List<string>();
			data.Add(r.RequestBody);
			data.Add(r.TimeCreated.ToString());

			cache.Add(data);
		}

		SavedData = GAMiniJSON.Json.Serialize(cache);
	}

	public static List<GACachedRequest> CurrenCachedRequests {
		get {
			if(SavedData == string.Empty) {
				return new List<GACachedRequest>();
			} else {
				try {
					List<GACachedRequest> current =  new List<GACachedRequest>();
					List<object> requests  = GAMiniJSON.Json.Deserialize(SavedData) as List<object>;
					foreach(object request in requests) {

						
						List<object> dataList = request as List<object>;
						GACachedRequest r =  new GACachedRequest();
						int index = 1;
						foreach(object d in dataList) {
							string val = d as String;
							switch(index) {
							case 1:
								r.RequestBody = val;
								break;
							case 2:
								r.TimeCreated = Convert.ToInt64(val);
								break;
							}

							index++;
						}

						current.Add(r);
					}

					return current;

				} catch(Exception ex) {
					Clear();
					Debug.LogError(ex.Message);
					return new List<GACachedRequest>();
				}
			}
		}
	}


}
