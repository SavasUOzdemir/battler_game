using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitAction_ShortSword : UnitAction
{
    float damage = -10.0f;
    float facingRadius = 90.0f;
    GameObject enemy;
    GameObject[] unitsBuffer = new GameObject[500];

    void Awake()
    {
        prio = 10;
        cooldown = 1.0f;
        castTime = 0.5f;
        backswing = 0.5f;
        range = 2.0f;
        animPath = "Placeholder/animation_attack";
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
                enemy = obj;
                return true;
            }
        }

        return false;
    }

    protected override bool ProduceEffect()
    {
        if (enemy == null)
            return false;
        AttackPacket attack = new AttackPacket(damage, gameObject);
        enemy.GetComponent<Attributes>().ReceiveAttack(attack);
        return true;
    }
}