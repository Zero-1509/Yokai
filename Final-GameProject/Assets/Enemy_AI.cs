using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    RaycastHit2D backhit;
    public LayerMask DetectMask;
    RaycastHit2D PlayerDetect;
    public int JumpForce;
    Rigidbody2D rb;
    bool IsJumped = false;
    bool isFlipped;
    public EnemyStates ES;
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
        backhit = Physics2D.Raycast(transform.position, -transform.right, Dis/2);
        
        //Debug.DrawRay(transform.position, transform.right * Dis);

        Debug.DrawRay(transform.position, transform.right * Dis, Color.red);

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
        transform.position += new Vector3(MoveDir * Movespeed * Time.deltaTime, 0);

        if (transform.position.x >= StartPos.x + 3 || transform.position.x <= StartPos.x - 3)
        {
            if (!isFlipped)
            {
                Flip();
            }
        }
        else
        {
            isFlipped = false;
        }
        
        if (PlayerDetect)
        {
            ES = EnemyStates.Found;
        }
        
    }
    void Stopped()
    {
        if (!PlayerDetect)
        {
            ES = EnemyStates.Patrol;
        }
        else
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (hit.distance >= 1.6f)
                {
                    ES = EnemyStates.Found;
                }
            }
        }
        if (backhit.collider.CompareTag("Player"))
        {
            if (!isFlipped)
            {
                Flip();
                ES = EnemyStates.Found;
            }
        }
        else
        {
            isFlipped = false;
        }
    }
    void Follow()
    {
        transform.position += new Vector3(Movespeed*1.5f*MoveDir * Time.deltaTime, 0);
        Debug.Log("Found You");
        if (PlayerDetect)
        {
            if (hit.distance <= 1.6f)
            {
                Debug.Log("Detected");
                ES = EnemyStates.Stop;
            }
        }
        else
        {
            ES = EnemyStates.Stop;
        }
        

        if (hit.collider.CompareTag("Wall"))
        {
            if (hit.distance <= 1.7f)
            {
                ES = EnemyStates.Jump;
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
        if (hit.collider.CompareTag("Wall"))
        {
            if (hit.distance < 1.3f)
            {
                ES = EnemyStates.Jump;
            }
        }
        else
        {
            if (PlayerDetect)
            {
                ES = EnemyStates.Found;
            }
            else
            {
                ES = EnemyStates.Stop;
            }
        }
    }
    void Flip()
    {
        isFlipped = true;
        MoveDir *= -1;
        transform.Rotate(0, 180, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            IsJumped = false;
            if (!IsJumped)
            {
                StartPos = transform.position;
            }

        }
        if (collision.collider.CompareTag("Wall"))
        {
            Flip();
        }
    }
}
