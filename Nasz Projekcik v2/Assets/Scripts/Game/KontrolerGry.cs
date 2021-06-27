using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class KontrolerGry : MonoBehaviour
{
    public static KontrolerGry instance;

    [SerializeField] int gemCount = 0;
    [SerializeField] float hp = 100;

    [SerializeField] Text gemCountText;
    [SerializeField] Text HP;

    [SerializeField] int maxStamina = 100;
    [SerializeField] Slider StaminaSlider;
    [SerializeField] Slider HealthSlider;

    public int currentStamina;
    public float currentHealth;

    private Coroutine Regen;


    private void Awake()
    {
        DontDestroyGameController();
    }

    

    private void DontDestroyGameController()
    {
       // int NumGameSession = FindObjectsOfType<KontrolerGry>().Length;

        if (instance == null)
        {
            instance = this;
        }
        else 
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void ScoreCount(int pointToAdd)
    {
        gemCount += pointToAdd;
        gemCountText.text = gemCount.ToString();
    }

    void Start()
    {
        gemCountText.text = gemCount.ToString();
        HP.text = hp.ToString();

        currentStamina = maxStamina;
        StaminaSlider.maxValue = maxStamina;
        StaminaSlider.value = maxStamina;

        currentHealth = hp;
        HealthSlider.maxValue = hp;
        HealthSlider.value = hp;
    }

    void Update()
    {
        if (hp <= 0)
        {
            if (Regen != null)
            {
                StopCoroutine(Regen);
            }
           
        }
    }
    public void TakingDamage(float Damage)
    {
        if (hp > 0)
        {
            TakeDamage(Damage);

            if(hp == 0 || hp < 0)
            {
                FindObjectOfType<Player>().CharacterDeath();

                StartCoroutine(LoadNewStatistics());
            }
        }
        else
        {

            FindObjectOfType<Player>().CharacterDeath();
            
            StartCoroutine(LoadNewStatistics());

        }
    }

    IEnumerator LoadNewStatistics()
    {
        yield return new WaitForSeconds(5);
        if(Regen != null)
        {
            StopCoroutine(Regen);
        }
     
        hp = 100;
        HP.text = hp.ToString();
        HealthSlider.value = hp;
        StaminaSlider.value = maxStamina;
        currentStamina = maxStamina;
       

        gemCount = 0;
        gemCountText.text = gemCount.ToString();
    }

    public void ResetStatistic()
    {
        if (Regen != null)
        {
            StopCoroutine(Regen);
        }

        hp = 100;
        HP.text = hp.ToString();
        HealthSlider.value = hp;
        StaminaSlider.value = maxStamina;
        currentStamina = maxStamina;


        gemCount = 0;
        gemCountText.text = gemCount.ToString();
    }


    private void TakeDamage(float Damage)
    {
        hp-=Damage;
        HP.text = hp.ToString();
        HealthSlider.value = hp;
    }

    public void ManageStaminaOnDodge(int ilosc)
    {

        if (currentStamina - ilosc >= 0)
        {
            currentStamina -= ilosc;
            StaminaSlider.value = currentStamina;

            if (Regen != null)
            {
                StopCoroutine(Regen);
            }

            if (hp > 0)
            {
                Regen = StartCoroutine(RegenStamina());
            }
          

        }     
    }

    private IEnumerator RegenStamina()
    {            
            yield return new WaitForSeconds(2);

            while (currentStamina < maxStamina)
            {              
                    currentStamina += maxStamina / 100;
                    StaminaSlider.value = currentStamina;
                    yield return new WaitForSeconds(0.1f);             
            }
        
            Regen = null;
        
    }
   
}

