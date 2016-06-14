using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemsManager : MonoBehaviour {

    public Material[] Materials;
    public bool Locked = false;
    public string ItemData;
    public int AvailableAtLvl;
    public int value;
    public string CustomAnimation;
   // public float amount; //for now not important... for next updates!!

    int materialSelected = -1;

    //Swap materials Up and Down
    public void SetNewMaterial(bool up)
    {
        
        if (Materials.Length > 0)
        {
            if (up)
            {
                materialSelected = (materialSelected + 1) % Materials.Length;
            }
            else
            {
                if (materialSelected == 0)
                {
                    materialSelected = Materials.Length;
                }
                materialSelected = (materialSelected - 1) % Materials.Length;
            }

            Renderer mesh = ItemMesh();

            if (mesh)
            {
                mesh.material = Materials[materialSelected];
            }
            else
            {
                Debug.Log("No Renderer Found");
            }
        }
    }

   public Renderer ItemMesh()
    {
        if (transform.childCount > 0)
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<Renderer>())
                {
                   return child.GetComponent<Renderer>();
                }
            }
        }
        if (GetComponent<Renderer>())
        {
           return GetComponent<Renderer>();   
        }
        return null;
    }

    // Returnds The bounds of the items
    public Vector3 BoundingBox()
    {
        if (transform.childCount > 0)
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<Collider>())
                    return child.GetComponent<Collider>().bounds.size;
            }
        }


        if (GetComponent<Collider>())
        {
            Collider col = GetComponent<Collider>();
            return col.bounds.size;
        }
        return Vector3.zero;
    }

    public void DestroyItem()
    {
        Destroy(gameObject);
    }
}
