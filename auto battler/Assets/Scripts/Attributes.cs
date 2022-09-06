using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attributes : MonoBehaviour
{
    public float health = 100f;
    public float speed = 1f;
    public float morale = 100f;
    public float endurance = 100f;
    public float damageReduction = 0.1f;

    [SerializeField]int team;

    float currentHP;
    Company company;
    SpriteRenderer spriteRenderer;
    Vector3 facing = Vector3.right;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        currentHP = health;
    }

    public void ChangeHP(float change)
    {
        currentHP += change;
        if (currentHP <= 0)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
        company.RemoveModel(gameObject);
    }

    public int GetTeam()
    {
        return team;
    }

    public void SetFacing(Vector3 _facing)
    {
        facing = _facing;
        if(facing.x < 0)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
    }

    public Vector3 GetFacing()
    {
        return facing;
    }

    public Company GetCompany()
    {
        return company;
    }

    void SetCompany(ModelAttributes modelAttributes)
    {
        company = modelAttributes.company;
        team = modelAttributes.team;
    }
}
