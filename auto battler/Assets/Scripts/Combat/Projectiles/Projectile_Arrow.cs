using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Projectile_Arrow : Projectile
{
    float damage = 0f;
    float piercing = 0f;
    float moraleDamage = 0f;
    System.Type debuff = null;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Attributes>().GetTeam() == team)
            return;
        UnitBuff newDebuff = null;
        if (debuff != null)
            newDebuff = (UnitBuff)Activator.CreateInstance(debuff);
        AttackPacket attack = new AttackPacket(damage, null, piercing, 0f, moraleDamage, newDebuff);
        other.GetComponent<Attributes>().ReceiveAttack(attack);
        Destroy(this.gameObject);
    }

    public void SetStats(float damage, float piercing, float moraleDamage, System.Type debuff)
    {
        this.damage = damage;
        this.piercing = piercing;
        this.moraleDamage = moraleDamage;
        this.debuff = debuff;
    }
}
