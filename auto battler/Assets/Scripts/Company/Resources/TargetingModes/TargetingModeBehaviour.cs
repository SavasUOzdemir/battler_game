using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetingModeBehaviour : MonoBehaviour
{
    protected Company company;

    void Awake()
    {
        company = GetComponent<Company>();
    }
    public abstract bool DetermineTarget(Vector3 companyPosition, ref GameObject target);
}
