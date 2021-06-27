using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Chest : Interactables
{
    public Sprite open;
    public Sprite closed;

    SpriteRenderer sr;
    private bool isOpen;


    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = closed;
    }

    public override void Interact()
    {
        if (isOpen)
        {
            sr.sprite = closed;
        }
        else
        {
            sr.sprite = open;
        }

        isOpen = !isOpen;
    }

}
