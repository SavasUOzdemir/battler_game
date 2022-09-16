using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attributes : MonoBehaviour
{
    public float maxHP = 100f;
    public float maxMorale = 100f;
    public float maxEndurance = 100f;
    public float armor = 0f;
    public float discipline = 0f;
    public float vigor = 0f;
    public float speed = 1f;
    public float knockbackResist = 0f;

    [SerializeField]int team;

    [SerializeField]float currentHP;
    [SerializeField] Vector3 facing = Vector3.right;
    Company company;
    SpriteRenderer spriteRenderer;   
    List<UnitBuff> buffList = new List<UnitBuff>();
    List<UnitPassive> passivesList = new List<UnitPassive>();
    Vector3 lastPos;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        BattlefieldManager.AddModel(gameObject);
        currentHP = maxHP;
        lastPos = transform.position;
    }

    private void Update()
    {
        //if(!company.InMelee())
            updateFacing();
        foreach(UnitBuff buff in buffList)
        {
            buff.BuffEffect(this);
            if(buff.BuffTick() <= 0f)
                buffList.Remove(buff);
        }
    }

    void updateFacing()
    {
        facing = (transform.position - lastPos).normalized;
    }

    void ChangeHP(AttackPacket packet)
    {
        if (packet.HpChange < 0f)
            currentHP += packet.HpChange * (1 - Mathf.Clamp(armor - packet.Piercing, 0f, 1f));
        else
            currentHP += packet.HpChange;
        if (currentHP <= 0)
            Die();
    }

    void Die()
    {
        Destroy(gameObject);
        company.RemoveModel(gameObject);
        BattlefieldManager.RemoveModel(gameObject);
    }

    public void ReceiveAttack(AttackPacket packet)
    {
        foreach(UnitPassive passive in passivesList)
        {
            if (packet.blocked)
                break;
            passive.OnHit(packet);
        }
        if(packet.buffList.Count > 0)
        {
            foreach(UnitBuff buff in packet.buffList)
            {
                buffList.Add(buff);
            }
        }
        if (!packet.blocked)
        {
            ChangeHP(packet);
        }    
    }

    public void AddUnitPassive(UnitPassive passive)
    {
        passive.SetAttributes(this);
        passivesList.Add(passive);
        passive.OnPurchase();
    }

    public int GetTeam()
    {
        return team;
    }

    public void SetFacing(Vector3 _facing)
    {
        facing = _facing;
        if(facing.x < 0)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
    }

    public Vector3 GetFacing()
    {
        return facing;
    }

    public Company GetCompany()
    {
        return company;
    }

    public void SetCompany(ModelAttributes modelAttributes)
    {
        company = modelAttributes.company;
        team = modelAttributes.team;
    }
}
