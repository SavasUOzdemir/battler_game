using UnityEngine;

public class UnitActionProjectileTest : UnitAction
{
    private GameObject projectile;
    private Transform target;
    private Attributes attributes;
    private float damage = 30.0f;
    private float facingRadius = 90.0f;
    private float ySpeed = 10f;
    private GameObject[] objects = new GameObject[100];

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
        Utils.UnitsInRadius(transform.position, range, objects);
        foreach(GameObject obj in objects)
        {
            if (!obj)
                continue;
            if(obj.GetComponent<Attributes>().GetTeam() != attributes.GetTeam() && 
               Vector3.Angle(obj.transform.position - transform.position, attributes.GetFacing()) < facingRadius)
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
