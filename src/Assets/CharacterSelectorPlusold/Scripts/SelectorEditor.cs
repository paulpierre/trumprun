using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ExecuteInEditMode]
public class SelectorEditor : MonoBehaviour
{

    public enum SelectorType
    {
        Circular, Linear
    }
   
    public Camera MenuCamera;
    [Tooltip("The camera distance from the objects")]
    public float CameraOffset = 2f;
    [Tooltip("The camera position offset")]
    public Vector3 CameraPosition;
    [Tooltip("The camera rotation offset")]
    public Vector3 CameraRotation;

    public SelectorType Type;
    [HideInInspector]
    public float Radius = 3f;
    [HideInInspector]
    public float LinearX, LinearY,LinearZ;

    [HideInInspector]
    public Vector3 LinearVector;
    [HideInInspector]
    public Transform[] Items;
    float angle;


    //-----------------------------------Start----------------------------------
    void Start()
    {
        storageInArray();
    }


    //-----------------------------------Update----------------------------------
    void Update()
    {
        LinearVector = new Vector3(LinearX, LinearY, LinearZ);
        ItemsLocation();
        SetCamera();

        if (Items.Length!=transform.childCount)
        {
            storageInArray();
            AddItemScript();
        }

        //Check if there was any change in the array
        int i = 0;
        foreach (Transform child in transform)
        {
            if (child != Items[i])
            {
                storageInArray();
                break;
            }
        }
    }


    //-----------------------------------Set the Camera Position----------------------------------
    public void SetCamera()
    {
        if (MenuCamera)
        {
            MenuCamera.transform.rotation = Quaternion.Euler(0, -90, 0);
            MenuCamera.transform.position = new Vector3(CameraOffset, transform.position.y, 0) + CameraPosition;
            MenuCamera.transform.eulerAngles += (CameraRotation);
        }
    }

    //--------------------------------------------Reset All childs Rotation to 0,0,0--------------------------------------------
    public void ResetItemRotation()
    {
        foreach (Transform child in transform)
            child.localRotation = new Quaternion();
    }

    //--------------------------------------------All childs Look at center--------------------------------------------
    public void LookAtRotation()
    {
        if (Type == SelectorType.Circular)
        {

            foreach (Transform child in transform)
            {
                child.transform.LookAt(transform, Vector3.up);
                child.Rotate(0, 180, 0);
            }
        }
    }

    // -----------------------------------Store in an  Array all the childrens--------------------------------------------
    public void storageInArray()
    {
        Items = new Transform[transform.childCount];
        //Filling the array
        int i = 0;
        foreach (Transform child in transform)
        {
            Items[i] = child;
            i++;
        }
    }

    //----------------------------------------------------Linear Vector Updated--------------------------------------------------
    public void UpdateLinearVector(Vector3 vect)
    {
        LinearVector = vect;
    }
    //----------------------------------------------Positions all items in a Circular/Linear path---------------------------------
    public void ItemsLocation()
    {

        if (transform.childCount != 0)
        {
            angle = 360 / transform.childCount;
        }
        int i = 0;

        foreach (Transform child in transform)
        {
            Vector3 posItem;

            if (Type == SelectorType.Circular)   //-----------------------Circular Selector-----------------------------------------------------------------
            {

                posItem = new Vector3(Mathf.Cos(angle * i * Mathf.PI / 180) * Radius, 0, Mathf.Sin(angle * i * Mathf.PI / 180) * Radius);
            }
            else  //--------------------------------Linear Selector------------------------------------------------------------
            {
                // posItem = new Vector3(0, 0, Radius * i / 2);
                posItem = LinearVector * (Radius * i / 2);
            }


            //Get the center of the item
            
            Vector3 center = child.transform.position;
            if (child.GetComponent<MeshRenderer>())
            {
                center = child.GetComponent<MeshRenderer>().bounds.center;
            }
                //Fix the item position does not have the same center and pivot position
                if (child.transform.position != center)
                    child.transform.position = posItem + (child.transform.position - center);

                else
                    child.transform.position = posItem;

                i++;
        }
    }

    //------------------------------------------------ Add ItemsManager to all Childs----------------------------------------------
    public void AddItemScript()
    {
        foreach (Transform child in transform)
        {

            //Add ItemsManagerScript
            if (!child.GetComponent<ItemsManager>())
            {
                child.gameObject.AddComponent<ItemsManager>();
            }

            //Add Collider if is a nested child
            if (child.childCount>0)
            {
                foreach (Transform grandchild in child)
                {
                    if (grandchild.GetComponent<Renderer>() && !grandchild.GetComponent<Collider>())
                    {
                            grandchild.gameObject.AddComponent<BoxCollider>();
                    }
                }
            }
            //Add Collider to the child
            else
            {
                if (!child.GetComponent<Collider>())
                {
                    child.gameObject.AddComponent<BoxCollider>();
                }
            }
           
        }
    }
}
