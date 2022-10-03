using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CompanyAction : UnitUpgrade
{
    [SerializeField] private float currentCooldown = 0.0f;

    [SerializeField] protected int prio = 10;
    [SerializeField] protected float cooldown = 10f;
    [SerializeField] protected float range = 1f;
    [SerializeField] protected float exhaust = 0f;

    protected Company company;

    private void Awake()
    {
        company = gameObject.GetComponent<Company>();
    }
    void ProgressCooldown()
    {
        currentCooldown -= Time.deltaTime;
    }

    public virtual bool FindTarget()
    {
        return true;
    }

    public bool DoAction()
    {
        if (ProduceEffect())
        {
            ProgressCooldown();
            company.ExhaustCompany(exhaust);
            return true;
        }

        return false;
    }

    protected abstract bool ProduceEffect();

    public int GetPrio()
    {
        return prio;
    }

    public float GetCurrentCooldown()
    {
        return currentCooldown;
    }
}
