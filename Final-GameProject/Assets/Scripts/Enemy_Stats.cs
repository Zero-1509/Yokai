using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Stats : MonoBehaviour
{
    public float Health;
    public float AttackPower;
    public float MoveSpeed;
    public float Strength;

    private void Awake()
    {
        if (CompareTag("HebikawaL"))
        {
            Health = Random.Range(10, 25);
            AttackPower = Random.Range(5, 14);
            MoveSpeed = Random.Range(0.7f, 1.9f);
        }
        if (CompareTag("HebikawaR"))
        {
            Health = Random.Range(15, 30);
            AttackPower = Random.Range(3, 10);
            MoveSpeed = Random.Range(0.7f, 1.9f);
        }
        if (CompareTag("HebikawaH"))
        {
            Health = Random.Range(25, 40);
            AttackPower = Random.Range(10, 18);
            MoveSpeed = Random.Range(0.3f, 1f);
        }
        Strength = (Health + AttackPower)/2;
    }
    // Update is called once per frame
   

    public void HUpdates(float DamageTaken)
    {
        Health -= DamageTaken;
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
    
}
