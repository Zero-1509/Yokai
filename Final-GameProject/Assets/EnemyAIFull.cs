using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyWorks
{
    Idle,
    Patrol,
    Detected,
    Jump,
    InAir,
    Flee,
    Attack
}
public class EnemyAIFull : MonoBehaviour
{
    public float movespeed;
    public int JS;
    public int maxDist = 10;
    [SerializeField] int MoveDir = 1;
    RaycastHit2D Playerhit;
    RaycastHit2D PlayerJumphit;
    RaycastHit2D PlayerDownhit;
    RaycastHit2D Otherhit;
    Rigidbody2D rb;
    public EnemyWorks EW;
    [SerializeField] Vector2 StartPos;
    bool isFlipped;
    bool InAir = false;
    public LayerMask PlayerMask;
    public LayerMask OtherMask;
    public LayerMask EnemyMask;

    public GameObject up;
    public GameObject Down;

    public GameObject bullet;
    bool onceFlip = true;
    public Collider2D[] cols;
    RaycastHit2D back;

    float MinDis;

    public float MyDamage;

    // Start is called before the first frame update
    void Start()
    {
        StartPos = transform.position;
        InAir = false;
        rb = GetComponent<Rigidbody2D>();
        movespeed = this.gameObject.GetComponent<Enemy_Stats>().MoveSpeed;
        MyDamage = this.gameObject.GetComponent<Enemy_Stats>().AttackPower;
        if (CompareTag("HebikawaL"))
        {
            MinDis = 1.7f;
        }
        if (CompareTag("HebikawaR"))
        {
            MinDis = 3f;
        }
        if (CompareTag("HebikawaH"))
        {
            MinDis = 4f;
        }
    }
    // Update is called once per frame
    void Update()
    {
        Playerhit = Physics2D.Raycast(transform.position, transform.right, maxDist, PlayerMask);
        PlayerJumphit = Physics2D.Raycast(up.transform.position, transform.right, maxDist, PlayerMask);
        PlayerDownhit = Physics2D.Raycast(Down.transform.position, transform.right, maxDist, PlayerMask);
        back = Physics2D.Raycast(transform.position, -transform.right, maxDist / 2, PlayerMask);
        cols = Physics2D.OverlapCircleAll(transform.position, 6f, EnemyMask);
        Debug.DrawRay(transform.position, transform.right * maxDist, Color.green);

        switch (EW)
        {
            case EnemyWorks.Idle:
                Idle();
                break;
            case EnemyWorks.Patrol:
                Patrol();
                break;
            case EnemyWorks.Detected:
                Follow();
                break;
            case EnemyWorks.Jump:
                Jump();
                break;
            case EnemyWorks.InAir:
                Air();
                break;
            case EnemyWorks.Flee:
                Run();
                break;
            case EnemyWorks.Attack:
                Attack();
                break;
            default:
                Idle();
                break;
        }
    }
    #region States
    void Idle()
    {
        if (!Playerhit && !PlayerJumphit && !PlayerDownhit)
        {
            rb.velocity = Vector2.zero;
            StartCoroutine(Delay());
        }
        else
        {
            EW = EnemyWorks.Detected;
        }
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        EW = EnemyWorks.Patrol;
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
                    if (cols[i].GetComponent<EnemyAIFull>().EW == EnemyWorks.Detected)
                    {
                        maxDist = 20;
                        if (Playerhit || PlayerJumphit || PlayerDownhit)
                            EW = EnemyWorks.Detected;

                    }
                }
            }
        
        if (Playerhit || PlayerJumphit || PlayerDownhit)
        {
            EW = EnemyWorks.Detected;
        }
    }
    void Follow()
    {
        if (Playerhit || PlayerJumphit || PlayerDownhit)
        {
            transform.position += new Vector3(movespeed * 1.5f * MoveDir * Time.deltaTime, 0);
            if (Playerhit.distance <= MinDis)
            {
                EW = EnemyWorks.Attack;
            }
        }
        else
        {
            EW = EnemyWorks.Patrol;
        }

        Otherhit = Physics2D.Raycast(transform.position, transform.right, maxDist / 2, OtherMask);
        if (Otherhit)
        {
            if (Otherhit.distance <= 1.7f)
            {
                EW = EnemyWorks.Jump;
            }
        }

    }
    void Jump()
    {
        if (!InAir)
        {
            rb.velocity = new Vector2(rb.velocity.x, JS);
            EW = EnemyWorks.InAir;
            InAir = true;
        }
        else
        {
            if (Playerhit || PlayerJumphit || PlayerDownhit)
            {
                EW = EnemyWorks.Detected;
            }
            else
            {
                EW = EnemyWorks.Patrol;
            }
        }
    }
    void Air()
    {
        float speeed;
        if (CompareTag("HebikawaH"))
            speeed = movespeed * 2;
        else
            speeed = movespeed;

        if (InAir)
            rb.velocity = new Vector2(MoveDir * speeed, rb.velocity.y);
        else
        {
            if (Playerhit || PlayerJumphit || PlayerDownhit)
            {
                EW = EnemyWorks.Detected;
            }
            else
            {
                EW = EnemyWorks.Idle;
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
                EW = EnemyWorks.Jump;
            }
        }
    }
    #region Attacks
    float StartTime = 0;
    int ShootTime = 2;
    float Starttime = 0;
    int CD = 3;
    int Wait = 2;
    void Attacking()
    {
        if (CompareTag("HebikawaL"))
        {
            Stamina_and_Health HealthPlayer = Playerhit.collider.gameObject.GetComponentInParent<Stamina_and_Health>();
            HealthPlayer.Health -= Time.deltaTime;
        }
        if (CompareTag("HebikawaR"))
        {
            if (ShootTime > StartTime)
            {
                StartTime += Time.deltaTime;
            }
            else
            {
                Instantiate(bullet, transform.position + transform.right, Quaternion.identity);
                StartTime = 0;
            }
        }
        if (CompareTag("HebikawaH"))
        {
            if (Starttime < CD)
            {
                Starttime += Time.deltaTime;
            }
            else
            {
                Starttime += Time.deltaTime;
                if(Starttime < (Wait + CD))
                {
                    rb.velocity = transform.right * 30;
                    Starttime = 0;
                    if (Playerhit)
                    {
                        EW = EnemyWorks.Attack;
                    }
                    if (back)
                    {
                        Flip();
                    }
                }
            }
        }
    }
    void Attack()
    {
        if (Playerhit || PlayerJumphit || PlayerDownhit)
        {
            if (Playerhit.distance <= MinDis)
            {
                Attacking();
            }
            else
            {
                EW = EnemyWorks.Detected;
            }
            if (CompareTag("HebikawaL") || CompareTag("HebikawaR"))
            {
                if (cols.Length == 1)
                {
                    EW = EnemyWorks.Flee;
                }
            }
        }
        
        if (back)
        {
            Flip();
        }
    }
    #endregion
    #endregion
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
        if (collision.collider.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Stamina_and_Health>().Health -= MyDamage;
            rb.velocity = Vector2.zero;
        }
    }
}
