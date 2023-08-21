using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public float speed = 7f;

    protected Rigidbody2D _rb;

    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void ResetPaddle()
    {
        _rb.position = new Vector2(_rb.position.x, 0.0f);
        _rb.velocity = Vector2.zero;
    }
}
