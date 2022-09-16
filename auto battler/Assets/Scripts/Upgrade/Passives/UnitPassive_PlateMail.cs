using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPassive_PlateMail : UnitPassive
{
    float armor = 0.3f;
    float discipline = 0.3f;
    float speedChange = -0.5f;
    float vigor = -0.2f;
    float knockbackResist = 0.2f;

    public override void OnPurchase()
    {
        attributes.armor += armor;
        attributes.discipline += discipline;
        attributes.ChangeSpeed(speedChange);
        attributes.vigor += vigor;
        attributes.knockbackResist += knockbackResist;
    }
}
