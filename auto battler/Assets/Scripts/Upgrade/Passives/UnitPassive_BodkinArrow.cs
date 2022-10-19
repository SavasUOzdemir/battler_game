using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPassive_BodkinArrow : UnitPassive
{
    float piercingUp = 10f;
    public override void OnPurchase()
    {
        Component bow = attributes.GetComponent<UnitAction_LongBow>();
        if (bow)
        {
            (bow as UnitAction_LongBow).UpgradeBow(0f, piercingUp, 0f, null);
            return;
        }
        bow = attributes.GetComponent<UnitAction_ShortBow>();
        if (bow)
        {
            (bow as UnitAction_ShortBow).UpgradeBow(0f, piercingUp, 0f, null);
            return;
        }
    }
}
