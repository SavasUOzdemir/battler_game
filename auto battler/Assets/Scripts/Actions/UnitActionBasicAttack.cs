using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionBasicAttack : UnitAction
{

    private float range = 2.0f;
    private float damage = 5.0f;
    private float facingRadius = 90.0f;

    void Start()
    {
        prio = 10;
        cooldown = 1.0f;
        castTime = 0.5f;
        backswing = 0.5f;
        animPath = "Animations/Placeholder/animation_attack";
    }
 
    protected override List<GameObject> FindTargets()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, range);
        foreach(Collider2D obj in objects)
        {
            if (Vector2.Angle(obj.transform.position - transform.position, transform.right) < facingRadius)  // TODO:: Transform.right to proper facing vector
                return new List<GameObject> { obj.gameObject };
        }
        return null;
    }

    protected override void ProduceEffect(GameObject enemy)
    {
        if (enemy == null)
            return;
        TakeDamage takeDamage = enemy.GetComponent<TakeDamage>();
        takeDamage.ChangeAttr(damage);
    }
}
