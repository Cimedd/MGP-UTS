using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Transform player;
    public float moveSpeed = 5f;
    public float swarmOffset = 2f;
    public float health = 10;
    private Vector3 swarmTarget;
    public ParticleSystem slimeBlobFX;

    void Start()
    {
        // Slightly offset each enemy so they don't stack
        Vector2 randomCircle = Random.insideUnitCircle * swarmOffset;
        swarmTarget = new Vector3(randomCircle.x, 0, randomCircle.y);
    }

    void Update()
    {
        if (player == null) return;

        // Move towards the player with slight offset
        Vector3 targetPos = player.position + swarmTarget;
        Vector3 direction = (targetPos - transform.position).normalized;

        transform.position += direction * moveSpeed * Time.deltaTime;

        // Optional: Look at the player
        transform.forward = direction;

        if(health <= 0)
        {   
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            FindObjectOfType<UIManager>().addScore(1);
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WarriorHandler player = other.GetComponent<WarriorHandler>();
            if (player != null)
            {
                player.TakeDamage();
                if (slimeBlobFX != null)
                {
                    slimeBlobFX.Play();
                }
            }
        }
    }
}
