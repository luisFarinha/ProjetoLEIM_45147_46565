using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public bool[] isFull;
    public GameObject[] slots;

    public void RemoveItems(List<string> items)
    {
        foreach (GameObject slot in slots)
        {
            foreach (string x in items)
            {                 
                if(slot.transform.childCount > 0) {
                    if (slot.transform.GetChild(0).name.Replace("(Clone)", "") == x) {
                        GameObject.Destroy(slot.transform.GetChild(0).gameObject);
                    }
                }
            }
        }
    }

}
