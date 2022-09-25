using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitAction_GreatSword : UnitAction
{
    float damage = -40.0f;
    float facingRadius = 60.0f;
    GameObject enemy;
    GameObject[] unitsBuffer = new GameObject[500];

    void Awake()
    {
        prio = 10;
        cooldown = 2.0f;
        castTime = 0.5f;
        backswing = 0.5f;
        range = 2.0f;
        animPath = "Placeholder/animation_attack";
    }

    public override bool FindTargets()
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
        AttackPacket attack = new AttackPacket(damage, gameObject);
        enemy.GetComponent<Attributes>().ReceiveAttack(attack);
        return true;
    }
}
