using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SpaceShipController : MonoBehaviour
{
    [Header("Speed Properties")]
    [SerializeField] float rotationSpeed = 5.0f;
    [SerializeField] float moveSpeed = 5.0f;

    [Space(10)]
    [SerializeField] float invincibleDuration = 3.0f;

    // Main References
    private Rigidbody2D _rb;
    private Animator _anim;
    private SpriteRenderer _renderer;
    private Camera _cam;

    // Movement properties
    private float horizontalInput;
    private bool shouldMove = false;

    // ScreenWrapping properties
    private Vector2 viewportPosition;
    private bool isWrappingX = false;
    private bool isWrappingY = false;
    private bool isVisible = false;

    private void Awake()
    {
        _cam = Camera.main;
        _rb = GetComponent<Rigidbody2D>();
        _anim = transform.GetChild(0).GetComponent<Animator>();
        _renderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        // Temporarily disable collisions after spawning to give the player
        // time to move away from asteroids safely
        ApplyInvincibleState();
        StartCoroutine(DisableInvincibleState());
    }

    private void Update()
    {
        InputHandler();
        ScreenWrap();
    }

    private void FixedUpdate()
    {
        // Apply rotation to the Rigidbody2D's rotation
        _rb.MoveRotation(_rb.rotation + rotationSpeed * -horizontalInput);

        if (!shouldMove) return;
        // Add a force to the Rigidbody2D to move the player forward
        _rb.AddForce(transform.up * moveSpeed);
    }

    private void InputHandler()
    {
        // Get the horizontal input for turning
        horizontalInput = Input.GetAxis("Horizontal");

        // Forward move
        shouldMove = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        _anim.SetBool("IsMoving", shouldMove);
    }

    private void ScreenWrap()
    {
        // If the object is currently visible, reset wrapping flags and return
        if (isVisible)
        {
            isWrappingX = isWrappingY = false;
            return;
        }

        Vector2 newPosition = transform.position;
        viewportPosition = _cam.WorldToViewportPoint(transform.position);

        // Check and apply wrapping on the X-axis if needed
        if (!isWrappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
        {
            newPosition.x = -newPosition.x;
            isWrappingX = true;
        }
        // Check and apply wrapping on the Y-axis if needed
        if (!isWrappingY && (viewportPosition.y > 1 || viewportPosition.y < 0))
        {
            newPosition.y = -newPosition.y;
            isWrappingY = true;
        }

        transform.position = newPosition;  // Update the object's position
    }

    private void ApplyInvincibleState()
    {
        gameObject.layer = LayerMask.NameToLayer("Ghost Layer");

        // Plays an invulnerability animation effect on 
        // the player character for a specified duration.
        StartCoroutine(InvincibilityAnimation());
    }

    private IEnumerator DisableInvincibleState()
    {

        yield return new WaitForSeconds(invincibleDuration);
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private IEnumerator InvincibilityAnimation()
    {
        float startTime = Time.time;

        while (Time.time - startTime < invincibleDuration)
        {
            _renderer.enabled = !_renderer.enabled; // Toggle renderer

            yield return new WaitForSeconds(0.1f);
        }
        _renderer.enabled = true; // Ensure the renderer is enabled at the end
    }

    // Check if renderer is visible by camera
    private void OnBecameVisible() => isVisible = true;
    private void OnBecameInvisible() => isVisible = false;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Asteroid"))
        {
            _rb.velocity = Vector2.zero;
            transform.rotation = Quaternion.identity;

            GameManager.GetInstance().OnPlayerDeath(this);
        }
    }
}
