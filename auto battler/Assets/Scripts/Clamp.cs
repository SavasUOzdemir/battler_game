using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clamp : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -9.3f, 9.7f), transform.position.y, Mathf.Clamp(transform.position.z, -6.81368f, 8.324822f));
    }
}