using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] itemPrefabs;

    public float spawnRadius = 15f;


    private float itemSpawnDlay = 5f;
    private float lastItemSpawn = 10f;

    private void Update()
    {
        if (Time.time - lastItemSpawn > itemSpawnDlay)
        {
            SpawnItem();
            lastItemSpawn = Time.time;
        }
    }

    void SpawnItem()
    {
        Vector3 randomPoint = 
            transform.position + Random.insideUnitSphere * spawnRadius;
        NavMeshHit hit;

        if (NavMesh.SamplePosition
            (randomPoint, out hit, spawnRadius, NavMesh.AllAreas))
        {
            GameObject item = Instantiate
                (itemPrefabs[Random.Range(0, itemPrefabs.Length)], hit.position, Quaternion.identity);
            Destroy(item, 10f);
        }
    }
}
