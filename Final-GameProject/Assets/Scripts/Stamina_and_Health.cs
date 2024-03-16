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
        ShowStamina = Stamina;
        if (!BasicScript.isDashing)
        {
            Stamina += Time.deltaTime;
            StaminaSlider.value = Stamina;
        }
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
