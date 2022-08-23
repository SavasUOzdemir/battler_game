using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionCleave : UnitAction
{
    private float range = 2.0f;
    private float damage = 10.0f;
    private float facingRadius = 90.0f;

    void Start()
    {
        prio = 1;
        cooldown = 8.0f;
        castTime = 0.5f;
        backswing = 0.5f;
    }

    public override void DoAction()
    {
        acting = true;
    }
 
    protected override List<GameObject> FindTargets()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, range);
        List<GameObject> gObjects = new List<GameObject>();
        foreach(Collider2D obj in objects)
        {
            if (Vector2.Angle(obj.transform.position - transform.position, transform.right) < facingRadius)  // TODO:: Transform.right to proper facing vector
                gObjects.Add(obj.gameObject);
        }
        return gObjects;

    }

    protected override void ProduceEffect(GameObject enemy)
    {
        if (enemy == null)
            return;
        TakeDamage takeDamage = enemy.GetComponent<TakeDamage>();
        takeDamage.ChangeAttr(damage);
    }

}
