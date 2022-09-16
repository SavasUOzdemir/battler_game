using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySingleton : MonoBehaviour
{
    public static InventorySingleton instance;
    private int currentGold = 0;
    public int CurrentGold
    { 
        get 
        { 
            return currentGold; 
        }
        set
        {
            currentGold = value;
        }
    }

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}