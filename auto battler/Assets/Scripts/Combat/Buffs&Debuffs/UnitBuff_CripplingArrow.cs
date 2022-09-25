using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBuff_CripplingArrow : UnitBuff
{
    float speedChange = -0.5f;

    public override void OnApply(Attributes attributes)
    {
        attributes.ChangeSpeed(speedChange);
        duration = 10f;
    }

    public override void OnExpire(Attributes attributes)
    {
        attributes.ChangeSpeed(-speedChange);
    }

}
