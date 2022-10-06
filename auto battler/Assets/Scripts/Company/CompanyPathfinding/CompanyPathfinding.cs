using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CompanyPathfinding : MonoBehaviour
{
    public abstract Vector3 GetMovementTarget();

    public virtual void OnTargetReached()
    {

    }

}
