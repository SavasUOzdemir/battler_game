using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickTarget: MonoBehaviour
{
    public GameObject[] enemies;
    public Transform Target;
    public Attributes unitattributes;

    void Update()
    {
        UpdateTarget();
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }

            if (nearestEnemy != null) //&& shortestDistance <= unitattributes.attackRange)
            {
                Target = nearestEnemy.transform;
            }
            else
            {
                Target = null;
            }
        }
    }
}