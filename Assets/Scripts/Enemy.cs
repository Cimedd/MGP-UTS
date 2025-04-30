using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 5f;
    public float swarmOffset = 2f;
    public float health = 10f;
    public ParticleSystem slimeBlobFX;
    public float damageCooldown = 1.0f; // Delay between player hits
    private float lastDamageTime = 0f;
    private Vector3 swarmTarget;

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

        if (health <= 0)
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

    // NOTE: Now using OnCollisionStay instead of OnTriggerStay
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                WarriorHandler player = collision.collider.GetComponent<WarriorHandler>();
                if (player != null)
                {
                    player.TakeDamage();
                    lastDamageTime = Time.time; // Reset cooldown

                    if (slimeBlobFX != null)
                    {
                        slimeBlobFX.Play();
                    }
                }
            }
        }
    }
}
