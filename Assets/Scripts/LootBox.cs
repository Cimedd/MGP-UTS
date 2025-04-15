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
        if (other.CompareTag("MainCamera"))
        {
            WarriorHandler player = other.GetComponent<WarriorHandler>();
            player.ammo += 10;
            GameObject.Destroy(this);
        }

    }
}
