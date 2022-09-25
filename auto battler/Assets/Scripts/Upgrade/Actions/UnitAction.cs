using System.Collections;
using UnityEngine;

public abstract class UnitAction : UnitUpgrade
{
    float currentCooldown = 0.0f;

    protected int prio = 10;
    protected float cooldown;
    protected float castTime;
    protected float backswing;
    protected float range = 1f;
    protected float exhaust = 0f;

    protected bool mainWeapon = true;
    protected bool melee = true;
    protected bool hasProjectile = false;
    protected float projectileSpeed;

    protected Animator animator;
    protected AnimatorOverrideController animatorOverrideController;
    protected Attributes attributes;
    protected string animPath;
    protected Actor actor;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        attributes = gameObject.GetComponent<Attributes>();
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
        actor.moving = false;
        animator.Play("Attack");
        yield return new WaitForSeconds(castTime);
        if(ProduceEffect())
        {
            attributes.ChangeEndurance(exhaust);
            currentCooldown = cooldown;
            yield return new WaitForSeconds(backswing);
        }
        animator.Play("Idle");
        actor.busy = false;
    }

    public virtual bool FindTargets()
    {
        return true;
    }

    protected abstract bool ProduceEffect();

    void ProgressCooldown()
    {
        currentCooldown -=  Time.deltaTime;
    }

    void LoadResources()
    {
        animatorOverrideController["animation_attack"] = Resources.Load<AnimationClip>(animPath);
    }

    public int GetPrio()
    {
        return prio;
    }

    public float GetCurrentCooldown()
    {
        return currentCooldown;
    }

    public float GetRange()
    {
        return range;
    }

    public bool IsMainWeapon()
    {
        return mainWeapon;
    }

    public bool IsActionMelee()
    {
        return melee;
    }

    public float GetExhaust()
    {
        return exhaust;
    }
}
