using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public abstract class Interactables : MonoBehaviour
{
    public bool canBeUsedAgain;

    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }
    public abstract void Interact();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canBeUsedAgain)
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<Player>().OpenInteractableIcon();
            }
        }
       

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
       
        
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<Player>().CloseInteractableIcon();
            }
        
    }

    public bool Using()
    {
        if (canBeUsedAgain)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
