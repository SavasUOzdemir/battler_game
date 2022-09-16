using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPassive_Gambeson : UnitPassive
{
    float armor = 0.1f;

    public override void OnPurchase()
    {
        attributes.armor += armor;
    }
}
