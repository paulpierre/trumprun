using UnityEngine;
using System;
using System.Collections;

public class InitAndroidInventoryTask : MonoBehaviour {

	public event Action ActionComplete = delegate{};
	public event Action ActionFailed = delegate{};


	public static InitAndroidInventoryTask Create() {
		return new GameObject("InitAndroidInventoryTask").AddComponent<InitAndroidInventoryTask>();
	}

	public void Run() {

		Debug.Log("InitAndroidInventoryTask task started");
		if(AndroidInAppPurchaseManager.Instance.IsConnected) {
			OnBillingConnected(null);
		} else {
			AndroidInAppPurchaseManager.ActionBillingSetupFinished += OnBillingConnected;
			if(!AndroidInAppPurchaseManager.Instance.IsConnectingToServiceInProcess) {
				AndroidInAppPurchaseManager.Instance.LoadStore();
			}
		}
	}



	private void OnBillingConnected(BillingResult result) {
		Debug.Log("OnBillingConnected");
		if(result == null) {
			OnBillingConnectFinished();
			return;
		}


		AndroidInAppPurchaseManager.ActionBillingSetupFinished -= OnBillingConnected;
		
		
		if(result.isSuccess) {
			OnBillingConnectFinished();
		}  else {
			Debug.Log("OnBillingConnected Failed");
			ActionFailed();
		}

	}

	private void OnBillingConnectFinished() {
		Debug.Log("OnBillingConnected COMPLETE");
		//Store connection is Successful. Next we loading product and customer purchasing details

		if(AndroidInAppPurchaseManager.instance.IsInventoryLoaded) {
			Debug.Log("IsInventoryLoaded COMPLETE");
			ActionComplete();
		} else {
			AndroidInAppPurchaseManager.ActionRetrieveProducsFinished += OnRetrieveProductsFinised;
			if(!AndroidInAppPurchaseManager.Instance.IsProductRetrievingInProcess) {
				AndroidInAppPurchaseManager.Instance.RetrieveProducDetails();
			}
		}

	}


	private void OnRetrieveProductsFinised(BillingResult result) {
		Debug.Log("OnRetrieveProductsFinised");

		AndroidInAppPurchaseManager.ActionRetrieveProducsFinished -= OnRetrieveProductsFinised;
		
		if(result.isSuccess) {
			Debug.Log("OnRetrieveProductsFinised COMPLETE");
			ActionComplete();
		} else {
			Debug.Log("OnRetrieveProductsFinised FAILED");
			ActionFailed();
		}
	}






}
