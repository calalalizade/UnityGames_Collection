using UnityEngine;

public class PlayerController : Paddle
{
    private Vector2 moveDir;

    // Update is called once per frame
    void Update()
    {
        moveDir = new Vector2(0, Input.GetAxis("Vertical"));
    }

    void FixedUpdate()
    {
        _rb.velocity = moveDir * speed;
    }
}
