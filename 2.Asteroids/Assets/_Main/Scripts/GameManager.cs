using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] SpaceShipController player;
    [SerializeField] GameObject explosionPlayer;
    [SerializeField] ParticleSystem explosionAsteroid;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] int totalLives = 3;
    [SerializeField] int respawnCooldown = 2;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] LiveSystem liveSystem;

    private int remainingLives;
    private int score;

    private void Start()
    {
        remainingLives = totalLives;
        NewGame();
    }

    public void NewGame()
    {
        Asteroid[] asteroids = FindObjectsOfType<Asteroid>();

        for (int i = 0; i < asteroids.Length; i++)
        {
            Destroy(asteroids[i].gameObject);
        }

        gameOverUI.SetActive(false);

        SetScore(0);
        SetLives(totalLives);
        StartCoroutine(RespawnPlayer(0f));
    }

    private void SetLives(int lives)
    {
        remainingLives = lives;
        liveSystem.UpdateUI(lives); // Updates UI with player's remaining lives.
    }

    private void SetScore(int _score)
    {
        score = _score;
        scoreText.text = _score.ToString();
    }

    public void OnAsteroidDestroyed(Asteroid asteroid)
    {
        if (asteroid.size < 0.7f)
        {
            SetScore(score + 100); // small
        }
        else if (asteroid.size < 1.2f)
        {
            SetScore(score + 50); // medium
        }
        else
        {
            SetScore(score + 20); // large
        }

        explosionAsteroid.transform.position = asteroid.transform.position;
        explosionAsteroid.transform.localScale = Vector3.one * asteroid.size;
        explosionAsteroid.Play();
    }

    public void OnPlayerDeath(SpaceShipController _player)
    {
        _player.gameObject.SetActive(false);

        // Spawn explosion effect
        GameObject _explosion = Instantiate(explosionPlayer, _player.transform.position, Quaternion.identity);

        SetLives(remainingLives - 1);

        if (remainingLives <= 0)
        {
            gameOverUI.SetActive(true);
        }
        else
        {
            StartCoroutine(RespawnPlayer(respawnCooldown));
        }

    }

    private IEnumerator RespawnPlayer(float _coolDown)
    {
        yield return new WaitForSeconds(_coolDown);
        player.gameObject.SetActive(true);
        player.transform.position = Vector3.zero;
    }
}
