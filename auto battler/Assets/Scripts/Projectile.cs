using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float damage;
    private Vector3 velocity;
    private Vector3 gravity = Vector3.zero;
    private Animator animator;
    private Collider collider;


    void Awake()
    {    
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();
    }

    private void Start()
    {

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

    public void Init(float speed, float ySpeed, Transform target, float damage)
    {    
        velocity = Vector3.Normalize(target.position - transform.position) * speed;
        velocity.y = ySpeed;
        gravity.y = -2 * speed * velocity.y / (transform.position - target.position).magnitude ;
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Attributes>().ChangeHP(-damage);
        Destroy(this.gameObject);
    }
}
