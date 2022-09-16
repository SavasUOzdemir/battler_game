using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitAction_LongBow : UnitAction
{
    GameObject projectile;
    Transform target;
    float damage = -30.0f;
    float facingRadius = 60.0f;
    float ySpeed = 20f;
    GameObject[] unitsBuffer = new GameObject[100];

    void Awake()
    {
        prio = 1;
        cooldown = 1.0f;
        castTime = 0.5f;
        backswing = 0.5f;
        range = 20f;
        melee = false;
        animPath = "Placeholder/animation_attack";
        hasProjectile = true;
        projectileSpeed = 20.0f;
        projectile = Resources.Load("Projectiles/Arrow/Prefab_Arrow") as GameObject;
    }

    protected override bool FindTargets()
    {
        BattlefieldManager.ModelsInRadius(transform.position, range, unitsBuffer);
        foreach (GameObject obj in unitsBuffer)
        {
            if (!obj)
                continue;
            if (obj.GetComponent<Attributes>().GetTeam() != attributes.GetTeam() &&
               Vector3.Angle(attributes.GetFacing(), obj.transform.position - transform.position) < facingRadius)
            {
                target = obj.transform;
                return true;
            }
        }
        return false;
    }

    protected override bool ProduceEffect()
    {
        if (target != null)
        {
            GameObject firedProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
            firedProjectile.GetComponent<Projectile_Arrow>().Init(projectileSpeed, ySpeed, target, damage, attributes.GetTeam());
            return true;
        }
        return false;
    }
}
