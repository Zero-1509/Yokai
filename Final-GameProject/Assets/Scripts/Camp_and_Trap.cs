using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camp_and_Trap : MonoBehaviour
{

    public GameObject Camp;
    public GameObject Trap;

    public GameObject Trapp;
    public GameObject Enemy;
    public float XPoint;

    Vector2[] Range_Trap;
    Vector2[] Range_Spawner_1;

    // Start is called before the first frame update
    void Start()
    {
        Range_Trap = new Vector2[2];
        Range_Spawner_1 = new Vector2[3];
        Range_Trap[0] = new Vector2(Random.Range(-25, -18), 0.5f);
        Range_Trap[1] = new Vector2(Random.Range(5, 18), 0.5f);

        Range_Spawner_1[0] = new Vector2(Random.Range(-20, -14), 0.5f);
        Range_Spawner_1[1] = new Vector2(Random.Range(2.3f, 10.2f), 0.5f);
        Range_Spawner_1[2] = new Vector2(Random.Range(15, 22), 0.5f);
        for (int i = 0; i < 2; i++)
        {
            Trapp = Instantiate(Trap, Range_Trap[i], Quaternion.identity,transform.parent);
        }
        for (int i = 0; i < 3; i++)
        {
            Enemy = Instantiate(Camp, Range_Spawner_1[i], Quaternion.identity,transform.parent);
        }
    }

    void Update()
    {
        
    }
}
