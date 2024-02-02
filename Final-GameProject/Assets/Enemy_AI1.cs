using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Enemy_Pos
{
    back,
    front
}
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
    public Enemy_Pos Back_Front;
    float StopDis = 1.6f;
    // Start is called before the first frame update

    private void Awake()
    {
        float R = Random.Range(0, 1);
        if (R <= 0.1f)
        {
            Back_Front = Enemy_Pos.back;
        }
        else
        {
            Back_Front = Enemy_Pos.front;
        }
    }
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
        hit = Physics2D.Raycast(transform.position, transform.right, Dis);
        backhit = Physics2D.Raycast(transform.position, -transform.right, Dis/2);
        
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
            ES = EStates.Found;
        }
        
    }
    
    void Stopped()
    {
        if (!PlayerDetect)
        {
            ES = EStates.Patrol;
        }
        else
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (Vector2.Distance(transform.position,hit.point) >= StopDis)
                {
                    ES = EStates.Found;
                }
            }
        }
        if (backhit.collider.CompareTag("Player"))
        {
            if (!isFlipped)
            {
                Flip();
                ES = EStates.Found;
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
            if (Vector2.Distance(transform.position, hit.point) <= StopDis)
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
