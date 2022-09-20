using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rbody;
    [SerializeField] float bulletSpeed = 20f;

    PlayerMovement player;
    float xSpeed;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();

        xSpeed = player.transform.localScale.x * bulletSpeed;
    }

    void Update()
    {
        rbody.velocity = new Vector2(xSpeed, 0f) ;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

}
