using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clamp : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -9.3f, 9.7f), Mathf.Clamp(transform.position.y, -4f, 4f), transform.position.z);
    }
}