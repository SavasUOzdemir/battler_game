using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualMove : MonoBehaviour
{
    float speed = 3f;
    //Transform target = null;
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            transform.Translate(0, 1 * Time.deltaTime * speed, 0, Space.World);
        if (Input.GetKey(KeyCode.S))
            transform.Translate(0, -1 * Time.deltaTime * speed, 0, Space.World);
        if (Input.GetKey(KeyCode.A))
            transform.Translate(-1 * Time.deltaTime * speed, 0, 0, Space.World);
        if (Input.GetKey(KeyCode.D))
            transform.Translate(1 * Time.deltaTime * speed, 0, 0, Space.World);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Attack done");
            //target = GetComponent<PickTarget>().Target;
            //target.GetComponent<Attributes>().health -= 5;
        }
    }
}
