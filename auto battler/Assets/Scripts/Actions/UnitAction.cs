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
    protected float range = 1f;
    protected bool melee = true;

    protected bool hasProjectile = false;
    protected float projectileSpeed;

    protected Animator animator;
    protected AnimatorOverrideController animatorOverrideController;
    protected string animPath;
    protected Actor actor;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        actor = gameObject.GetComponent<Actor>();
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
        actor.busy = false;
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

    public bool IsActionMelee()
    {
        return melee;
    }
}
