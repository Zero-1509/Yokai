using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossStates{
    Resting,
    Found,
    Charge,
    Rage,
    Shockwave,
    Stun
}

public class Boss_AI : MonoBehaviour
{
    public float Speed;
    public BossStates BS;
    public float EnemyRange;
    public LayerMask Playerlayer;
    public int Direction;

    public GameObject Weapon;

    bool CanJump = true;
    // Start is called before the first frame update
    void Start()
    {
        Direction = -1;
        BS = BossStates.Resting;
        rb = GetComponent<Rigidbody2D>();
    }
    Collider2D PlayerCol;
    // Update is called once per frame
    void Update()
    {
        PlayerCol = Physics2D.OverlapCircle(transform.position,EnemyRange,Playerlayer);
        switch (BS)
        {
            case BossStates.Resting:
                Rest();
                break;
            case BossStates.Found:
                Found();
                break;
            case BossStates.Charge:
                Charging();
                break;
            case BossStates.Shockwave:
                Earthquake();
                break;
            case BossStates.Rage:
                RageMode();
                break;
            case BossStates.Stun:
                Stunned();
                break;
        }

        if(Direction == -1)
        {
            transform.rotation = Quaternion.identity;
        }
        if(Direction == 1)
        {
            transform.rotation = Quaternion.Euler(0,180,0);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, EnemyRange);
    }
    void Rest()
    {
        if (PlayerCol)
        {
            BS = BossStates.Found;
            if (PlayerCol.transform.position.x > transform.position.x)
            {
                Direction = 1;
            }
            else
            {
                Direction = -1;
            }
        }
    }
    void Found()
    {
        IsStunned = false;
        if (Vector2.Distance(transform.position,PlayerCol.transform.position) <= 4f)
        {
            BS = BossStates.Charge;
        }
        if (Vector2.Distance(transform.position,PlayerCol.transform.position) < 8f && Vector2.Distance(transform.position, PlayerCol.transform.position) > 4f)
        {
            BS = BossStates.Shockwave;
        }

        if (!PlayerCol)
        {
            BS = BossStates.Resting;
        }
    }
    public int CD;
    float Starttime = 0;
    public int Wait;
    Rigidbody2D rb;
    void Charging()
    {
        /*if (Starttime < CD)
        {
            Starttime += Time.deltaTime;
        }
        else
        {
            Starttime += Time.deltaTime;
            if (Starttime < (Wait + CD))
            {
                rb.velocity = transform.right * Direction * 30;
                Starttime = 0;
                StartCoroutine(StnTime());
            }
        }*/

        Weapon.GetComponent<HomingMissile>().Speed *= Direction;
    }
    bool IsStunned;
    void Stunned()
    {
        Debug.Log("Vulnerable");
    }

    public IEnumerator StnTime()
    {
        BS = BossStates.Stun;
        yield return new WaitForSeconds(2.5f);
        AttackJumped = false;
        BS = BossStates.Found;
    }
    void Earthquake()
    {
        if (CanJump == true && !IsStunned)
            Jump();
    }
    void RageMode()
    {

    }
    bool AttackJumped = false;
    void Jump()
    {
        rb.velocity = transform.up * 10f;
        CanJump = false;
        AttackJumped = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            rb.velocity = Vector2.zero;
        }
        if (collision.collider.CompareTag("Ground"))
        {
            IsStunned = true;
            CanJump = true;
            ShakeManager.Instance.Shake(3,1.5f);
            if(PlayerCol.GetComponent<BasicScript>().IsTouchingGround == true && AttackJumped)
            {
                PlayerCol.GetComponent<Stamina_and_Health>().Health -= 50;
                //PlayerCol.GetComponent<Stamina_and_Health>().HealthSlider.value = PlayerCol.GetComponent<Stamina_and_Health>().Health;
            }
                StartCoroutine(StnTime());
        }
    }
}
