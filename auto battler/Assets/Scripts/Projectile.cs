using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float damage;
    Vector3 velocity;
    Vector3 gravity = Vector3.zero;
    Animator animator;
    float fuse = 0.2f;
    float timeAlive = 0;



    void Awake()
    {    
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        GetComponent<Collider>().enabled = false;
    }

    void Update()
    {
        transform.position += velocity * Time.deltaTime;
        velocity += gravity * Time.deltaTime;
        if (transform.position.y <= 0)
        {
            Destroy(this.gameObject);
        }
        if (timeAlive > fuse)
            GetComponent<Collider>().enabled = true;
        timeAlive += Time.deltaTime;
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
