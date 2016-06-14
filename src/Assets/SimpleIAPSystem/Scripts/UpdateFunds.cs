/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SIS;

/// <summary>
/// displays currency value in a text component.
/// Also updates this value in case it changed
/// </summary>
public class UpdateFunds : MonoBehaviour
{
    /// <summary>
    /// text reference for displaying the currency value
    /// </summary>
    public Text label;

    /// <summary>
    /// name of currency to display (set via IAP Editor)
    /// </summary>
    public string currency;

    /// <summary>
    /// time for animating the current start to end value
    /// </summary>
    public float duration = 2;

    //cache current currency value for accessing it later
    private int curValue;


    void OnEnable()
    {
	    //subscribe to successful purchase/update event,
	    //it could be that the player obtained currency
        IAPManager.purchaseSucceededEvent += UpdateValue;
        DBManager.updatedDataEvent += UpdateValue;

        //update currency display right away
        if (!DBManager.GetInstance()) return;
        //get current currency value
        int funds = DBManager.GetFunds(currency);
        //display value in the UILabel
        label.text = funds.ToString();
        //store value
        curValue = funds;
    }


    void OnDisable()
    {
	    //unsubscribe from events
        IAPManager.purchaseSucceededEvent -= UpdateValue;
        DBManager.updatedDataEvent -= UpdateValue;
    }


    void UpdateValue() { UpdateValue(null); }
    void UpdateValue(string s)
    {
	    //stop existing text animation routines,
	    //we don't want to have two running at the same time
        StopCoroutine("CountTo");

	    //if this gameobject is active and visible in our GUI,
	    //start text animation to the current currency value
	    //(if it isn't active, the value will be updated in OnEnable())
        if(gameObject.activeInHierarchy)
            StartCoroutine("CountTo", DBManager.GetFunds(currency));
    }


    IEnumerator CountTo(int target)
    {
	    //remember current value as starting position
        int start = curValue;
	
	    //over the duration defined, lerp value from start to target value
	    //and set the UILabel text to this value
        for (float timer = 0; timer < duration; timer += Time.deltaTime)
        {
            float progress = timer / duration;
            curValue = (int)Mathf.Lerp(start, target, progress);
            label.text = curValue + "";
            yield return null;
        }

	    //once the duration is over, directly set the value and text
	    //to the targeted value to avoid rounding issues or inconsistency
        curValue = target;
        label.text = curValue + "";
    }
}
