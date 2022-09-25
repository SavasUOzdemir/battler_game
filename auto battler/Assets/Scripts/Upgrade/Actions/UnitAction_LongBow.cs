using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class UnitAction_LongBow : UnitAction
{
    GameObject projectile;
    Transform target;
    GameObject[] unitsBuffer = new GameObject[100];

    float damage = -30.0f;
    float piercing = 0f;
    float moraleDamage = 0f;
    Type debuff = null;
    float facingRadius = 60.0f;
    float ySpeed = 20f;
    

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

    public override bool FindTargets()
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
            Projectile_Arrow firedProjectile = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Projectile_Arrow>();
            firedProjectile.Init(projectileSpeed, ySpeed, target, attributes.GetTeam());
            firedProjectile.SetStats(damage, piercing, moraleDamage, debuff); 
            return true;
        }
        return false;
    }

    public void UpgradeBow(float damage, float piercing, float moraleDamage, Type debuff)
    {
        this.damage += damage;
        this.piercing += piercing;
        this.moraleDamage += moraleDamage;
        this.debuff = debuff;
    }
}
