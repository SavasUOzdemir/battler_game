using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionCleave : UnitAction
{
    private float damage = -10.0f;
    private float facingRadius = 90.0f;
    private List<GameObject> enemies;

    void Start()
    {
        prio = 1;
        cooldown = 8.0f;
        castTime = 0.5f;
        backswing = 0.5f;
        range = 1f;
    }
 
    protected override bool FindTargets()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, range);
        enemies = new List<GameObject>();
        foreach(Collider2D obj in objects)
        {
            if (Vector2.Angle(obj.transform.position - transform.position, transform.right) < facingRadius)  // TODO:: Transform.right to proper facing vector
                enemies.Add(obj.gameObject);
        }
        if(enemies.Count > 0)
            return true;
        return false;

    }

    protected override bool ProduceEffect()
    {
        AttackPacket attack = new AttackPacket(damage, gameObject);
        foreach(GameObject enemy in enemies)
        {
            enemy.GetComponent<Attributes>().ReceiveAttack(attack);
        }
        return true;
    }
}
