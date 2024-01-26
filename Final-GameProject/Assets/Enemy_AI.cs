using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStates
{
    Patrol,
    Found,
    Jump,
    Stop
}
public class Enemy_AI : MonoBehaviour
{
    public float Movespeed;
    public int Dis;
    [SerializeField]int MoveDir;
    public Vector2 StartPos;
    RaycastHit2D hit;
    public LayerMask DetectMask;
    public LayerMask EnemyMask;
    RaycastHit2D PlayerDetect;
    public int JumpForce;
    Rigidbody2D rb;
    bool IsJumped = false;
    public EnemyStates ES;
    Vector2 TargetPos;
    public GameObject PlayerPos;

    public Collider2D[] Overlaps;
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        ES = EnemyStates.Patrol;
        StartPos = transform.position;
        MoveDir = 1;
    }

    // Update is called once per frame
    void Update()
    {
        hit = Physics2D.Raycast(transform.position, transform.right, Dis);
        //Debug.DrawRay(transform.position, transform.right * Dis);

        Debug.DrawRay(transform.position, transform.right * Dis, Color.red);
        Overlaps = Physics2D.OverlapCircleAll(transform.position, 5, EnemyMask);
        for (int i = 0; i > Overlaps.Length; i++)
        {
            if (Overlaps[i].gameObject.GetComponent<EnemyStates>() == EnemyStates.Found)
            {
                    //PlayerPos = Overlaps[i].transform.gameObject;
                    ES = EnemyStates.Found;
            }
            else
            {
                break;
            }
        }
        switch (ES)
        {
            case EnemyStates.Patrol:
                Movement();
                break;
            case EnemyStates.Found:
                Follow();
                break;
            case EnemyStates.Jump:
                Jumping();
                break;
            case EnemyStates.Stop:
                Stopped();
                break;
            default:
                Movement();
                break;
        }

    }

    void Movement()
    {
        PlayerDetect = Physics2D.Raycast(transform.position, transform.right, Dis, DetectMask);
        //rb.velocity = new Vector2(MoveDir* Movespeed,rb.velocity.y);
        transform.position += new Vector3(MoveDir * Movespeed * Time.deltaTime, 0);

        if (transform.position.x >= StartPos.x + 3 || transform.position.x <= StartPos.x - 3)
        {
            Flip();
        }


        if (PlayerDetect)
        {
            ES = EnemyStates.Found;
        }
        else
        {
            ES = EnemyStates.Patrol;
        }
        
    }
    void Stopped()
    {
        if (hit.collider.CompareTag("Player"))
        {
            ES = EnemyStates.Found;
        }
        else
        {
            StartPos = transform.position;
            StartCoroutine(Wait());
        }
    }
    void Follow()
    {
        //rb.velocity = new Vector2(MoveDir * Movespeed * 2, rb.velocity.y);
        PlayerPos = PlayerDetect.collider.gameObject;
        if (PlayerPos != null)
        {
            TargetPos = new Vector2(PlayerPos.transform.position.x + 2f, PlayerPos.transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, TargetPos, Movespeed *1.5f* Time.deltaTime);
            Debug.Log("Found You");
            if (!PlayerDetect)
            {
                ES = EnemyStates.Patrol;
            }

            if (Physics2D.Raycast(transform.position,-transform.right,2f,DetectMask))
            {
                Debug.DrawRay(transform.position, -transform.right*2f,Color.blue);
                Flip();
            }
            if (transform.position.x >= TargetPos.x)
            {
                ES = EnemyStates.Stop;
            }
            if (hit.collider.CompareTag("Wall"))
            {
                if (hit.distance < 1.3f)
                {
                    ES = EnemyStates.Jump;
                }
                else
                {
                    ES = EnemyStates.Found;
                }
            }

        }

    }
    void Jumping()
    {
        if (!IsJumped)
        {
            rb.velocity=new Vector2(rb.velocity.x, JumpForce);
            IsJumped = true;
        }
        else{
            rb.velocity = new Vector2(MoveDir* Movespeed,rb.velocity.y);
        }
    }
    void Flip()
    {
        MoveDir *= -1;
        transform.Rotate(0, 180, 0);
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            IsJumped = false;
            if (PlayerDetect)
            {
                ES = EnemyStates.Found;
            }
            else
            {
                ES = EnemyStates.Patrol;
            }
        }
        if (collision.collider.CompareTag("Wall"))
        {
            Flip();
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);

        ES = EnemyStates.Patrol;
    }
}
