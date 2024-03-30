using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Stamina_and_Health : MonoBehaviour
{
    public static float Stamina;
    public float Health;
    public float ShowStamina;

    [SerializeField] float StaminaRegenSpeed = 5;

    public Slider HealthSlider;
    public Slider StaminaSlider;
    // Start is called before the first frame update
    void Start()
    {
        Stamina = 100;
    }

    // Update is called once per frame
    void Update()
    {
        HealthManage();
        Stamina = Mathf.Clamp(Stamina, 0, 100);
        StaminaSlider.value = Stamina;
        Health = Mathf.Clamp(Health, 0, 100);
        HealthSlider.value = Health;
        ShowStamina = Stamina;
        if (!BasicScript.isDashing)
        {
            Stamina += Time.deltaTime*StaminaRegenSpeed;
            StaminaSlider.value = Stamina;
        }
        if (StaminaRegenSpeed > 5)
        {
            if (ResetTime > 0)
            {
                ResetTime -= Time.deltaTime;
            }
            else
            {
                ResetTime = 5;
                StaminaRegenSpeed = 5f;
            }
        }
    }
    float ResetTime = 5f;
    public void StaminaBoost()
    {
        StaminaRegenSpeed = 12;
    }
    public void HealthUP()
    {
        Health += 30;
        HealthSlider.value = Health;
    }


    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void HealthManage()
    {
        if (Health <= 0)
        {
            RestartScene();
        }
    }

    
}
