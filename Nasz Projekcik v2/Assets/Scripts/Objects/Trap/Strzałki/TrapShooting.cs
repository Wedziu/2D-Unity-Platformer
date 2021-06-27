using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapShooting : MonoBehaviour
{
    float shotCounter;
    [SerializeField] float minTimeBetweenShots = 1.5f;
    [SerializeField] float maxTimeBetweenShots = 2f;
    public GameObject laserPrefab;


    private void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    private void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if(shotCounter <=0f)
        {
            shootBullet();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    public void shootBullet()
    {
        GameObject b = Instantiate(laserPrefab,  new Vector3(0,0,0), Quaternion.identity);
        b.transform.parent = gameObject.transform.Find("placeFromShoot");
        b.transform.localPosition = Vector3.zero;
        


    }
}

