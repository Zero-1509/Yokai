using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] Enemies;
    
    
    float Health;
    float AttackPower;
    float MoveSpeed;
    float Strength;

    float SpawnStrength;
    int MaxCount;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        if (CompareTag("HebikawaL"))
        {
            Health = Random.Range(10, 25);
            AttackPower = Random.Range(5, 14);
            MoveSpeed = Random.Range(2, 4);
        }
        if (CompareTag("HebikawaR"))
        {
            Health = Random.Range(15, 30);
            AttackPower = Random.Range(3, 10);
            MoveSpeed = Random.Range(2, 4);
        }
        if (CompareTag("HebikawaH"))
        {
            Health = Random.Range(25, 40);
            AttackPower = Random.Range(10, 18);
            MoveSpeed = Random.Range(0.8f, 1.3f);
        }
        SpawnStrength = 100;
        MaxCount = 5;
    }

    public void SpawnWait()
    {
        GameObject Enemy = Instantiate(Enemies[Random.Range(0, Enemies.Length)], transform.position, Quaternion.identity);
        SpawnStrength -= Enemy.GetComponent<Enemy_Stats>().Strength;
    }
    // Update is called once per frame
    float StartTime = 0;
    int MaxTime = 4;
    void Update()
    {
        for(int i=0;i<MaxCount; i++)
        {
            if (SpawnStrength > 0)
            {
                if (MaxTime < StartTime)
                {
                    SpawnWait();
                    StartTime = 0;
                }
                else
                {
                    StartTime += Time.deltaTime;
                }
            }
            else
                break;
        }
    }
}
