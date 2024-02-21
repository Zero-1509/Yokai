using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NewEnemyAI : MonoBehaviour
{
    public int movespeed;
    public int JS;
    public int maxDist = 10;
    [SerializeField] int MoveDir=1;
    RaycastHit2D Playerhit;
    RaycastHit2D PlayerJumphit;
    RaycastHit2D PlayerDownhit;
    RaycastHit2D Otherhit;
    Rigidbody2D rb;
    public EnemyStates EW;
    [SerializeField]Vector2 StartPos;
    bool isFlipped;
    bool InAir = false;
    public LayerMask PlayerMask;
    public LayerMask OtherMask;
    public LayerMask EnemyMask;

    public GameObject up;
    public GameObject Down;

    bool onceFlip = true;
    public Collider2D[] cols;

    // Start is called before the first frame update
    void Start()
    {
        StartPos = transform.position;
        InAir = false;
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        Playerhit = Physics2D.Raycast(transform.position, transform.right, maxDist, PlayerMask);
        PlayerJumphit = Physics2D.Raycast(up.transform.position, transform.right, maxDist, PlayerMask);
        PlayerDownhit = Physics2D.Raycast(Down.transform.position, transform.right, maxDist, PlayerMask);

        cols = Physics2D.OverlapCircleAll(transform.position, 6f, EnemyMask);
        Debug.DrawRay(transform.position, transform.right*maxDist, Color.green);
        /*if (Playerhit)
        {
            if (transform.position.x > Playerhit.point.x)
            {
                MoveDir = -1;
            }
            if (transform.position.x < Playerhit.point.x)
            {
                MoveDir = 1;
            }
        }*/


        switch (EW)
        {
            case EnemyStates.Idle:
                Idle();
                break;
            case EnemyStates.Patrol:
                Patrol();
                break;
            case EnemyStates.Detected:
                Follow();
                break;
            case EnemyStates.Jump:
                Jump();
                break;
            case EnemyStates.InAir:
                Air();
                break;
            case EnemyStates.Flee:
                Run();
                break;
            case EnemyStates.Attack:
                Attack();
                break;
            default:
                Idle();
                break;
        }
    }
    void Idle()
    {
        if (!Playerhit && !PlayerJumphit && !PlayerDownhit)
        {
            rb.velocity = Vector2.zero;
            StartCoroutine(Delay());
        }
        else
        {
            if (cols.Length == 1)
            {
                EW = EnemyStates.Flee;
            }
        }
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        EW = EnemyStates.Patrol;
    }
    void Patrol()
    {
        if (!Playerhit && !PlayerJumphit && !PlayerDownhit)
        {
            transform.position += new Vector3(movespeed * MoveDir * Time.deltaTime, 0);
            if (transform.position.x >= StartPos.x + 3 || transform.position.x <= StartPos.x - 3)
            {
                if (!isFlipped)
                {
                    StartCoroutine(Delay());
                    Flip();
                }
            }
            else
            {
                isFlipped = false;
            }
        }

        if (cols.Length > 1)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].GetComponent<NewEnemyAI>().EW == EnemyStates.Detected || cols[i].GetComponent<Heavy_Enemy_AI>().EW == EnemyStates.Detected || cols[i].GetComponent<Ranged_Enemy_AI>().EW == EnemyStates.Detected)
                {
                    EW = EnemyStates.Detected;
                }
            }
        }

        if (Playerhit || PlayerJumphit || PlayerDownhit)
        {
            EW = EnemyStates.Detected;
        }
    }
    void Follow()
    {
        if (Playerhit || PlayerJumphit || PlayerDownhit)
        {
            transform.position += new Vector3(movespeed * 1.5f * MoveDir * Time.deltaTime, 0);
            if (Playerhit.distance <= 2)
            {
                EW = EnemyStates.Attack;
            }
        }
        else
        {
            EW = EnemyStates.Patrol;
        }
        Otherhit = Physics2D.Raycast(transform.position, transform.right, maxDist / 2, OtherMask);
        if (Otherhit)
        {
            if (Otherhit.distance <= 1.7f)
            {
                EW = EnemyStates.Jump;
            }
        }

    }
    void Jump()
    {
        if (!InAir)
        {
            rb.velocity = new Vector2(rb.velocity.x, JS);
            EW = EnemyStates.InAir;
            InAir = true;
        }
        else
        {
            if (Playerhit || PlayerJumphit || PlayerDownhit)
            {
                EW = EnemyStates.Detected;
            }
            else
            {
                EW = EnemyStates.Patrol;
            }
        }
    }
    void Air()
    {
        if(InAir)
            rb.velocity = new Vector2(MoveDir * movespeed, rb.velocity.y);
        else
        {
            if (Playerhit || PlayerJumphit || PlayerDownhit)
            {
                EW = EnemyStates.Detected;
            }
            else
            {
                EW = EnemyStates.Idle;
            }
        }
    }
    void Run()
    {
        if (onceFlip)
        {
            Flip();
            onceFlip = false;
        }
        transform.position += new Vector3(MoveDir * movespeed * Time.deltaTime, 0);
        Otherhit = Physics2D.Raycast(transform.position, transform.right, maxDist / 2, OtherMask);
        if (Otherhit)
        {
            if (Otherhit.distance <= 1.7f)
            {
                EW = EnemyStates.Jump;
            }
        }
    }
    void Attacking() {
        if (cols.Length <= 1)
        {
            EW = EnemyStates.Flee;
        }
        Stamina_and_Health HealthPlayer = Playerhit.collider.gameObject.GetComponentInParent<Stamina_and_Health>();
        HealthPlayer.Health -= Time.deltaTime;
    }
    
    void Attack()
    {
        if (Playerhit || PlayerJumphit || PlayerDownhit)
        {
            if (Playerhit.distance <= 2)
            {
                Attacking();
            }
            else
            {
                EW = EnemyStates.Detected;
            }

            if (cols.Length == 1)
            {
                EW = EnemyStates.Flee;
            }
        }
        RaycastHit2D back = Physics2D.Raycast(transform.position,-transform.right,maxDist/2,PlayerMask);
        if (back)
        {
            Flip();
        }
    }
    void Flip()
    {
        isFlipped = true;
        onceFlip = true;
        MoveDir *= -1;
        transform.Rotate(0, 180, 0);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            InAir = false;
        }
        if (collision.collider.CompareTag("Wall"))
        {
            Flip();
        }
    }
}
