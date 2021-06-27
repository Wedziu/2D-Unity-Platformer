
using UnityEngine;

public class Door : Interactables
{
    public bool locked;

    public Sprite door_closed;
    public Sprite door_open;

    SpriteRenderer sr;
    public GameObject target;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        checkDoor();
    }

    private void Update()
    {
        checkDoor();
    }

    public override void Interact()
    {
       
        if (!locked)
        {

            SceneHandler.TransitionPlayer(target.transform.position);
        }
       
    }

    public void checkDoor()
    {
        if (locked)
        {
            sr.sprite = door_closed;
        }
        else 
        {
            sr.sprite = door_open;
        }
    }
}
