using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitPassive : UnitUpgrade
{
    protected Attributes attributes;
    public virtual void OnPurchase()
    {

    }
    public virtual void OnHit(AttackPacket packet)
    {

    }

    public void SetAttributes(Attributes _attributes)
    {
        attributes = _attributes;
    }
}
