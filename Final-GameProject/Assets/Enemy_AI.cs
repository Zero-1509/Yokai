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
    RaycastHit2D PlayerDetect;
    public int JumpForce;
    Rigidbody2D rb;
    bool IsJumped = false;
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
        //rb.velocity = new Vector2(MoveDir* Movespeed,rb.velocity.y);
        //transform.position = new Vector3(Mathf.Clamp(transform.position.x, StartPos.x-3, StartPos.x + 3), StartPos.y);
        transform.position += new Vector3(MoveDir * Movespeed * Time.deltaTime, 0);

        if (transform.position.x >=StartPos.x + 3 || transform.position.x <=StartPos.x - 3)
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
        if(!PlayerDetect)
        {
            ES = EnemyStates.Patrol;
        }
        else
        {
            Debug.Log("You can't escape!!");
            StartCoroutine(Wait());    
        }
    }
    void Follow()
    {
        //rb.velocity = new Vector2(MoveDir * Movespeed * 2, rb.velocity.y);
        Vector2 TargetPos = new Vector2(PlayerDetect.transform.position.x-0.8f, PlayerDetect.transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, TargetPos, Movespeed*1.5f * Time.deltaTime);
        //transform.position += new Vector3(Movespeed*1.5f * Time.deltaTime, 0);
        Debug.Log("Found You");
        if (!PlayerDetect)
        {
            ES = EnemyStates.Patrol;
        }
        else
        {
            if(Vector2.Distance(transform.position,PlayerDetect.collider.transform.position)<= 0.8f)
            {
                ES = EnemyStates.Stop;
            }
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
        else
        {
            ES = EnemyStates.Found;
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
            //Debug.Log("Jumping");
        if (!hit.collider.CompareTag("Wall"))
        {
            if (PlayerDetect)
            {
                ES = EnemyStates.Found;
            }
            else
            {
                ES = EnemyStates.Patrol;
            }
        }
    }
    void Flip()
    {
        MoveDir *= -1;
        transform.Rotate(0, 180, 0);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        Flip();
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
    }
}
