using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D rbody;
    
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        
    }

    void Update()
    {
        rbody.velocity = new Vector2(moveSpeed, 0);
    }

    

    void OnTriggerExit2D(Collider2D collision)
    {
        moveSpeed = -moveSpeed;
        FlipEnemyFacing();
    }

    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(rbody.velocity.x)), 1f);
    }
}
