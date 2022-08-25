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
    protected Animator animator;
    protected AnimatorOverrideController animatorOverrideController;
    protected string animPath;

    void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
    }

    private void Start()
    {
        LoadAnimationClip(animPath);
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
            animator.Play("Attack");
            yield return new WaitForSeconds(castTime);     
            foreach (GameObject unit in targets)
            {
                ProduceEffect(unit);
            }
            currentCooldown = cooldown;
            yield return new WaitForSeconds(backswing);
            animator.Play("Idle");
        }
        gameObject.GetComponent<Actor>().busy = false;
    }

    protected abstract List<GameObject> FindTargets();
    protected abstract void ProduceEffect(GameObject unit);

    void ProgressCooldown()
    {
        currentCooldown = currentCooldown - Time.deltaTime;
    }

    void LoadAnimationClip(string path)
    {
        animatorOverrideController["animation_attack"] = Resources.Load<AnimationClip>(path);
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
