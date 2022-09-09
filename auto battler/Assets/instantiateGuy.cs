using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class instantiateGuy : MonoBehaviour
{
    public GameObject guyPrefab;
    public GameObject spawner;

    public void InstantiateThisGuy()
    {
        var helo = Instantiate(guyPrefab, spawner.transform.position, Quaternion.identity);
    }
}
