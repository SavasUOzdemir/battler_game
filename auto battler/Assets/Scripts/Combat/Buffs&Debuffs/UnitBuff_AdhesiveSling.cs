using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBuff_AdhesiveSling : UnitBuff
{
    float enduranceChange = -5;
    float everyXSecond = 2f;
    float tickCooldown = 2f;
    
    public UnitBuff_AdhesiveSling()
    {
        duration = 10f;
    }
    public override void EveryTick(Attributes attributes)
    {
        tickCooldown -= Time.deltaTime;
        if(tickCooldown <= 0f)
        {
            attributes.ChangeEndurance(enduranceChange);
            tickCooldown = everyXSecond;
        }
    }
}
