using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Transform target;
    float shortestDistance;
    float attackRange;
    float speed;
    float distance;

    private void Awake()
    {
        attackRange = GetComponent<Attributes>().attackRange;
        speed = GetComponent<Attributes>().speed;
    }
    void GetTarget()
    {
        target = GetComponent<PickTarget>().Target;
    }

    void Movement_()
    {
        
        if (target == null)
        {
            GetTarget();
            return;
        }
        else
        {
            distance = Vector2.Distance(transform.position, target.position);
        }
        if (Mathf.Abs(distance) > attackRange)
        {
            transform.Translate(target.position.x*Time.deltaTime*speed, target.position.y*Time.deltaTime*speed, 0, Space.World);
        }
    }
}
