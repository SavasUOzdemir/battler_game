using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    private float currentHP = 30.0f;

    public void ChangeAttr(float change)
    {
        currentHP -= change;
        Debug.Log("I took " + change + " damage.");
        if (currentHP <= 0)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
