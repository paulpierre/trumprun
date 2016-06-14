/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using UnityEngine;
#if (!UNITY_4_6 && !UNITY_4_7 && !UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2)
using UnityEngine.SceneManagement;
#endif
	
/// <summary>
/// simple script will load the assigned scene
/// </summary>
public class UIButtonScene : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
		{
			#if (UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2)
				Application.LoadLevel(sceneName);
			#else
				SceneManager.LoadScene(sceneName);
			#endif
		}
    }
}
