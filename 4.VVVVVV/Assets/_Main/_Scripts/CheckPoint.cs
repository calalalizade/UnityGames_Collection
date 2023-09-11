using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private CheckPoint[] checkpoints;
    private Color fullOpacityColor = new(1f, 1f, 1f, 1f);
    private Color fadedColor = new(1f, 1f, 1f, 0.1f);

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        checkpoints = FindObjectsOfType<CheckPoint>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().ChangeCheckPoint(this);
            spriteRenderer.color = fullOpacityColor;

            // Disable other checkpoints (or set their alpha to a lower value)
            DisableOtherCheckpoints();
        }
    }

    private void DisableOtherCheckpoints()
    {
        foreach (CheckPoint checkpoint in checkpoints)
        {
            if (checkpoint != this)
            {
                checkpoint.spriteRenderer.color = fadedColor;
            }
        }
    }
}