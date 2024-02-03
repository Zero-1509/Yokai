using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EStates
{
    Patrol,
    Found,
    Jump,
    Stop
}
public class Enemy_AI1 : MonoBehaviour
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
    public EStates ES;
    float StopDis = 1.6f;
    // Start is called before the first frame update
    
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        ES = EStates.Patrol;
        StartPos = transform.position;
        MoveDir = 1;
    }

    // Update is called once per frame
    void Update()
    {
        CastingRay();
        backhit = Physics2D.Raycast(transform.position, -transform.right, Dis / 2,DetectMask);

        //Debug.DrawRay(transform.position, transform.right * Dis);

        Debug.DrawRay(transform.position, transform.right * Dis, Color.red);

        switch (ES)
        {
            case EStates.Patrol:
                Movement();
                break;
            case EStates.Found:
                Follow();
                break;
            case EStates.Jump:
                Jumping();
                break;
            case EStates.Stop:
                Stopped();
                break;
            default:
                Movement();
                break;
        }

        PlayerDetect = Physics2D.Raycast(transform.position, transform.right, Dis, DetectMask);

    }

    float Gap(Vector3 pos1, Vector3 pos2)
    {
        float gap = pos1.x - pos2.x;
        return gap;
    }
    void Movement()
    {
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
            ES = EStates.Found;
        }
        
    }
    RaycastHit2D CastingRay()
    {
        hit = Physics2D.Raycast(transform.position, transform.right, Dis);
        return hit;
    }

    void Stopped()
    {
        if (!PlayerDetect)
        {
            if (!isFlipped)
            {
                Flip();
                if (PlayerDetect)
                {
                    ES = EStates.Found;
                }
            }
            else{
                isFlipped = false;
                ES = EStates.Patrol;
            }
        }
        else
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (Gap(transform.position,hit.point) >= StopDis)
                {
                    ES = EStates.Found;
                }
            }
            else
            {
                if (!isFlipped)
                {
                    Flip();
                    if (PlayerDetect)
                    {
                        ES = EStates.Found;
                    }
                }
                else
                {
                    isFlipped = false;
                    ES = EStates.Patrol;
                }
            }
        }
    }
    void Follow()
    {
        transform.position += new Vector3(Movespeed*1.5f*MoveDir * Time.deltaTime, 0);
        Debug.Log("Found You");
        if (PlayerDetect)
        {
            if (Gap(transform.position, hit.point) <= StopDis)
            {
                Debug.Log("Detected");
                ES = EStates.Stop;
            }
        }
        else
        {
            ES = EStates.Stop;
        }
        

        if (hit.collider.CompareTag("Wall"))
        {
            if (hit.distance <= 1.7f)
            {
                ES = EStates.Jump;
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
                ES = EStates.Jump;
            }
        }
        else
        {
            if (PlayerDetect)
            {
                ES = EStates.Found;
            }
            else
            {
                ES = EStates.Stop;
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
            if (!PlayerDetect)
            {
                ES = EStates.Patrol;
            }
            else
            {
                ES = EStates.Patrol;
            }
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
