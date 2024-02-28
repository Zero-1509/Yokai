using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

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
        HealthManage();
    }


    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void HealthManage()
    {
        var main = HealthUpdate.main;
        /*if (Health > 99)
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
        }*/
        if (Health <= 0)
        {
            RestartScene();
        }
    }

    
}
