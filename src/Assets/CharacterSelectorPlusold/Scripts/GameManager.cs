using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

    public int Level;
	public GameObject purchase;
	public GameObject select;

    [Tooltip("The selected object position in the main Screen")]
    public Transform PosInMainScreen;
	public int Coins = PlayerPrefs.GetInt("coins");
    public int CoinsSpent = 0;
    public Transform ItemSelected;
    public bool TurnTable = false;
    public float Speed = 10f;
    void Start()
    {

		Coins = PlayerPrefs.GetInt("coins");

        if (transform.childCount>0)
        {
            SelectedCharacter(transform.GetChild(0));
        }

    }

    void Update()
    {
        RotateCurrentItem();
    }


    // --------------------------------------Purschase--------------------------------------------
    public void Purchase(int amount)
    {
        if (Coins - amount >= 0)
        {
            Coins -= amount;
			PlayerPrefs.SetInt ("coins", Coins);
            CoinsSpent += amount;
        }
    }

// --------------------------------------Reset Purschase--------------------------------------------
    public void ResetPurshase()
    {
        Coins += CoinsSpent;
        CoinsSpent = 0;
    }


    // --------------------------------------Remove Current Child and Add the selected One--------------------------------------------
    public void SelectedCharacter(Transform Item)
    {


        if (transform.childCount > 0)
        {
            transform.GetChild(0).GetComponent<ItemsManager>().DestroyItem();
        }
        ItemSelected = Instantiate(Item);
        ItemSelected.parent = transform;
        ItemSelected.position = PosInMainScreen.position;
        ItemSelected.rotation = PosInMainScreen.rotation;
        Vector3 center = ItemSelected.GetComponent<ItemsManager>().ItemMesh().bounds.center;

        if (ItemSelected.position != center)
            ItemSelected.position = PosInMainScreen.position + (ItemSelected.position - center);
            
    }
    
    
    // --------------------------------------Remove Current Child and Add the selected One--------------------------------------------
    public void Children(bool state)
    {
        if (transform.childCount > 0)
            transform.GetChild(0).gameObject.SetActive(state);
    }

    // --------------------------------------Remove Current Child and Add the selected One--------------------------------------------
    void RotateCurrentItem()
    {
        if (transform.childCount > 0)
        {
            Renderer mesh = ItemSelected.GetComponent<ItemsManager>().ItemMesh();
            if (mesh && TurnTable)
            {
                ItemSelected.RotateAround(mesh.bounds.center, Vector3.up,Speed * Time.deltaTime * 10); //rotate by center
            }
        }
     
    }


    // Change Scene

    public void ChangeToScene(string SceneName)
    {
        Application.LoadLevel(SceneName);
    }

}
