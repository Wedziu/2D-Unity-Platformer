using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SceneHandler : MonoBehaviour
{
    public Image fader;
    private static SceneHandler instance;
    private GameObject player;
    private PlayerInput pi;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");     
        pi = player.GetComponent<PlayerInput>();
    }

    public static void TransitionPlayer(Vector3 pos)
    {
        instance.StartCoroutine(instance.Transition(pos));
    }

    private IEnumerator Transition(Vector3 pos)
    {
        fader.gameObject.SetActive(true);

       
        pi.enabled = false;

        for (float a = 0; a < 1; a += Time.deltaTime / 0.25f)
        {
            fader.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, a));
            yield return null;
        }

        player.transform.position = pos;

     

        for (float a = 0; a < 1; a += Time.deltaTime / 0.25f)
        {
            fader.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, a));
            yield return null;
        }

       
        fader.gameObject.SetActive(false);        
       
        pi.enabled = true;
    }




}
