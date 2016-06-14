using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;


[RequireComponent(typeof(SelectorEditor))]
public class SelectorManager : MonoBehaviour {

    #region Variables
    public enum ActionOnCLick
    {
        ChangeMaterial, PlayAnimation, SelectSelected
    }
    
    #region Public Variables
    public GameManager GlobalVariables;


    public Transform Menu;
    public ActionOnCLick Onclick;

    public float 
        SelectionSpeed, 
        DragSpeed, 
        RotationItem;

    public int FirstItem = 1;
    public Material LockMaterial;
    public bool 
        FrameCamera,
        RotateItem;
    public Vector3 RotationVector;
    public string AnimToPlay;
    #endregion
   
    #region Reference Variables
    private SelectorEditor circleS;
    Vector3 linear;
    private Transform[] Items;
    private int IndexSelected = 0;
    private Text TextSelectedName;
    private Text ItemData;
    private Text TextLevel, TextTotalCoins;
    private Camera cam;
    private bool pressed;
    #endregion
   
    #region MouseVariables
    private Vector3 TransformStartRot, TransformStartPos, MouseStartPos;
    float rot;
    Vector2 Deltamouse;
    #endregion

    float angle,distance;
    Vector3 InitialPosCam; // Initial Camera Position where the CircleEditor edited it
    ItemsManager current;
    #endregion

    void Start()
    {
        circleS = GetComponent<SelectorEditor>();
        Items = circleS.Items;
        linear = circleS.LinearVector;
       

        //get the angle between Items
        if (transform.childCount > 0)
        {
            angle = 360 / transform.childCount;
        }
        //get the distance between Items
        distance = circleS.Radius / 2;


        //Reference The text in the UI
        if (GameObject.Find("TextItemSelected").GetComponent<Text>() && GameObject.Find("ItemData").GetComponent<Text>())
        {
            TextSelectedName = GameObject.Find("TextItemSelected").GetComponent<Text>();
            ItemData = GameObject.Find("ItemData").GetComponent<Text>();
            TextTotalCoins = GameObject.Find("TotalCoins").GetComponent<Text>();
       //     TextLevel = GameObject.Find("TextLevel").GetComponent<Text>();
            UpdateValues();
            pressed = false;
        }
        else
        {
            Debug.Log("TextGUI not found");
        }

        cam = circleS.MenuCamera;  //Get the camera from the CircleEditor
        circleS.enabled = false;   //Disable Circle EditorScript
        InitialPosCam = cam.transform.position;
        CheckLockItem();


        //Snap to the first item you want to be on focus
        if (circleS.Type == SelectorEditor.SelectorType.Circular)
        {
            transform.rotation = Quaternion.Euler(0, angle * (FirstItem-1), 0); 
        }
        else
        {

            IndexSelected = (FirstItem - 1) % transform.childCount;
            transform.position = linear * distance * -IndexSelected;
        }
    }


    void FixedUpdate()
    {

    }
    //----------------------------------------Update is called once per frame-------------------------------------------------------
    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            SelectbyClick();
        }
        Selector();
        if (RotateItem)
        TurnTableItem(IndexSelected);

        CameraFix();
          
        if (Input.GetKeyDown(KeyCode.LeftArrow)) Pressed(false);
        if (Input.GetKeyDown(KeyCode.RightArrow)) Pressed(true);
        if (Input.GetKeyDown(KeyCode.UpArrow)) Pressed(true);
        if (Input.GetKeyDown(KeyCode.DownArrow)) Pressed(false);
    }

    //----------------------------------------Focus the Object with the indexSelected Value-----------------------------------------
    public void Selector()
    {
        if (pressed && !Input.GetMouseButton(0))
        {
            if (circleS.Type == SelectorEditor.SelectorType.Circular)  //-----------------------------------Circular Selector-----------------------------------------------------------
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, angle * IndexSelected, 0), Time.deltaTime * SelectionSpeed);

                if (Mathf.Abs( transform.eulerAngles.y -(angle * IndexSelected))<0.05f) //Error treshold to Snap to the correct angle
                {
                    transform.rotation = Quaternion.Euler(0, angle * IndexSelected, 0);
                    pressed = false;
                }
            }
            else   //-----------------------------------------------------------Linear Selector-----------------------------------------------------------
            {
                transform.position = Vector3.Lerp(transform.position, linear * distance * -IndexSelected, Time.fixedDeltaTime * SelectionSpeed);

                Vector3 difference = transform.position - (linear * distance * -IndexSelected);

                if (Mathf.Abs(difference.x)<0.05f && Mathf.Abs(difference.y) < 0.05f && Mathf.Abs(difference.z) < 0.05f)
                {
                    transform.position = linear * distance * -IndexSelected;
                    pressed = false;
                }
            }
        }
    }

    //----------------------------------------Gives the Lock Material---------------------------------------------------------------
    void CheckLockItem()
    {
        foreach (Transform child in transform)
        {
            ItemsManager currentitem = child.GetComponent<ItemsManager>();

          //  if (currentitem.Locked && currentitem.AvailableAtLvl > GlobalVariables.Level)
			if (currentitem.Locked)
            {
                if (currentitem.ItemMesh())
                {
                 //   currentitem.ItemMesh().material = LockMaterial;
                }
            }
            else if (!currentitem.Locked && currentitem.ItemMesh().material == LockMaterial && currentitem.Materials.Length > 0)
                currentitem.ItemMesh().material = currentitem.Materials[0];
        }
    }

    //----------------------------------------Action on Click ----------------------------------------------------------------------
    void Action()
    {
        switch (Onclick)
        {
            case ActionOnCLick.ChangeMaterial:
                SwapMaterial(true);
                break;
            case ActionOnCLick.PlayAnimation:

                if (Items[IndexSelected].GetComponent<Animator>() && !Items[IndexSelected].GetComponent<ItemsManager>().Locked)
                {
                    if (Items[IndexSelected].GetComponent<ItemsManager>().CustomAnimation != "") // Check if the object has a custom animator
                    {
                        Items[IndexSelected].GetComponent<Animator>().CrossFade(Items[IndexSelected].GetComponent<ItemsManager>().CustomAnimation,0.2f);
                    }
                    else
                    {
                        Items[IndexSelected].GetComponent<Animator>().CrossFade(AnimToPlay,0.05f);
                    }
                }
                break;
            case ActionOnCLick.SelectSelected:
                Select();
                break;
            default:
                break;
        }
    }

    //----------------------------------------Drag/Swipe with Mouse/Touch and Action by Click/Tap-----------------------------------
    public void SelectbyClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MouseStartPos = Input.mousePosition;
            TransformStartRot = transform.eulerAngles;
            TransformStartPos = transform.position;
        }
        #region DRAG OR SWIPE MOUSE OR TOUCH

        if (Input.GetMouseButton(0))
        {

            Deltamouse = MouseStartPos - Input.mousePosition;
              
            rot = Deltamouse.x * Time.fixedDeltaTime * DragSpeed;

            if (circleS.Type == SelectorEditor.SelectorType.Circular) //--------------------------------Circular Selector-----------------------------------------------------------
            {
                transform.eulerAngles = TransformStartRot + new Vector3(0, rot, 0);
            }
            else //--------------------------------------------------------Linear Selector-----------------------------------------------------------
            {
                float amount = Mathf.Max(Deltamouse.x,Deltamouse.y);
                float a= Mathf.Min(Deltamouse.x, Deltamouse.y);
                if (amount<Mathf.Abs( a))
                    amount = a;

                transform.position = TransformStartPos +  circleS.LinearVector * (-amount * DragSpeed * 0.002f);


                //Snap On the last or first item when the swipe is on the wrong direction 

               float maxDistance = -distance * (Items.Length - 1);

                  if ((transform.position.x < maxDistance * linear.x ) || 
                      ((transform.position.y < maxDistance * linear.y)) ||
                      (transform.position.z < maxDistance * linear.z))
                  {
                      transform.position = circleS.LinearVector * maxDistance;
                      IndexSelected = Items.Length-1;
                  }

                if (transform.position.x > 0 || transform.position.y > 0  || transform.position.z > 0)
                {
                    transform.position = Vector3.zero;
                    IndexSelected = 0;
                }

            }
        }
        #endregion
        if (Input.GetMouseButtonUp(0))
        {
            if (Deltamouse == Vector2.zero) // if it was a CLICK OR TOUCH----------------------------------------------
            {
               // Debug.Log("A Was a Click/Touch");
                RaycastHit hit;

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
                {
                    int i = 0;
                    int NewSelection;
                    foreach (Transform child in transform)
                    {
                        if (hit.transform == child || hit.transform.parent == child)
                        {
                            NewSelection = i;

                            // if Click the same selected item make the selected action  |ON CLICK|
                            if (IndexSelected == NewSelection)
                            {
                                Action();
                                UpdateValues();

                                if (child.GetComponent<ItemsManager>().Locked)
                                {
                                    ItemData.text = "Unlock First";
                                }
                            }
                            else
                            {
                                // If another visible item was touch;
                                IndexSelected = NewSelection;
                                UpdateValues();
                                break;
                            }
                        }
                        i++;
                    }
                    

                }
                
                else //------------------CLick Left or Right of the Screen to Change Next/Before---------------------------------
                {
                    Vector2 center = new Vector2(Screen.width / 2f, Screen.height / 2f);

               
                        if (Input.mousePosition.x < center.x)
                        {
                            IndexSelected--;
                        }
                        else
                        {
                            IndexSelected++;
                        }
                        UpdateValues();
                   
                }
                
            }

            else // if it was a swipe, average to the nearest item on the camera view-----------------------------------------------
            {
               // Debug.Log("A Was a Swipe");

                if (circleS.Type == SelectorEditor.SelectorType.Circular)//-------------Circular Selector---------------------------
                {
                    float currentangle = transform.eulerAngles.y;

                    IndexSelected = Mathf.RoundToInt(currentangle / angle) % Items.Length;

                }
                else  //------------------------Linear Selector---------------------------------------------------------------------
                {
                    float max=0f; 

                    if ((linear.x >= linear.y) && (linear.x >= linear.z))
                        max = transform.position.x / linear.x;

                    if ((linear.y >= linear.x) && (linear.y >= linear.z))
                        max = transform.position.y / linear.y;

                    if ((linear.z >= linear.x) && (linear.z >= linear.y))
                        max = transform.position.z / linear.z;

                    
                    if (Mathf.Abs(max) > 0)
                    {
                        IndexSelected = Mathf.RoundToInt(Mathf.Abs(max) / distance) % Items.Length;
                    }
                }
                UpdateValues();
            }
        }
    }

    //----------------------------------------Update GUI, Variables-----------------------------------------------------------------
    void UpdateValues()
    {

		Items[IndexSelected].GetComponent<Animator>().CrossFade(AnimToPlay,0.05f);
		ItemData.text = "";
        pressed = true;
        if (IndexSelected>=Items.Length)IndexSelected = 0;
        if (IndexSelected< 0) IndexSelected = Items.Length-1;

        TextSelectedName.text = Items[IndexSelected].name;
      //  TextLevel.text = "LVL " + GlobalVariables.Level.ToString();
        TextTotalCoins.text = GlobalVariables.Coins.ToString();


        current = circleS.Items[IndexSelected].GetComponent<ItemsManager>();


		if (PlayerPrefs.HasKey (current.name) || !isLocked() ) {

			//current.Locked = false;
			current.SetNewMaterial (true);
			GlobalVariables.purchase.SetActive (false);
			GlobalVariables.select.SetActive (true);

		} 
	
	else {
			ItemData.text = "5,000 e-mails to unlock";
			GlobalVariables.purchase.SetActive (true);
			GlobalVariables.select.SetActive (false);

			Debug.LogError (" locked ", null);
		} 
		/*
		 * else {
			GlobalVariables.purchase.SetActive (false);
			GlobalVariables.select.SetActive (true);
		}
         */

			

		/*
        if (GlobalVariables.Level<current.AvailableAtLvl && current.Locked)
        {
            ItemData.text = "Available at level " + current.AvailableAtLvl.ToString() + "    $"+ current.value.ToString();
        }
        else 
        {
            ItemData.text = current.ItemData;
        }*/
    }

    //----------------------------------------TurnTable of the item on display------------------------------------------------------
    void TurnTableItem(int index)
    {
        Renderer mesh = Items[index].GetComponent<ItemsManager>().ItemMesh();
        if (mesh)
            Items[index].transform.RotateAround(mesh.bounds.center, RotationVector, RotationItem * Time.deltaTime * 10); //rotate by center
      //  else
        //    Items[index].transform.Rotate(RotationVector, RotationItem * Time.deltaTime * 10);  //rotate by pivot
    }

    //----------------------------------------Check if the current item is locked---------------------------------------------------
    bool isLocked()
    {
        return Items[IndexSelected].GetComponent<ItemsManager>().Locked;
    }
        
    //----------------------------------------change to the before/next item--------------------------------------------------------
    public void Pressed(bool next)
    {
        if (next)
        {
            IndexSelected++;
        }
        else
        {
            IndexSelected--;
        }
     UpdateValues();
    }

    //-----------------------------------------Change the material on the selected Item---------------------------------------------
   public void SwapMaterial(bool up)
    {
        if (isLocked() == false)
        Items[IndexSelected].GetComponent<ItemsManager>().SetNewMaterial(up);
    }

    //-----------------------------------------Move camera near or far depending the Size of the selected item--------------------
    public void CameraFix()
    {
        if (FrameCamera)
        {
            Vector3 FixPos = new Vector3((InitialPosCam.x) + (Items[0].GetComponent<ItemsManager>().BoundingBox().y- current.BoundingBox().y) * -1.5f, InitialPosCam.y, InitialPosCam.z);

            cam.transform.position = Vector3.Lerp(cam.transform.position, FixPos, Time.deltaTime * 5);
        }
    }

    //-----------------------------------------Buy an item---------------------------------------------------------------------------
    public void Purchase()
    {
		int currentcoins = PlayerPrefs.GetInt ("coins");
		GlobalVariables.Coins = currentcoins;
        ItemsManager current = circleS.Items[IndexSelected].GetComponent<ItemsManager>();

        if (GlobalVariables.Coins >= current.value && current.Locked)
        {
			Debug.Log ("something");
            current.Locked = false;
            current.SetNewMaterial(true);
            GlobalVariables.Purchase(current.value);
            UpdateValues();
			PlayerPrefs.SetInt (current.name, 1);
        }
        else if (GlobalVariables.Coins >= current.value && !current.Locked)
        {
			ItemData.text = "Already Owned";
        }
        else
        {
            ItemData.text = "Not enough e-mails";
       } 
    }

    //-----------------------------------------Reset the purchase--------------------------------------------------------------------
    public void ResetPurchase()
    {
		/*
        GlobalVariables.ResetPurshase();

        foreach (Transform child in transform)
        {
            ItemsManager currentitem = child.GetComponent<ItemsManager>();
          
			if (currentitem.AvailableAtLvl>GlobalVariables.Level)
            {
                currentitem.ItemMesh().material = LockMaterial;
                currentitem.Locked = true;
            }
        }
        UpdateValues();

       */
    }

    //-----------------------------------------Select the current item if is unlocked------------------------------------------------
    public void Select()
    {
        
		if (!isLocked() || PlayerPrefs.HasKey(current.name))
        {



			//PlayerPrefs.SetString ("selectedChar", "Trump");



			///Application.LoadLevel ("3D Infinite Runner");
			GlobalVariables.ItemSelected = Items[IndexSelected];

			if (GlobalVariables.ItemSelected.CompareTag ("Trump")) {

				Debug.LogError ("load trump", null);

				ChangeToScene ("Trump");

			}

			if (GlobalVariables.ItemSelected.CompareTag ("Bernie")) {

				Debug.LogError ("load Bernie", null);

				ChangeToScene ("Bernie");

			}

			if (GlobalVariables.ItemSelected.CompareTag ("Hillary")) {

				Debug.LogError ("load Hillary", null);

				ChangeToScene ("Hillary");

			}

	
				
            //GlobalVariables.SelectedCharacter(Items[IndexSelected]);
           
            //gameObject.SetActive(false);

            //Find the Menu selector and Hide it
            //GameObject.Find("CanvasSelector").SetActive(false);

            //Find the Main menu and Show it /referenced
            //Menu.gameObject.SetActive(true);
        }
        else
        {
            ItemData.text = "Unlock first";
        }
       
    }

	// Change Scene

	public void ChangeToScene(string SceneName)
	{
		Application.LoadLevel(SceneName);
	}
   
}
