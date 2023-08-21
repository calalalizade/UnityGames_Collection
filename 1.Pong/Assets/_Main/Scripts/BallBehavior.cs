using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    [SerializeField] private float initialSpeed = 5f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] ParticleSystem collisionParticle;
    [SerializeField] AudioClip ballHit;

    private Rigidbody2D _rb;
    private CameraController cameraController;
    AudioSource audioData;

    private Vector2 direction;
    private float currentSpeed = 5f;
    private float extraSpeed = 0.5f;
    private int hitCounter = 0;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        audioData = GetComponent<AudioSource>();
        cameraController = Camera.main.GetComponent<CameraController>();

        Invoke(nameof(StartBall), 2f);
    }

    private void StartBall()
    {
        float x = Random.value < 0.5f ? -1f : 1f;
        float y = Random.value < 0.5f ? Random.Range(-0.8f, -0.5f) : Random.Range(0.5f, 0.8f);

        direction = new Vector2(x, y);
        _rb.velocity = direction * currentSpeed;
    }

    // Calculate the point of contact on the paddle where the ball hit
    private float HitFactor(Vector2 ballPos, Vector2 paddlePos, float paddleHeight)
    {
        return (ballPos.y - paddlePos.y) / paddleHeight;
    }

    private void BallBouncePaddle(Transform _hitObject)
    {
        float dirX, dirY;
        if (transform.position.x > 0)
        {
            dirX = -1;
        }
        else
        {
            dirX = 1;
        }
        dirY = HitFactor(transform.position,
                        _hitObject.position,
                        _hitObject.GetComponent<Collider2D>().bounds.size.y);

        // Avoid cases where the ball reflects in a straight horizontal line
        // by setting a small value to dirY
        if (dirY == 0)
        {
            dirY = Random.value < 0.5f ? -.3f : .3f;
        }

        direction = new Vector2(dirX, dirY);
        _rb.velocity = direction * currentSpeed;
    }

    private void BallBounceWall(Collision2D _hitObject)
    {
        // Calculation of reflection vector 
        Vector2 normal = _hitObject.GetContact(0).normal;
        Vector2 reflection = direction - 2 * Vector2.Dot(direction, normal) * normal;

        _rb.velocity = reflection * currentSpeed;
    }

    private void AdjustBallSpeed()
    {
        hitCounter++;
        currentSpeed = initialSpeed + hitCounter * extraSpeed;
        currentSpeed = Mathf.Clamp(currentSpeed, 5f, maxSpeed);
    }

    private void AdjustCameraShakeIntensity()
    {
        float shakeIncrement = currentSpeed - initialSpeed;
        cameraController.IncreaseIntensity(shakeIncrement);
    }

    private void EmitParticle(int _amount)
    {
        collisionParticle.Emit(_amount + hitCounter);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Paddle"))
        {
            AdjustBallSpeed(); // Increment ball speed after each hit
            AdjustCameraShakeIntensity(); // Increment camera shake intensity
            BallBouncePaddle(collision.transform);
            EmitParticle(2);

            AudioSource.PlayClipAtPoint(ballHit, collision.transform.position);
        }
        else if (collision.collider.CompareTag("Wall"))
        {
            BallBounceWall(collision);
            EmitParticle(0);

            AudioSource.PlayClipAtPoint(ballHit, collision.transform.position);
        }
    }

    public void ResetBall()
    {
        // reset position and velocity
        _rb.velocity = Vector2.zero;
        transform.position = Vector2.zero;

        // reset speed
        currentSpeed = initialSpeed;
        hitCounter = 0;

        Invoke(nameof(StartBall), 1f);
    }
}
