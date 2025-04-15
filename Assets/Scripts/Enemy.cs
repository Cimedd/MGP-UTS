using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
/*    public Transform goal;*/
    /* // Start is called before the first frame update
     void Start()
     {

     }

     // Update is called once per frame
     void Update()
     {

     }*/

    public Transform player;
    public float moveSpeed = 5f;
    public float swarmOffset = 2f;
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
    }
}
