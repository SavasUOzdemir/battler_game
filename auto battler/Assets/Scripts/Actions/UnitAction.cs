using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitAction : MonoBehaviour
{
    float currentCooldown = 0.0f;

    protected int prio = 10;
    protected float cooldown;
    protected float castTime;
    protected float backswing;
    protected float range = 0;

    protected bool hasProjectile = false;
    protected float projectileSpeed;

    protected Animator animator;
    protected AnimatorOverrideController animatorOverrideController;
    protected string animPath;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        LoadResources();
    }

    void Update()
    {
        ProgressCooldown();
    }

    public IEnumerator DoAction()
    {
        if(FindTargets())
        {
            animator.Play("Attack");
            yield return new WaitForSeconds(castTime);
            if(ProduceEffect())
            {
                currentCooldown = cooldown;
                yield return new WaitForSeconds(backswing);
            }
            animator.Play("Idle");
        }
        gameObject.GetComponent<Actor>().busy = false;
    } 

    protected virtual bool FindTargets()
    {
        return true;
    }

    protected abstract bool ProduceEffect();

    void ProgressCooldown()
    {
        currentCooldown = currentCooldown - Time.deltaTime;
    }

    void LoadResources()
    {
        animatorOverrideController["animation_attack"] = Resources.Load<AnimationClip>(animPath);
    }

    public int getPrio()
    {
        return prio;
    }

    public float getCurrentCooldown()
    {
        return currentCooldown;
    }

    public float GetRange()
    {
        return range;
    }

}
