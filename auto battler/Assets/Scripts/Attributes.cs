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
    }

    public int GetTeam()
    {
        return team;
    }
}
