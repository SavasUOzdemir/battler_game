using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float damage;
    Vector3 velocity;
    Vector3 gravity = Vector3.zero;
    Animator animator;
    int team;



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

    public void Init(float speed, float ySpeed, Transform target, float damage, int team)
    {    
        velocity = Vector3.Normalize(target.position - transform.position) * speed;
        velocity.y = ySpeed;
        gravity.y = -2 * speed * velocity.y / (transform.position - target.position).magnitude ;
        this.damage = damage;
        this.team = team;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Attributes>().GetTeam() == team)
            return;
        AttackPacket attack = new AttackPacket(damage);
        other.GetComponent<Attributes>().ReceiveAttack(attack);
        Destroy(this.gameObject);
    }
}
