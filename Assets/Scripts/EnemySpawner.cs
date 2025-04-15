using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;
    public float spawnRadius = 5f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null) return;

        Vector2 circle = Random.insideUnitCircle * spawnRadius;

        Vector3 spawnPosition = new Vector3(
            transform.position.x + circle.x,
            transform.position.y,
            transform.position.z + circle.y
        );

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
