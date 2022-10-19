using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBuff
{
    protected float duration = 0f;

    public virtual void OnApply(Attributes attributes)
    {

    }

    public virtual void EveryTick(Attributes attributes)
    {

    }

    public virtual void OnExpire(Attributes attributes)
    {

    }

    public float BuffTick()
    {
        return duration -= Time.deltaTime;
    }

    public float GetDuration()
    {
        return duration;
    }
}
