using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ArrangementBehaviour : MonoBehaviour
{
    protected Company company;
    protected CompanyMover companyMover;

    //ARRANGEMENT VARIABLES
    //WARNING:: Bad variables might break shit
    //Wedge arrangement
    protected static readonly int InnerWedge = 4;
    protected static readonly int WedgeArmWidth = 2;

    void Awake()
    {
        company = GetComponent<Company>();
        companyMover = GetComponent<CompanyMover>();
    }
    public abstract void ArrangeModels(Vector3 companyPos, Vector3 direction);

}
