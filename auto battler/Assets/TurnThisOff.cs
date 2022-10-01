using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnThisOff : MonoBehaviour
{
    public void OnPress()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
