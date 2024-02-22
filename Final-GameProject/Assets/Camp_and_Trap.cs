using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camp_and_Trap : MonoBehaviour
{

    public GameObject Camp;
    public GameObject Trap;

    public Vector2[] TrapPoints;
    public Vector2[] CampPoints;
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0;i<2; i++)
        {
            Instantiate(Trap, new Vector2(Random.Range(-17, 37), 0.5f), Quaternion.identity);
        }
        for(int i=0;i<3; i++)
        {
            Instantiate(Camp, new Vector2(Random.Range(-17, 37), 0.5f), Quaternion.identity);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
