using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SIS
{

public class menuC : MonoBehaviour {

	public GameObject iapMenu;
	public GameObject mainMenu;
	public GameObject charSelector;
		public Text emails;

	// Use this for initialization
	void Start () {


	
	}

	public void enableIAP(){
		iapMenu.SetActive (true);
		mainMenu.SetActive (false);

	}

	public void enableMainMenu()
	{
		iapMenu.SetActive (false);
		mainMenu.SetActive (true);
		charSelector.SetActive (true);

		
	}

	public void enableChar()
	{
		charSelector.SetActive (true);
		mainMenu.SetActive (false);
		//disable main....

	}

	
	// Update is called once per frame
	void Update () {

			int email = PlayerPrefs.GetInt ("coins");
			DBManager.SetFunds ("coins", email);
			emails.text = email.ToString ();
	
	}
}
}
