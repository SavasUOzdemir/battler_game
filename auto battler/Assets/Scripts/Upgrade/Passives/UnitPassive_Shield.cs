using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPassive_Shield : UnitPassive
{
    float armor = 0.1f;
    float discipline = 0.1f;
    float vigor = -0.1f;
    float knockbackResist = 0.1f;
    float blockChance = 0.2f;

    public override void OnPurchase()
    {
        attributes.armor += armor;
        attributes.discipline += discipline;
        attributes.vigor += vigor;
        attributes.knockbackResist += knockbackResist;
    }

    public override void OnHit(AttackPacket packet)
    {
        if(Random.Range(0f,1f) < blockChance)
            packet.Blocked = true;
    }
}
