using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanyPassive_MoreModels : CompanyPassive
{
    [SerializeField] int extraModels = 8;
    public override void OnPurchase()
    {
        company.ModelCount += extraModels;
    }
}
    
