using UnityEngine;

public class AiController : Paddle
{
    [SerializeField] Rigidbody2D ball;

    // void FixedUpdate()
    // {
    //     // Check if the ball is moving to the right
    //     if (ball.velocity.x > 0.0f)
    //     {
    //         if (ball.transform.position.y > transform.position.y)
    //         {
    //             _rb.velocity = Vector2.Lerp(_rb.velocity, Vector2.up * speed, Time.deltaTime);
    //         }
    //         else if (ball.transform.position.y < transform.position.y)
    //         {
    //             _rb.velocity = Vector2.Lerp(_rb.velocity, Vector2.down * speed, Time.deltaTime);
    //         }
    //         else
    //         {
    //             _rb.velocity = Vector2.Lerp(_rb.velocity, Vector2.zero, Time.deltaTime);
    //         }
    //     }
    //     // If the ball is moving to the left
    //     else
    //     {
    //         if (_rb.position.y > 0.0f)
    //         {
    //             _rb.velocity = Vector2.Lerp(_rb.velocity, Vector2.down * speed, Time.deltaTime);
    //         }
    //         else if (_rb.position.y < 0.0f)
    //         {
    //             _rb.velocity = Vector2.Lerp(_rb.velocity, Vector2.up * speed, Time.deltaTime);
    //         }
    //     }
    // }

    void FixedUpdate()
    {
        Vector2 targetVelocity = Vector2.zero;

        // Check if the ball is moving to the right
        if (ball.velocity.x > 0.0f)
        {
            if (ball.transform.position.y > _rb.position.y)
            {
                targetVelocity = Vector2.up * speed;
            }
            else if (ball.transform.position.y < _rb.position.y)
            {
                targetVelocity = Vector2.down * speed;
            }
        }
        // If the ball is moving to the left
        else
        {
            if (_rb.position.y > 0.0f)
            {
                targetVelocity = Vector2.down * speed;
            }
            else if (_rb.position.y < 0.0f)
            {
                targetVelocity = Vector2.up * speed;
            }
        }

        _rb.velocity = Vector2.Lerp(_rb.velocity, targetVelocity, Time.deltaTime);
    }

}