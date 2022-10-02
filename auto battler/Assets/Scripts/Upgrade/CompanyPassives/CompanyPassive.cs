using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanyPassive : UnitUpgrade
{
    protected Company company;

    void Awake()
    {
        company = gameObject.GetComponent<Company>();
    }

    public virtual void OnPurchase()
    {

    }

}
