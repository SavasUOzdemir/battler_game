using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionBasicAttack : UnitAction
{

    private float range = 2.0f;
    private float damage = 5.0f;
    private float facingRadius = 90.0f;
    private GameObject enemy;

    void Start()
    {
        prio = 10;
        cooldown = 1.0f;
        castTime = 0.5f;
        backswing = 0.5f;
        animPath = "Placeholder/animation_attack";
    }
 
    protected override bool FindTargets()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, range);
        foreach(Collider2D obj in objects)
        {
            if (Vector2.Angle(obj.transform.position - transform.position, transform.right) < facingRadius)
            {
                enemy = obj.gameObject;
                return true;
            }  // TODO:: Transform.right to proper facing vector
                
        }
        return false;
    }

    protected override bool ProduceEffect()
    {
        if (enemy == null)
            return false;
        enemy.GetComponent<TakeDamage>().ChangeAttr(damage);
        return true;
    }
}
