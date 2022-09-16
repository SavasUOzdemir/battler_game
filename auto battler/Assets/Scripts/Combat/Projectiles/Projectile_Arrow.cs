using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Arrow : Projectile
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Attributes>().GetTeam() == team)
            return;
        AttackPacket attack = new AttackPacket(damage);
        other.GetComponent<Attributes>().ReceiveAttack(attack);
        Destroy(this.gameObject);
    }
}
