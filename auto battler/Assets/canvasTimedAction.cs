using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canvasTimedAction : MonoBehaviour
{
    FunctionTimer functionTimer;
    [SerializeField]GameObject tooltipCanvas;
    // Start is called before the first frame update
      
    private void OnEnable()
    {
        FunctionTimer.Create(DisableMove, 3f);
    }

    void DisableMove()
    {
        Debug.Log("Move disabled");
    }
}
