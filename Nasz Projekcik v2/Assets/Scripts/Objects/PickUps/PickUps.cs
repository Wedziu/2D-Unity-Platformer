using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUps : MonoBehaviour
{
    
    [SerializeField] int gemValue = 1;
    private new AudioSource audio;
    public LayerMask layer;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            AudioSource.PlayClipAtPoint(audio.clip, transform.position);
            FindObjectOfType<KontrolerGry>().ScoreCount(gemValue);
            Destroy(gameObject);
        }
    }


}
