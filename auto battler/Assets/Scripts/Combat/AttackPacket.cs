using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPacket
{
    public float HpChange { get; }
    public float Piercing { get; }
    public float EnduranceChange { get; }
    public float MoraleChange { get; }
    public List<UnitBuff> buffList = new List<UnitBuff>();
    public GameObject owner { get; }
    public bool blocked { get; set; }

    public AttackPacket(float hpChange, GameObject owner = null, float piercing = 0f, float enduranceDamage = 0f, float moraleDamage = 0f, UnitBuff buff = null)
    {
        HpChange = hpChange;
        Piercing = piercing;
        EnduranceChange = enduranceDamage;
        MoraleChange = moraleDamage;
        if (!(buff == null))
            buffList.Add(buff);
        this.owner = owner;
    }
}
