using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }

    public void Launch(Vector2 _dir, float bulletSpeed, float bulletLifetime)
    {
        _rb.AddForce(_dir * bulletSpeed);

        // Destroy the bullet after it reaches its lifetime
        Destroy(gameObject, bulletLifetime);
    }
}
