using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_AdhesiveSling : Projectile
{
    float damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Attributes>().GetTeam() == team)
            return;
        AttackPacket attack = new AttackPacket(damage, null, 0f, 0f, 0f, new UnitBuff_AdhesiveSling(), 0f);
        other.GetComponent<Attributes>().ReceiveAttack(attack);
        Destroy(this.gameObject);
    }

    public void SetStats(float damage)
    {
        this.damage = damage;
    }
}
