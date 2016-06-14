////////////////////////////////////////////////////////////////////////////////
//  
// @module Android Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////


using UnityEngine;
using System.Collections;

public class GoogleCloudUseExample : MonoBehaviour {

	public GameObject[] hideOnConnect;
	public GameObject[] showOnConnect;

	void Awake() {


		GoogleCloudManager.ActionAllStatesLoaded += OnAllLoaded;
		GoogleCloudManager.ActionStateLoaded += OnStateUpdated;
		GoogleCloudManager.ActionStateResolved += OnStateUpdated;
		GoogleCloudManager.ActionStateUpdated += OnStateUpdated;

		GoogleCloudManager.ActionStateConflict += OnStateConflict;

		GooglePlayConnection.instance.Connect ();
	}

	void FixedUpdate() {
		if(GooglePlayConnection.State == GPConnectionState.STATE_CONNECTED) {
			foreach(GameObject o in hideOnConnect) {
				o.SetActive(false);
			}

			foreach(GameObject o in showOnConnect) {
				o.SetActive(true);
			}
		} else {
			foreach(GameObject o in hideOnConnect) {
				o.SetActive(true);
			}
			
			foreach(GameObject o in showOnConnect) {
				o.SetActive(false);
			}
		}
	}


	private void LoadAllStates() {
		GoogleCloudManager.instance.loadAllStates ();
	}

	private void LoadState() {
		GoogleCloudManager.instance.loadState (GoogleCloudSlot.SLOT_0);
	}

	private void UpdateState() {
		string msg = "Hello bytes data";
		System.Text.UTF8Encoding  encoding = new System.Text.UTF8Encoding();
		byte[] data = encoding.GetBytes(msg);
		GoogleCloudManager.instance.updateState (GoogleCloudSlot.SLOT_0, data);
	}

	private void DeleteState() {
		GoogleCloudManager.instance.deleteState(GoogleCloudSlot.SLOT_0);
		GoogleCloudManager.ActionStateDeleted += OnStateDeleted;
	}



	//--------------------------------------
	//  EVENTS
	//--------------------------------------

	private void OnStateConflict(GoogleCloudResult result) {
		AN_PoupsProxy.showMessage ("OnStateUpdated", result.message 
		                           + "\n State ID: " + result.stateKey 
		                           + "\n State Data: " + result.stateData
		                           + "\n State Conflict: " + result.serverConflictData
		                           + "\n State resolve: " + result.resolvedVersion);

		//Resolving conflict with our local data
		//you should create your own resolve logic for your game. Read more about resolving conflict on Android developer website

		GoogleCloudManager.instance.resolveState (result.stateKey, result.stateData, result.resolvedVersion);
	}



	private void OnStateUpdated(GoogleCloudResult result) {


		AN_PoupsProxy.showMessage ("State Updated", result.message + "\n State ID: " + result.stateKey + "\n State Data: " + result.stateDataString);
	}


	private void OnAllLoaded(GoogleCloudResult result) {
		AN_PoupsProxy.showMessage ("All States Loaded", result.message + "\n" + "Total states: " + GoogleCloudManager.instance.states.Count);
	}

	private void OnStateDeleted(GoogleCloudResult result) {
		AN_PoupsProxy.showMessage ("KeyDeleted", result.message + "\n for state key: " + result.stateKey.ToString());
	}

	
}
