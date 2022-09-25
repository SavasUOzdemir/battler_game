using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class UnitPassive_WhistleArrow : UnitPassive
{
    float moraleDamage = -1f;
    public override void OnPurchase()
    {
        Component bow = attributes.GetComponent<UnitAction_LongBow>();
        if (bow)
        {
            (bow as UnitAction_LongBow).UpgradeBow(0f, 0f, moraleDamage, null);
            return;
        }
        bow = attributes.GetComponent<UnitAction_ShortBow>();
        if (bow)
        {
            (bow as UnitAction_ShortBow).UpgradeBow(0f, 0f, moraleDamage, null);
            return;
        }
    }
}
