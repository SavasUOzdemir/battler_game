using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class UnitActionProjectileTest : UnitAction
{
    GameObject projectile;
    Transform target;
    Attributes attributes;
    float damage = 10.0f;
    float facingRadius = 60.0f;
    float ySpeed = 10f;
    GameObject[] unitsBuffer = new GameObject[100];

    void Awake()
    {
        attributes = GetComponent<Attributes>();
        prio = 1;
        cooldown = 1.0f;
        castTime = 0.5f;
        backswing = 0.5f;
        range = 30f;
        animPath = "Placeholder/animation_attack";
        hasProjectile = true;
        projectileSpeed = 30.0f;
        projectile = Resources.Load("Projectiles/Arrow/Prefab_Arrow") as GameObject;
    }

    protected override bool FindTargets()
    {
        Utils.UnitsInRadius(transform.position, range, unitsBuffer);
        foreach(GameObject obj in unitsBuffer)
        {
            if (!obj)
                continue;
            if(obj.GetComponent<Attributes>().GetTeam() != attributes.GetTeam() && 
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
            firedProjectile.GetComponent<Projectile>().Init(projectileSpeed, ySpeed, target, damage);
            return true;
        }
        return false;
    }
}
