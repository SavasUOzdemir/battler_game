using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class UI_Squad : MonoBehaviour
{
    GameObject[] items = new GameObject[4];
    int filledSlots = 0;

    void RegisterItems()
    {
        for (int i = 0; i < items.Length; i++)
        {
            Transform currentItem = transform.GetChild(i);
            if (currentItem.childCount != 0)
            {            
                items[i] = currentItem.GetChild(0).gameObject;
                filledSlots++;
            }
        }
        Debug.Log("This squad has " + items[0] + ", " + items[1] + ", " + items[2] + ", " + items[3]);
    }
}
