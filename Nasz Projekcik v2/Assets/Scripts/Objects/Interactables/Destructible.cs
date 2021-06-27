using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    GameObject placeToSpawn;
    Rigidbody2D rb;
    Animator an;
    CapsuleCollider2D cc;
    BoxCollider2D bc;




    Vector3 spawnPoint;

    bool isDestroyed;

    bool canDestroy = true;

    [SerializeField] GameObject objectPrefab;
    [SerializeField] AnimationClip anim;
    [SerializeField] float TimeToDestroy;
    [SerializeField] int objectCount;
    [SerializeField] float objectSpeed_x_min = 1.5f;
    [SerializeField] float objectSpeed_x_max = 2;
    [SerializeField] float objectSpeed_y_min = 5;
    [SerializeField] float objectSpeed_y_max = 6;

    float[] randomDirection = new float[] { -1, 1 };
    float speed_x;
    float speed_y;

    Vector2 objectDirection;
    Vector2 finalSpeed;
    int randomIndex;
    string anim_name;


    void Start()
    {
        // placeToSpawn = FindObjectOfType<GameObject>();

        an = GetComponent<Animator>();
        spawnPoint = transform.position;
        anim_name = anim.name;
    }


    void Update()
    {

    }

    void SpawnObject()
    {
        if (canDestroy)
        {
            for (int i = 0; i < objectCount; i++)
            {
                randomIndex = Random.Range(0, 2);
                speed_x = Random.Range(objectSpeed_x_min, objectSpeed_x_max);
                speed_y = Random.Range(objectSpeed_y_min, objectSpeed_y_max);

                objectDirection = new Vector2(randomDirection[randomIndex], 1);
                finalSpeed = new Vector2(objectDirection.x * speed_x, objectDirection.y * speed_y);

                placeToSpawn = Instantiate(objectPrefab, spawnPoint, Quaternion.identity);

                rb = placeToSpawn.GetComponent<Rigidbody2D>();
                bc = placeToSpawn.GetComponent<BoxCollider2D>();
                cc = placeToSpawn.GetComponent<CapsuleCollider2D>();

                cc.enabled = false;
                bc.enabled = false;

                rb.velocity += finalSpeed;

                StartCoroutine(ActivateCollider(cc, bc, 0.1f));



                //Invoke("ActivateCollider(cc,bc)", 0.1f);
            }
            canDestroy = false;
        }


    }
    void Zniszcz()
    {
        if (isDestroyed)
        {
            Destroy(gameObject);

        }
    }

    IEnumerator ActivateCollider(CapsuleCollider2D cc, BoxCollider2D bc, float time)
    {
        yield return new WaitForSeconds(time);
        cc.enabled = true;
        bc.enabled = true;

    }



    public void Damage(AttackDetails attackDeteals)
    {
        SpawnObject();


        an.Play(anim_name);
        isDestroyed = true;
        Invoke("Zniszcz", TimeToDestroy);


    }


}
