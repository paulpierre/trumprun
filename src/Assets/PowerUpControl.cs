using UnityEngine;
using System.Collections;

public class PowerUpControl : MonoBehaviour {
	public GameObject Magnet;

	public GameObject Shield;
	public GameObject test;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void powerupMagnet(){

		StartCoroutine(ActivationMagent());

	}
	public void powerupShield(){

		//StartCoroutine(ActivationShield());

		Shield.SetActive (true);
		test.SetActive (true);
	}

	public IEnumerator ActivationMagent()
	{   
		int mag = PlayerPrefs.GetInt ("mag");

		if (mag > 0) {
		
			Magnet.SetActive (true);

			test.SetActive (false);


			PlayerPrefs.SetInt ("mag", mag - 1);

			yield return new WaitForSeconds (14);
			Magnet.SetActive (false);
			test.SetActive (true);

		}
	}



}
