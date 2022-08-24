using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState : MonoBehaviour
{

    //TODO:: Add animator controls
    //TODO:: Add movement states
    List<string> states = new List<string>();
    string currentState = null;

    public void AddState(string state)
    {
        if (!states.Contains(state))
        {
            states.Add(state);
        }
    }

    public void SetCurrentState(string state)
    {
        if(states.Contains(state) && !currentState.Equals(state))
        {
            currentState = state;
        }
    }

    public string GetCurrentState()
    {
        return currentState;
    }
}
