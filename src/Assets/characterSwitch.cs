using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

namespace SIS
{

public class characterSwitch : MonoBehaviour {

	public Text coins;

	//public GameObject Trump; 
	//public GameObject Bernie;

	// Use this for initialization
	void Start () {

	//	PlayerPrefs.SetInt ("ForgiveKeys", 100);
	
	/*	string player = PlayerPrefs.GetString ("selectedChar");
		if (player == "Trump") {
			//Trump.SetActive (true);
		Bernie.SetActive (true);

		} else if (player == "Bernie") {
			Bernie.SetActive (true);
		}

*/
	
			int myCoins = PlayerPrefs.GetInt("coins");
			coins.text = myCoins.ToString ();
			DBManager.SetFunds ("coins", myCoins);


	}



	public void characterSelector(){

		SceneManager.LoadScene ("Characters");
	//	Application.LoadLevel("Characters");

	}

public void updateCoins()
{

			int myCoins = PlayerPrefs.GetInt("coins");
			DBManager.SetFunds ("coins", myCoins);
			coins.text = myCoins.ToString ();


}
	
	// Update is called once per frame
	void Update () {
	
			int myCoins = PlayerPrefs.GetInt("coins");
			DBManager.SetFunds ("coins", myCoins);
			coins.text = myCoins.ToString ();

	//	Debug.LogError(" coins  "+coins, null);
	}
}

}