using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Attributes : MonoBehaviour
{
    public float maxHP = 100f;
    public float maxMorale = 100f;
    public float maxEndurance = 100f;
    public float armor = 0f;
    public float discipline = 0f;
    public float vigor = 1f; 
    public float knockbackResist = 0f;
    public float attackSpeedMod = 1f;

    

    [SerializeField]int team;

    [field: SerializeField] public float CurrentHp { get; private set; }
    [SerializeField] float currentEndurance;
    [SerializeField] bool winded = false;
    [SerializeField] float speed = 1f;
    [SerializeField] Vector3 facing = Vector3.right;
    [SerializeField] List<UnitBuff> buffList = new List<UnitBuff>();
    Company company;
    SpriteRenderer spriteRenderer;
    AIPath aIPath;
    float perSecond = 1f;
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
        CurrentHp = maxHP;
        currentEndurance = maxEndurance;
        aIPath.maxSpeed = speed;
        lastPos = transform.position;
    }

    private void Update()
    {
        foreach(UnitBuff buff in buffList.ToList())
        {
            buff.EveryTick(this);
            if(buff.BuffTick() <= 0f)
            {
                buff.OnExpire(this);
                buffList.Remove(buff);
            }
        }
        
        perSecond -= Time.deltaTime;
        if (perSecond <= 0f)
        {
            ChangeEndurance(vigor);
            perSecond = 1f;
        }
    }

    public void UpdateFacing()
    {
        SetFacing((transform.position - lastPos).normalized);
    }

    public void ChangeSpeed(float change)
    {
        speed += change;
        if(speed <= 0f)
        {
            aIPath.maxSpeed = 0f;
        }
        else
        {
            aIPath.maxSpeed = speed;
        }
    }

    public float GetEndurance()
    {
        return currentEndurance;
    }

    public void ChangeEndurance(float change)
    {
        currentEndurance += change;
        if(currentEndurance > maxEndurance)
            currentEndurance = maxEndurance;
        else if(currentEndurance < 0f)
            currentEndurance = 0f;
        if(currentEndurance < maxEndurance * 0.2f)
            winded = true;
        else
            winded = false;
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
            aIPath.Move((transform.position - packet.Owner.transform.position).normalized * packet.KnockBack * knockbackResist);
        }
    }

    void ChangeHP(AttackPacket packet)
    {
        if (packet.HpChange < 0f)
            CurrentHp += packet.HpChange * (1 - Mathf.Clamp(armor - packet.Piercing, 0f, 1f));
        else
            CurrentHp += packet.HpChange;
        if (CurrentHp <= 0)
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
                buff.OnApply(this);
                buffList.Add(buff);
            }
        }
        if (!packet.Blocked)
        {
            ChangeHP(packet);
            KnockBack(packet);
            company.ChangeMorale(packet.MoraleChange);
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

    public bool GetWinded()
    {
        return winded;
    }

    public void SetCompany(ModelAttributes modelAttributes)
    {
        company = modelAttributes.company;
        team = modelAttributes.team;
    }
}
