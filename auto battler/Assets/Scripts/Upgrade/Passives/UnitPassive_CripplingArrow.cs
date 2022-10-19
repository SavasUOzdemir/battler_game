using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPassive_CripplingArrow : UnitPassive
{
    System.Type debuff = typeof(UnitBuff_CripplingArrow);

    public override void OnPurchase()
    {
        Component bow = attributes.GetComponent<UnitAction_LongBow>();
        if (bow)
        {
            (bow as UnitAction_LongBow).UpgradeBow(0f, 0f, 0f, debuff);
            return;
        }
        bow = attributes.GetComponent<UnitAction_ShortBow>();
        if (bow)
        {
            (bow as UnitAction_ShortBow).UpgradeBow(0f, 0f, 0f, debuff);
            return;
        }
    }
}

