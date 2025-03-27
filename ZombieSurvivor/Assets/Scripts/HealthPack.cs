using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour, IItem
{
    public float healAmount = 20;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Pick(other.GetComponent<Woman>());
        }
    }

    public void Pick(Woman woman)
    {
        woman.health += healAmount;
        if (woman.health > woman.maxHealth)
        {
            woman.health = woman.maxHealth;
        }
        Destroy(gameObject);
    }
}
