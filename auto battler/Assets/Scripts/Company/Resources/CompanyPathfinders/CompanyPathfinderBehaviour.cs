using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CompanyPathfinderBehaviour : MonoBehaviour
{
    protected Company company;
    protected CompanyMover mover;
    void Awake()
    {
        company = GetComponent<Company>();
        mover = GetComponent<CompanyMover>();
    }

    public abstract Vector3 GetMovementTarget(out Vector3 direction);

}
