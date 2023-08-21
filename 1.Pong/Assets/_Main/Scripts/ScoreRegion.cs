using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRegion : MonoBehaviour
{
    [SerializeField] AudioClip score;

    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.GetInstance();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            AudioSource.PlayClipAtPoint(score, transform.position);
            if (transform.position.x < 0)
            {
                gameManager.ComputerScoresPoint();
            }
            else if (transform.position.x > 0)
            {
                gameManager.PlayerScoresPoint();
            }
        }
    }
}

