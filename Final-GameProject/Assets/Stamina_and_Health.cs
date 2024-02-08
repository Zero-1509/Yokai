using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Stamina_and_Health : MonoBehaviour
{
    public float Stamina;
    public float Health;
    public Volume vol;
   

    public float MaxRunningTime = 10;
    public float MinRunningTime = 0;


    public ParticleSystem HealthUpdate;
    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem.MainModule mainModule = HealthUpdate.main;
        
    }

    // Update is called once per frame
    void Update()
    {
        vol.weight = Mathf.Clamp(vol.weight, 0, 0.5f);
        Stamina = Mathf.Clamp(Stamina, 0, 100);
        StaminaUpdate();
        HealthManage();

        if (Input.GetKey(KeyCode.I))
        {
            Health -= 5 * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.O))
        {
            Health += 5 * Time.deltaTime;
        }
    }
    
    void HealthManage()
    {
        var main = HealthUpdate.main;
        if (Health > 99)
        {
            main.prewarm = true;
            HealthUpdate.Stop();
        }
        if (Health <= 99)
        {
            HealthUpdate.Play();
            main.prewarm = false;
            
        }
        if (Health <= 80)
        {
            main.simulationSpeed = 2.3f;
        }
        if (Health <= 60)
        {
            main.simulationSpeed = 4.7f;
        }
        if (Health <= 40)
        {
            main.simulationSpeed = 6f;
        }
        if (Health <= 20)
        {
            main.simulationSpeed = 8.5f;
        }
    }

    void StaminaUpdate()
    {
        if (BasicScript.InMotion)
        {
            if (MinRunningTime < MaxRunningTime)
            {
                Stamina -= 0.8f;
                MinRunningTime += Time.deltaTime;
            }
            else
            {
                vol.weight += 0.1f * Time.deltaTime;
            }
        }
        else
        {
            Stamina += 1.3f;
            if(MinRunningTime >= 0)
            {
                MinRunningTime -= Time.deltaTime;
                vol.weight -= 0.09f * Time.deltaTime;
            }
        }
    }
}
