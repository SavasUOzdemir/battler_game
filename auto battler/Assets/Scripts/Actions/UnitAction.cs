using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitAction : MonoBehaviour
{
    protected int prio = 10;
    protected float cooldown;
    protected float castTime;
    protected float backswing;
    protected float currentAnimTime = 0.0f;
    protected float currentCooldown = 0.0f;
    protected bool acting = false;
    protected bool casted = false;

    void Update()
    {
        if (acting)
        {
            currentAnimTime += Time.deltaTime;
            if (currentAnimTime > castTime && !casted)
            {
                foreach (GameObject unit in FindTargets())
                {
                    ProduceEffect(unit);
                }
                currentCooldown = cooldown;
                casted = true;
            }
            else if (currentAnimTime > castTime + backswing)
            {
                acting = false;
                casted = false;
                currentAnimTime = 0.0f;
                gameObject.GetComponent<Actor>().busy = false;
            }
        }
        ProgressCooldown();
    }

    public abstract void DoAction();
    protected abstract List<GameObject> FindTargets();
    protected abstract void ProduceEffect(GameObject unit);

    void ProgressCooldown()
    {
        currentCooldown = currentCooldown - Time.deltaTime;
    }

    public int getPrio()
    {
        return prio;
    }

    public float getCurrentCooldown()
    {
        return currentCooldown;
    }

}
