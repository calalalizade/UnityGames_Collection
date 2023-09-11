using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    public enum MovementType
    {
        Vertical,
        Horizontal
    }

    public MovementType movementType = MovementType.Vertical;

    [SerializeField] private float moveSpeed = 15.0f;

    private Rigidbody2D _rb;
    private bool reverseVertical;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (movementType == MovementType.Vertical)
        {
            _rb.velocity = new Vector2(0, reverseVertical ? -moveSpeed : moveSpeed);
        }
        else if (movementType == MovementType.Horizontal)
        {
            _rb.velocity = new Vector2(reverseVertical ? -moveSpeed : moveSpeed, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            reverseVertical = !reverseVertical;
        }
    }
}
