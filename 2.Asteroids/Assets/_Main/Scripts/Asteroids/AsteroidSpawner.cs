using System.Collections;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] Asteroid asteroidPrefab;
    [Range(1.0f, 1.5f)]
    [SerializeField] float spawnOffset = 1.2f;
    [SerializeField] float spawnAmount = 1.0f;
    [SerializeField] float spawnRate = 1.0f;
    [SerializeField] float angleVariation = 20.0f;


    IEnumerator Start()
    {
        while (true)
        {
            SpawnAsteroid();

            yield return new WaitForSeconds(spawnRate);
        }
    }

    private void SpawnAsteroid()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            Quaternion rotation = Quaternion.AngleAxis(Random.Range(-angleVariation, angleVariation), Vector3.forward);
            Vector3 spawnPoint = GetRandomOffscreenCoordinate();
            Vector3 targetDir = transform.position - spawnPoint;

            Asteroid asteroid = Instantiate(asteroidPrefab, spawnPoint, Quaternion.identity);
            asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);
            asteroid.Cast(rotation * targetDir);
        }
    }

    private Vector3 GetRandomOffscreenCoordinate()
    {
        float rnd = Random.value;

        float rX = rnd > .5f ? Random.Range(0f, 1f) : (Random.value > 0.5f ? spawnOffset : -spawnOffset % 1);
        float rY = rnd < .5f ? Random.Range(0f, 1f) : (Random.value > 0.5f ? spawnOffset : -spawnOffset % 1);

        Vector3 randomPoint = new(rX, rY, 10f);  // random point in viewport space
        Vector3 worldPoint = Camera.main.ViewportToWorldPoint(randomPoint);

        return worldPoint;
    }
}
