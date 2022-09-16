using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPacket
{
    public float HpChange { get; }
    public float Piercing { get; }
    public float EnduranceChange { get; }
    public float MoraleChange { get; }
    public float KnockBack { get; }
    public List<UnitBuff> buffList = new List<UnitBuff>();
    public GameObject Owner { get; }
    public bool Blocked { get; set; }

    public AttackPacket(float hpChange, GameObject owner = null, float piercing = 0f,float knockBack = 0f, float moraleDamage = 0f, UnitBuff buff = null, float enduranceDamage = 0f)
    {
        HpChange = hpChange;
        Piercing = piercing;
        EnduranceChange = enduranceDamage;
        MoraleChange = moraleDamage;
        KnockBack = knockBack;
        if (!(buff == null))
            buffList.Add(buff);
        Owner = owner;
    }
}
