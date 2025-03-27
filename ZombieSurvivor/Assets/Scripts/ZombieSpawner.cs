using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombie;

    public Transform[] section = new Transform[4];

    public ZombieSO defaultZombie;
    public ZombieSO speedZombie;
    public ZombieSO tankZombie;

    private void Start()
    {
        Spawn();
    }

    private void Update()
    {
    }

    public void Spawn()
    {
        int randomSection = Random.Range(0, 4);
        float rnd = Random.value;

        GameObject zombie = Instantiate(this.zombie);

        if (rnd < 0.6f)
        {
            zombie.GetComponent<Zombie>().zombieSO = defaultZombie;
        }
        else if (rnd < 0.6f + 0.25f)
        {
            zombie.GetComponent<Zombie>().zombieSO = speedZombie;
        }
        else
        {
            zombie.GetComponent<Zombie>().zombieSO = tankZombie;
        }
        zombie.transform.position = section[randomSection].position;
    }

}
