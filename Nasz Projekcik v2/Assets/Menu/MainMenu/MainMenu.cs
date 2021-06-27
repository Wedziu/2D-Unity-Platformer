using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public bool checkKontroler = false;
    GameObject kontrolerGry;

    public void Awake()
    { 
         if (GameObject.Find("KontrolerGry"))
        {
            
            kontrolerGry = GameObject.Find("KontrolerGry");
            checkKontroler = true;
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Level testowy v3");
        if (checkKontroler)
        {         
            kontrolerGry.GetComponent<KontrolerGry>().ResetStatistic();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
