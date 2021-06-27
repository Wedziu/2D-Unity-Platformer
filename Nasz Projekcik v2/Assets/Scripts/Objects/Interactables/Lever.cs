using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactables
{
    public Sprite wasUsed_sp;
    public Sprite wasNotUsed_sp;

    SpriteRenderer sr;

    private GameObject player;
    public GameObject obj;
    Door door;

    private void Start()
    {
        player = GameObject.Find("Player");

        sr = GetComponent<SpriteRenderer>();
        
        door = obj.GetComponent<Door>();
        sr.sprite = wasNotUsed_sp;
    }

    public override void Interact()
    {
        if (canBeUsedAgain)
        {         
            sr.sprite = wasUsed_sp;          
            door.locked = false;       
            door.checkDoor();               
            canBeUsedAgain = false;
            player.GetComponent<Player>().CloseInteractableIcon();
            gameObject.GetComponent<Lever>().enabled = false;
        }
        else 
        {
            
        }
      
       
    }
}
