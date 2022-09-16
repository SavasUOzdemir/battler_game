using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_AdhesiveSling : Projectile
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Attributes>().GetTeam() == team)
            return;
        AttackPacket attack = new AttackPacket(damage, null, 0f, 0f, 0f, new UnitBuff_AdhesiveSling(), 0f);
        other.GetComponent<Attributes>().ReceiveAttack(attack);
        Destroy(this.gameObject);
    }
}
