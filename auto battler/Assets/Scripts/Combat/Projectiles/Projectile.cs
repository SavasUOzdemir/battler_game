using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected Vector3 velocity;
    protected Vector3 gravity = Vector3.zero;
    protected Animator animator;
    protected int team;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        transform.position += velocity * Time.deltaTime;
        velocity += gravity * Time.deltaTime;
        if (transform.position.y <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void Init(float speed, float ySpeed, Transform target, int team)
    {
        velocity = Vector3.Normalize(target.position - transform.position) * speed;
        velocity.y = ySpeed;
        gravity.y = -2 * speed * velocity.y / (transform.position - target.position).magnitude;
        this.team = team;
    }

}
