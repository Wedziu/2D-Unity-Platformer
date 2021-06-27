using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    private float timeTilSpawn;
    public float startTimeTilSpawn;

    public GameObject lazer;
    public Transform whereToSpawn;

    private void Update()
    {
        if (timeTilSpawn <= 0)
        {
            Instantiate(lazer, whereToSpawn.position, whereToSpawn.rotation);
            
            timeTilSpawn = startTimeTilSpawn;
        }
        else
        {
            timeTilSpawn -= Time.deltaTime;
        }
    }
}
