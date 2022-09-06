using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class UnitActionBasicAttack : UnitAction
{

    float damage = 50.0f;
    float facingRadius = 60.0f;
    GameObject enemy;
    GameObject[] unitsBuffer = new GameObject[100];
    Attributes attributes;

    void Awake()
    {
        attributes = GetComponent<Attributes>();
        prio = 10;
        cooldown = 1.0f;
        castTime = 0.5f;
        backswing = 0.5f;
        range = 2.0f;
        animPath = "Placeholder/animation_attack";
    }
 
    protected override bool FindTargets()
    {
        Utils.UnitsInRadius(transform.position, range, unitsBuffer);
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
        enemy.GetComponent<Attributes>().ChangeHP(-damage);
        return true;
    }
}
