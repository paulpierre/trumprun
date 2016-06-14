using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Magpower : MonoBehaviour {
	public Text mag_count;

	// Use this for initialization
	void Start () {
		int mag = PlayerPrefs.GetInt ("mag");
		mag_count.text = mag.ToString ();
	
	}
	
	// Update is called once per frame
	void Update () {

		int mag = PlayerPrefs.GetInt ("mag");
		mag_count.text = mag.ToString ();
	
	}
}
