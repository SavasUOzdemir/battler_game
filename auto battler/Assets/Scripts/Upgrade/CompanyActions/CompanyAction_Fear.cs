using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanyAction_Fear : CompanyAction
{
    GameObject target = null;
    float moraleDamage = -10f;

    void Start()
    {
        range = 10f;
        cooldown = 10f;
    }
    public override bool FindTarget()
    {
        foreach (GameObject enemy in company.enemiesList)
        {
            if ((enemy.transform.position - transform.position).sqrMagnitude <= range * range)
            {
                target = enemy;
                return true;
            }
        }
        return false;
    }

    protected override bool ProduceEffect()
    {
        if (!target)
            return false;
        target.GetComponent<Company>().ChangeMorale(moraleDamage);
        return true;
    }
}
