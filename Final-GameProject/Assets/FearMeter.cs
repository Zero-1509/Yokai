using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FearMeter : MonoBehaviour
{
    int Max_Value;
    int Min_Value;
    float My_Meter_Value;
    float Allies;
    float AlliesPercent;
    EnemyAIFull Allies_Nearby;
    Collider2D[] cols;
    public LayerMask EnemyMask;
    public static bool Check;
    // Start is called before the first frame update
    void Start()
    {
        Min_Value = 25;
        My_Meter_Value = 50;
        Max_Value = 75;
        Allies_Nearby = GetComponent<EnemyAIFull>();

    }
    // Update is called once per frame
    void Update()
    {
        cols = Physics2D.OverlapCircleAll(transform.position, 6f, EnemyMask);
        
        Allies = cols.Length;
        AlliesPercent = Allies * 100 / 4;
        My_Meter_Value = AlliesPercent;
        if (My_Meter_Value <= Min_Value && Allies_Nearby.Playerhit)
        {
            Allies_Nearby.EW = EnemyWorks.Flee;
        }
        if(My_Meter_Value >= Max_Value)
        {
            Allies_Nearby.MyDamage = 40;
        }
    }
}
