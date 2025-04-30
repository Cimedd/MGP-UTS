using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBox : MonoBehaviour
{
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WarriorHandler player = other.GetComponent<WarriorHandler>();
            player.addAmmo();
            Destroy(gameObject);
        }

    }
}
