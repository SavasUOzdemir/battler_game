using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPassive_ArmorSpikes : UnitPassive
{
    float damageReturn = 0.1f;

    public override void OnHit(AttackPacket packet)
    {
        if (packet.Owner != null)
        {
            AttackPacket returnToSender = new AttackPacket(packet.HpChange * damageReturn, gameObject);
            packet.Owner.GetComponent<Attributes>().ReceiveAttack(returnToSender);
        }
    }
}
