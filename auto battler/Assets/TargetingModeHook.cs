using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingModeHook : MonoBehaviour
{
    [SerializeField] string targetingModeName = null;

    public string TargetingModeName
    {
        get { return targetingModeName; }
    }
}
