using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitAction_MorningStar : UnitAction
{
    float damage = -10.0f;
    float facingRadius = 60.0f;
    float piercing = 0.1f;
    GameObject enemy;
    GameObject[] unitsBuffer = new GameObject[500];
    Attributes attributes;

    void Awake()
    {
        attributes = GetComponent<Attributes>();
        prio = 10;
        cooldown = 2.0f;
        castTime = 0.5f;
        backswing = 0.5f;
        range = 2.0f;
        animPath = "Placeholder/animation_attack";
    }

    protected override bool FindTargets()
    {
        BattlefieldManager.ModelsInRadius(transform.position, range, unitsBuffer);
        foreach (GameObject model in unitsBuffer)
        {
            if (!model)
                continue;
            if (model.GetComponent<Attributes>().GetTeam() != attributes.GetTeam() &&
               Vector3.Angle(attributes.GetFacing(), model.transform.position - transform.position) < facingRadius)
            {
                enemy = model;
                return true;
            }
        }
        return false;
    }

    protected override bool ProduceEffect()
    {
        if (enemy == null)
            return false;
        AttackPacket attack = new AttackPacket(damage, gameObject, piercing);
        enemy.GetComponent<Attributes>().ReceiveAttack(attack);
        return true;
    }
}
