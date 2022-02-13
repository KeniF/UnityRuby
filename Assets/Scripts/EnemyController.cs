using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyController : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    Animator animator;
    public bool isVertical = true;
    int direction = 1;
    bool broken = true;
    float currentPosition = 0;
    public int maxDisplacement = 100;
    public float speed = 1.0f;
    public ParticleSystem smokeEffect;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }

    void Update()
    {
        if (!broken)
        {
            return;
        }
    }

    void FixedUpdate()
    {
        if (!broken)
        {
            return;
        }
        Vector2 position = rigidbody2d.position;
        currentPosition += speed * direction;
        if (Math.Abs(currentPosition) > maxDisplacement)
        {
            direction *= -1;
        }

        if (!isVertical)
        {
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
            position.x = position.x + speed * Time.deltaTime * direction;
        }
        else
        {
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
            position.y = position.y + speed * Time.deltaTime * direction;
        }
        rigidbody2d.MovePosition(position);
    }
    public void Fix()
    {
        smokeEffect.Stop();
        broken = false;
        animator.SetTrigger("Fixed");
        GetComponent<Rigidbody2D>().simulated = false;
    }
}
