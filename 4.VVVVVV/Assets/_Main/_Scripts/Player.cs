using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private SpriteRenderer _playerSprite;
    [SerializeField] private Animator _anim;
    [SerializeField] AudioClip groundedSound;
    [SerializeField] AudioClip hurtSound;


    [Header("Speed Properties")]
    [SerializeField] private float verticalSpeed = 10.0f;
    [SerializeField] private float horizontalSpeed = 10.0f;

    private Rigidbody2D _rb;
    private Collider2D _collider;
    private float inputHorizontal;
    private bool isGrounded = true;

    // Respawn properties
    private Vector3 lastCheckpoint;
    private bool lastFlip;
    private bool isDied;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();

        lastCheckpoint = transform.position;
        lastFlip = _playerSprite.flipY;
    }

    private void Update()
    {
        if (isDied) return;

        inputHorizontal = Input.GetAxisRaw("Horizontal");

        DetectGround();
        FlipPlayerSprite();

        _anim.SetBool("isMove", inputHorizontal != 0);
        _anim.SetBool("isJump", !isGrounded);
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(inputHorizontal * horizontalSpeed, GetVerticalVelocity());
    }

    private float GetVerticalVelocity()
    {
        return isGrounded ? 0 : (_playerSprite.flipY ? verticalSpeed : -verticalSpeed);
    }

    private void DetectGround()
    {
        // Determine the direction of the ray based on the player's orientation
        Vector2 rayDirection = -transform.up;

        if (_playerSprite.flipY) // If the player is upside down
        {
            rayDirection = transform.up;
        }

        RaycastHit2D hit = Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0f, rayDirection, .1f, whatIsGround);

        isGrounded = hit.collider ? true : false;
    }

    private void FlipPlayerSprite()
    {
        // Check if the player should flip vertically (when gravity shifting)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            _playerSprite.flipY = !_playerSprite.flipY;

            SoundManager.GetInstance().PlayEffect(groundedSound);
        }

        // Check the horizontal input to determine the horizontal flip
        if (inputHorizontal < 0)
        {
            // Flip the player sprite horizontally (left)
            _playerSprite.flipX = true;
        }
        else if (inputHorizontal > 0)
        {
            // Flip the player sprite horizontally (right)
            _playerSprite.flipX = false;
        }
    }

    public void ChangeCheckPoint(CheckPoint checkPoint)
    {
        lastCheckpoint = checkPoint.transform.position;
        lastFlip = _playerSprite.flipY;
    }

    public void RespawnAtCheckpoint()
    {
        _rb.simulated = true;
        isDied = false;
        _rb.velocity = Vector3.zero;

        transform.position = lastCheckpoint;
        _playerSprite.flipY = lastFlip;
    }

    private IEnumerator OnPlayerDeath()
    {
        SoundManager.GetInstance().PlayEffect(hurtSound);

        _anim.SetBool("isJump", false);
        _anim.SetBool("isMove", false);
        _anim.SetTrigger("Die");

        _rb.simulated = false;
        isDied = true;

        yield return new WaitForSeconds(2f);

        RespawnAtCheckpoint();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            StartCoroutine(OnPlayerDeath());
        }
    }
}
