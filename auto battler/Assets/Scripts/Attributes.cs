using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Attributes : MonoBehaviour
{
    public float maxHP = 100f;
    public float maxMorale = 100f;
    public float maxEndurance = 100f;
    public float armor = 0f;
    public float discipline = 0f;
    public float vigor = 0f; 
    public float knockbackResist = 0f;

    float speed = 1f;

    [SerializeField]int team;

    [SerializeField] float currentHP;
    [SerializeField] float currentEndurance;
    [SerializeField] Vector3 facing = Vector3.right;
    [SerializeField] List<UnitBuff> buffList = new List<UnitBuff>();
    Company company;
    SpriteRenderer spriteRenderer;
    AIPath aIPath;
    List<UnitPassive> passivesList = new List<UnitPassive>();
    Vector3 lastPos;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        aIPath = GetComponent<AIPath>();
    }

    void Start()
    {
        BattlefieldManager.AddModel(gameObject);
        currentHP = maxHP;
        currentEndurance = maxEndurance;
        aIPath.maxSpeed = speed;
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
        SetFacing((transform.position - lastPos).normalized);
    }

    public void ChangeSpeed(float change)
    {
        speed += change;
        aIPath.maxSpeed = speed;
    }
  

    public void ChangeEndurance(float change)
    {
        currentEndurance += change * (1 - vigor);
        if(currentEndurance > maxEndurance)
            currentEndurance = maxEndurance;
        if(currentEndurance < 0f)
            currentEndurance = 0f;
        Debug.Log(change);
    }
  

    void Die()
    {
        Destroy(gameObject);
        company.RemoveModel(gameObject);
        BattlefieldManager.RemoveModel(gameObject);
    }
    void KnockBack(AttackPacket packet)
    {
        if(packet.Owner != null)
        {
            aIPath.Move((transform.position - packet.Owner.transform.position).normalized * packet.KnockBack);
        }
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

    public void ReceiveAttack(AttackPacket packet)
    {
        foreach(UnitPassive passive in passivesList)
        {
            if (packet.Blocked)
                break;
            passive.OnHit(packet);
        }
        if(packet.buffList.Count > 0)
        {
            foreach(UnitBuff buff in packet.buffList)
            {
                if (buffList.Any(x => x.GetType() == buff.GetType()))
                    continue;
                buffList.Add(buff);
            }
        }
        if (!packet.Blocked)
        {
            ChangeHP(packet);
            KnockBack(packet);
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
