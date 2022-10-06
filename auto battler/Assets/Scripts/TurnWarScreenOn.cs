using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnWarScreenOn : MonoBehaviour
{
    [SerializeField] GameObject warScreen;
    public void OnPress()
    {
        warScreen.SetActive(true);
    }
}
