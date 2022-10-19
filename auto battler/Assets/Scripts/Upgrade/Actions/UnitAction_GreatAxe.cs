using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitAction_GreatAxe : UnitAction
{
    private float damage = -20.0f;
    private float facingRadius = 90.0f;
    private List<GameObject> enemies;
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
        enemies = new List<GameObject>();
        foreach (GameObject model in unitsBuffer)
        {
            if (!model)
                continue;
            if (model.GetComponent<Attributes>().GetTeam() != attributes.GetTeam() && 
                Vector3.Angle(model.transform.position - transform.position, attributes.GetFacing()) < facingRadius)
                enemies.Add(model.gameObject);
        }
        if (enemies.Count > 0)
            return true;
        return false;

    }

    protected override bool ProduceEffect()
    {
        AttackPacket attack = new AttackPacket(damage, gameObject);
        foreach (GameObject enemy in enemies)
        {
            if(!enemy)
                continue;
            enemy.GetComponent<Attributes>().ReceiveAttack(attack);
        }
        return true;
    }
}
