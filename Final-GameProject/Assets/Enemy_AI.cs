using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum EnemyStates
{
    Patrol,
    Found,
    Jump,
    Flee,
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
    public LayerMask ExcludeLayers;
    public LayerMask CheckLayer;
    public LayerMask EnemyLayer;
    
    public Collider2D[] cols;

    public static bool InCombat;
    // Start is called before the first frame update
    void Start()
    {
        //Physics2D.queriesStartInColliders = false;
        InCombat = false;
        rb =GetComponent<Rigidbody2D>();
        ES = EnemyStates.Patrol;
        StartPos = transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        hit = Physics2D.Raycast(transform.position+transform.right, transform.right, Dis, ExcludeLayers);
        cols = Physics2D.OverlapCircleAll(transform.position, 8f, EnemyLayer);


        
        //Debug.DrawRay(transform.position, transform.right * Dis);
        if (hit)
        {
            if(transform.position.x > hit.point.x)
            {
                MoveDir = -1;
            }
            if(transform.position.x < hit.point.x)
            {
                MoveDir = 1;
            }
        }
        Debug.DrawRay(transform.position+transform.right, transform.right * Dis, Color.red);
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
            case EnemyStates.Flee:
                Flee();
                break;
            default:
                Movement();
                break;
        }
    }

    void Flee()
    {
        if (!isFlipped)
        {
            Flip();
        }
        transform.position += new Vector3(MoveDir * Movespeed * Time.deltaTime, 0);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, transform.right, 10, CheckLayer);
        if (hit2)
        {
            if (hit2.distance <= 1.7f)
            {
                ES = EnemyStates.Jump;
            }
        }
    }
    IEnumerator FlipEverySec()
    {
        yield return new WaitForSecondsRealtime(1f);
    }
    void Movement()
    {
        PlayerDetect = Physics2D.Raycast(transform.position + transform.right, transform.right, Dis, DetectMask);
        transform.position += new Vector3(MoveDir * Movespeed * Time.deltaTime, 0);


        if (cols.Length > 1)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].GetComponent<Enemy_AI>().ES == EnemyStates.Found)
                {
                    Dis = 20;
                    if (hit)
                        ES = EnemyStates.Found;
                    else if (!hit)
                    {
                        if (!IsJumped)
                        {
                            Flip();
                            StartCoroutine(FlipEverySec());
                        }
                    }
                }
            }
        }

        if (PlayerDetect)
        {
            if(cols.Length == 1)
            {
                ES = EnemyStates.Flee;
            }
            else
            {
                ES = EnemyStates.Found;
            }
        }

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
        
    }
    void Stopped()
    {
        if (!PlayerDetect)
        {
            ES = EnemyStates.Patrol;
        }
        else
        {
            if (hit && hit.distance >= 1.6f)
            {
                InCombat = true;
                Debug.Log("Gotchaaa!!");
                ES = EnemyStates.Found;
            }
            if (hit)
            {
                if (cols.Length == 1)
                {
                    Debug.Log("I beg You!!");
                    ES = EnemyStates.Flee;
                }
            }
        }
        backhit = Physics2D.Raycast(transform.position-transform.right, -transform.right, Dis / 2,ExcludeLayers);
        if (backhit) {
            Debug.Log("There You Are!!!");
            Flip();
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

        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, transform.right,10,CheckLayer);
        if (hit2)
        {
            if (hit2.distance <= 1.7f)
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
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, transform.right, 10,CheckLayer);
        if (hit2)
        {
            if (hit2.distance < 1.3f)
            {
                ES = EnemyStates.Jump;
            }
        }
        else
        {
            if (PlayerDetect && cols.Length>1)
            {
                ES = EnemyStates.Found;
            }
            else if (PlayerDetect && cols.Length==1)
            {
                ES = EnemyStates.Flee;
            }
            else if(!PlayerDetect)
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
