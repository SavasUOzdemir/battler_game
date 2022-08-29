using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnitActionProjectileTest : UnitAction
{
    private GameObject projectile;
    private Transform target;
    private float range = 30.0f;
    private float damage = 30.0f;
    private float facingRadius = 90.0f;
    private float ySpeed = 10f;
    private Collider[] objects = new Collider[100];


    void Start()
    {
        prio = 1;
        cooldown = 1.0f;
        castTime = 0.5f;
        backswing = 0.5f;
        animPath = "Placeholder/animation_attack";
        hasProjectile = true;
        projectileSpeed = 30.0f;
        projectile = Resources.Load("Projectiles/Arrow/Prefab_Arrow") as GameObject;
    }

    protected override bool FindTargets()
    {
        Physics.OverlapSphereNonAlloc(transform.position, range, objects);
        foreach (Collider obj in objects)
        {
            if (obj != null && obj.CompareTag("Enemy") && Vector3.Angle(obj.transform.position - transform.position, transform.right) < facingRadius)
            {
                target = obj.gameObject.transform;
                objects[0] = null;
                return true;
            }  // TODO:: Transform.right to proper facing vector

        }
        return false;
    }

    protected override bool ProduceEffect()
    {
        if(target != null)
        {
            GameObject firedProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
            firedProjectile.GetComponent<Projectile>().Init(projectileSpeed, ySpeed, target, damage);
            return true;
        }
        return false;
    }
}
