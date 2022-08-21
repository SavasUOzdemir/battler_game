using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behaviors : MonoBehaviour
{
    public GameObject[] enemies;
    private Transform Target;
    public Attributes unitattributes;

    // Start is called before the first frame update
    void Awake()
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
            Debug.Log(enemy.GetComponent<Attributes>().speed);
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }

            if (nearestEnemy != null && shortestDistance <= unitattributes.attackRange)
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