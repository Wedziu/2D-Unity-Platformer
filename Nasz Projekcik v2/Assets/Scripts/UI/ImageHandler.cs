using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ImageHandler : MonoBehaviour
{
    public Sprite keyBoardSprite;
    public Sprite GamepadSprite;
    
    Image currentImage;
    
    
    

    private void Start()
    {
        currentImage = GetComponent<Image>();
      
        ChangeSprite();
    }

    private void Update()
    {
        FindObjectOfType<Player>().getCurrentDevice();
        ChangeSprite();
    }

    private void ChangeSprite()
    {

        if (FindObjectOfType<Player>().isUsingKeyboard)
        {

            currentImage.sprite = keyBoardSprite;
            Debug.Log("klawiatura");
        }
        else
        {
            currentImage.sprite = GamepadSprite;
            Debug.Log("gamepad");
        }

        

        /*
        if (obj.currentControlScheme == "Gamepad")
        {
            currentImage.sprite = GamepadSprite;
            Debug.Log("gamepad");
        }

        if (obj.currentControlScheme == "Keyboard")
        {
            currentImage.sprite = keyBoardSprite;
            Debug.Log("klawiatura");
        }
        */
    }


}
