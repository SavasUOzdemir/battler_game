using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBuff
{
    protected float duration = 0f;

    public abstract void BuffEffect(Attributes attribute);

    public float BuffTick()
    {
        return duration -= Time.deltaTime;
    }

    public float GetDuration()
    {
        return duration;
    }
}
