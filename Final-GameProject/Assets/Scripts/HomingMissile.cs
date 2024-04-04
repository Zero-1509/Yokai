using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    Rigidbody2D rb;
    bool Thrown;
    bool WallStuck;
    Vector2 StartPos;
    public float Speed = 120;

    public bool EnemyUsed;
    // Start is called before the first frame update
    void Start()
    {
        StartPos = transform.position;
    }

    public void Throw()
    {
        rb.velocity = transform.right * Speed* Time.deltaTime;
        Thrown = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (EnemyUsed)
        {
            Throw();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            WallStuck = true;
        }
    }
    void Return()
    {
        transform.position = Vector3.MoveTowards(transform.position, StartPos, 20 * Time.deltaTime);
    }

    float RetTime = 0;
    float RetWait = 3f;
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            if(WallStuck)
            {
                if (RetTime < RetWait)
                {
                    RetTime += Time.deltaTime;
                }
                else
                {
                    RetTime = 0;
                }
            }
        }
    }
}
