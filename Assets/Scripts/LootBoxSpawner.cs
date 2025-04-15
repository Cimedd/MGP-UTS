using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBoxSpawner : MonoBehaviour
{
    public GameObject lootboxPrefab;
    public Transform plane; // Reference to the ground/plane object
    public float dropHeight = 25f;
    public float spawnTimer = 30f;

    void Start()
    {
        SpawnLootbox();
    }

    void Update()
    {
        spawnTimer -= 1 * Time.deltaTime;
        if(spawnTimer <= 0.0f)
        {
            SpawnLootbox();
            spawnTimer= 30.0f;
        }
    }
    void SpawnLootbox()
    {
        // Get the size of the plane using its Renderer
        Renderer renderer = plane.GetComponent<Renderer>();

        float planeWidth = renderer.bounds.size.x;
        float planeLength = renderer.bounds.size.z;
        Vector3 planeCenter = plane.position;

        // Pick a random position on top of the plane within its bounds
        float randomX = Random.Range(-planeWidth / 2f, planeWidth / 2f);
        float randomZ = Random.Range(-planeLength / 2f, planeLength / 2f);

        Vector3 spawnPosition = new Vector3(
            planeCenter.x + randomX,
            planeCenter.y + dropHeight,
            planeCenter.z + randomZ
        );

        Instantiate(lootboxPrefab, spawnPosition, Quaternion.identity);
    }
}
