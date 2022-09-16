using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPassive_Shirt : UnitPassive
{
    float speedChange = 0.5f;

    public override void OnPurchase()
    {
        attributes.ChangeSpeed(speedChange);
    }
}
