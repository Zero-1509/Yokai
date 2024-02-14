using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackStabSpawn : MonoBehaviour
{
    public GameObject BackStabber;

    int SpawnCount = 3;
    public int i;
    [SerializeField]bool isSpawned;
    // Start is called before the first frame update
    void Start()
    {
        SpawnCount = Random.Range(1,3);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Enemy_AI.InCombat && !isSpawned)
        {
            for(i = 0;i < SpawnCount; i++)
            {
                Instantiate(BackStabber,transform.position,transform.rotation);
                if (i >= SpawnCount-1)
                {
                    isSpawned = true;
                }
            }
        }
        
    }
}
