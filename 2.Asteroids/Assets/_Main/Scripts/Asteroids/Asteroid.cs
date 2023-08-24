using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] float speed = 50.0f;
    [SerializeField] float lifeTime = 30.0f;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;

    [HideInInspector] public float size = 1.0f;
    public float minSize = 0.3f;
    public float maxSize = 1.5f;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        InitializeAsteroid(sprites, size);
        Destroy(gameObject, lifeTime);
    }

    private void InitializeAsteroid(Sprite[] sprites, float size)
    {
        _spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)]; // random asteroid
        transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f); // random start rotation
        transform.localScale = Vector3.one * size; // random start size
        _rb.mass = size * 2.0f; // set mass based on size
    }

    public void Cast(Vector2 _dir)
    {
        _rb.AddForce(_dir * speed);
    }

    private void SplitAsteroid()
    {
        for (int i = 0; i < 2; i++)
        {
            Vector2 position = transform.position;
            position += Random.insideUnitCircle * .5f;

            Asteroid semiAsteroid = Instantiate(this, position, transform.rotation);
            semiAsteroid.size = size * .5f;

            float speedMultiplier = Random.Range(1, 5);
            semiAsteroid.Cast(Random.insideUnitCircle.normalized * speedMultiplier);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            if (size * .5f >= minSize)
            {
                SplitAsteroid(); // split the asteroid into smaller parts
            }

            GameManager.GetInstance().OnAsteroidDestroyed(this);

            Destroy(gameObject);
        }
    }
}
