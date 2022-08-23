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
    protected Animator animator;

    void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        ProgressCooldown();
    }

    public IEnumerator DoAction()
    {
        List<GameObject> targets = FindTargets();
        if(targets != null)
        {
            animator.SetBool("acting", true);
            yield return new WaitForSeconds(castTime);
            foreach(GameObject unit in targets)
            {
                animator.SetBool("casted", true);
                ProduceEffect(unit);
            }
            currentCooldown = cooldown;
            yield return new WaitForSeconds(backswing);
            animator.SetBool("acting", false);
            animator.SetBool("casted", false);
        }
        gameObject.GetComponent<Actor>().busy = false;
    }

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
