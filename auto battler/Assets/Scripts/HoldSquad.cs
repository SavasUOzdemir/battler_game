using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldSquad : MonoBehaviour
{
    [SerializeField] Squad squad;

    public Squad Squad
    {
        get
        {
            return squad;
        }
        set
        {
            squad = value;
            UpdateImage();
        }
    }

    void UpdateImage()
    {
        //some logic to update image associated to grid slot
    }
}
