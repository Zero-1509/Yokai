using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BasicScript : MonoBehaviour
{
    public float speed = 4f;
    Vector2 moveVector;
    public Animator anim;
    bool isholding;
    Rigidbody2D rb;
    [SerializeField] float JumpForce = 7f;
    bool isFacingright;
    bool canjump;

    Vector2 newpos;
    int Btn_Pressed_Counter;
    int Sec_Combo_BtnPressed;


    public bool canDash;
    public static bool isDashing;
    private float DashingTime = 0.3f;

    public static bool InMotion;

    public GameObject AttackPoint;
    public GameObject DefendPoint;


    public bool isDefending;
    bool TouchingWall;

    [SerializeField] int Limit;
    [SerializeField] int Level;

    [SerializeField] int WallSpeed;
    [SerializeField] float GroundCheckRadius;
    [SerializeField] LayerMask GroundCheckMask;
    

    public float DashingPower;
    // Start is called before the first frame update
    void Start()
    {
        Level = PlayerPrefs.GetInt("Levels");
        canDash = true;
        isDashing = false;
        anim = GetComponent<Animator>();
        rb= GetComponent<Rigidbody2D>();
        AttackPoint.SetActive(false);
        DefendPoint.SetActive(false);
    }
    private void Update()
    {
        /*if(PlayerPrefs.GetFloat("Experience") > Limit)
        {
            Level++;
            PlayerPrefs.SetInt("Levels",Level);
            float Exp = PlayerPrefs.GetFloat("Experience") - Limit;
            PlayerPrefs.SetFloat("Experience", Exp);
        }

        if(Level == 0)
        {
            Limit = 50;
        }
        if(Level == 1)
        {
            Limit = 70;
        }
        if(Level == 2)
        {
            Limit = 80;
        }
        if(Level == 3)
        {
            Limit = 100;
        }
    }*/
        RaycastHit2D GroundDetect = Physics2D.Raycast(transform.position, Vector2.down, GroundCheckRadius,GroundCheckMask);
        Debug.DrawRay(transform.position, Vector2.down * GroundCheckRadius, Color.blue);
        if (GroundDetect)
        {
            canjump = true;
        }
        else
        {
            canjump = false;
        }

        if (moveVector.x != 0)
        {
            InMotion = true;
            transform.position = new Vector3(transform.position.x + moveVector.x*speed * Time.deltaTime, transform.position.y,0);
        }
        else
        {
            InMotion = false;
        }
    }
    // Update is called once per frame
   /* void FixedUpdate()
    {
        if (moveVector.x != 0)
        {
            InMotion = true;
            //transform.position = new Vector3(transform.position.x + moveVector.x*speed * Time.deltaTime, transform.position.y,0);
            rb.velocity = new Vector2(moveVector.x*speed*Time.fixedDeltaTime, rb.velocity.y);
        }
        else
        {
            InMotion = false;
        }
        
    }*/
    bool GrabbingWall;
    public void WallGrab(InputAction.CallbackContext context)
    {
        //GrabbingWall = context.ReadValue<float>() > 0.5f;
        {
            if (context.ReadValue<float>() > 0.5f)
            {
                GrabbingWall = true;
            }
            if (context.canceled)
            {
                rb.gravityScale = 1f;
                GrabbingWall = false;
            }
            /* if (context.duration> 0.1f)
             {
                 GrabbingWall = true;
             }
             if (context.canceled)
             {
                 GrabbingWall = false;
             }*/
        }
    }
    //float GoingTime = 0;
    public void WallClimb(InputAction.CallbackContext context)
    {
        /*if (GrabbingWall && context.ReadValue<float>() >= 0.5f)
        {
            rb.velocity = transform.up * WallSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }*/
        if (TouchingWall)
        {
            if (GrabbingWall)
            {
                if (context.action.inProgress)
                {
                    rb.gravityScale = 0f;
                    rb.velocity = transform.up*WallSpeed;
                }
                else
                {
                    rb.gravityScale = 1f;
                    rb.velocity = Vector2.zero;
                }
            }
            else
            {
                rb.gravityScale = 1f;
                rb.velocity = Vector2.zero;
            }
        }

    }
    public void OnDefend(InputAction.CallbackContext context)
    {
        if (context.duration > 0.02f)
        {
            anim.SetBool("Defend", true);
            Debug.Log("Is Blocking");
        }
        if (context.canceled)
        {
            anim.SetBool("Defend", false);
            Debug.Log("Is Not Blocking");
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
        anim.SetFloat("speed", Mathf.Abs(moveVector.x));
        if(moveVector.x > 0 && isFacingright) {
            Flip();
        }
        if(moveVector.x < 0 && !isFacingright) {
            Flip();
        }
    }
    public void OnBtnPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Btn_Pressed_Counter++;

            if (Sec_Combo_BtnPressed == 0 && Btn_Pressed_Counter == 1)
            {
                OnAttack();
                Invoke("ResetStart", 1f);
            }
            if (Sec_Combo_BtnPressed == 0 && Btn_Pressed_Counter == 2)
            {
                OnAttack2();
                Invoke("ResetStart", 1f);
            }
            if (Sec_Combo_BtnPressed == 0 && Btn_Pressed_Counter == 3)
            {
                OnAttack3();
            }
            if (Sec_Combo_BtnPressed == 1 && Btn_Pressed_Counter == 2)
            {
                //Debug.Log("Other Combo Completed!!");
                ResetStart();
            }
        }

    }
    public void OnSecBtnPressed(InputAction.CallbackContext context)
    {
        if (Btn_Pressed_Counter == 1 && context.performed)
        {
            Sec_Combo_BtnPressed++;
            if(Btn_Pressed_Counter == 1 && Sec_Combo_BtnPressed == 1)
            {
                anim.SetTrigger("Secondary_Attack_1");
                //Debug.Log("Other Combo Incomplete!!");
                Invoke("ResetStart", 1f);
            }
        }
    }
    public void OnAttack()
    {
        //Debug.Log("Combo Started!! " + Btn_Pressed_Counter);
        if (!isholding)
        {
            if (canjump)
                anim.SetTrigger("Attack");
            else
            {
                anim.SetTrigger("JumpAttack");
            }
        }
    }
    public void OnAttack2()
    {
        //Debug.Log("Combo Incomplete!! " + Btn_Pressed_Counter);
        anim.SetTrigger("Attack2");
        
    }
    public void OnAttack3()
    {
        //Debug.Log("Combo COmpleted!! " + Btn_Pressed_Counter);
        ResetStart();
        anim.SetTrigger("Attack3");
    }
    void ResetStart()
    {
        Btn_Pressed_Counter = 0;
        Sec_Combo_BtnPressed = 0;
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && canjump)
        {
            Jump();
        }
    }
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Stamina_and_Health.Stamina > 27f && !isDashing)
            {
                Debug.Log("Dashing");
                StartCoroutine(Dash());
            }
        }
    }
    public void Special(InputAction.CallbackContext context)
    {
        if (isholding)
        {
            if (context.performed)
            {
                SpecialAttack();
            }
        }
    }
    public void ishold(InputAction.CallbackContext context)
    {
        if (context.duration>0.2f)
        {
            isholding = true;
        }
        if (context.canceled)
        {
            isholding = false;
        }
        
    }
    void SpecialAttack()
    {
        //Debug.Log("Behold Special Attack!!");
        anim.SetTrigger("Special");
    }
    void Jump()
    {
        //Debug.Log("Jumped");
        canjump = false;
        rb.gravityScale = 1f;
        anim.SetTrigger("Jump");
        //rb.AddForce(transform.up * JumpForce);
        rb.velocity = new Vector2(rb.velocity.x, JumpForce*Time.fixedDeltaTime);
    }
    /*public void Hide(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            this.gameObject.SetActive(false);
        }
    }*/
    public IEnumerator Dash()
    {
        isDashing = true;
        //rb.velocity = new Vector2(DashingPower,rb.velocity.y);

        Debug.Log("Dashing rn");
        rb.velocity = new Vector2(transform.right.x * DashingPower,rb.velocity.y);
        Stamina_and_Health.Stamina -= 27;
        yield return new WaitForSeconds(DashingTime);
        isDashing = false;
    }
    void Flip()
    {
        isFacingright = !isFacingright;
        transform.Rotate(0, 180, 0);
    }
    public void OnSave()
    {
        SaveData.Save(this);
    }
    public void OnLoad()
    {
        SavingData sd = SaveData.Load();
        newpos.x = sd.position[0];
        newpos.y = sd.position[1];
        transform.position = newpos;
    }
    public void AttackActivate()
    {
        AttackPoint.SetActive(true);
    }
    public void AttackDeactivate()
    {
        AttackPoint.SetActive(false);
    }
    public void DefenceActivate()
    {
        DefendPoint.SetActive(true);
        isDefending = true;
    }
    public void DefenceDeactivate()
    {
        DefendPoint.SetActive(false);
        isDefending = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*if(collision.gameObject.CompareTag("Ground"))
        {
            canjump = true;
            rb.gravityScale = 3f;
        }*/
        if (collision.gameObject.CompareTag("Wall"))
        {
            TouchingWall = true;
        }
        if (collision.gameObject.CompareTag("DG"))
        {
            collision.gameObject.GetComponent<Animator>().enabled = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            TouchingWall = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            rb.gravityScale = 1f;
            TouchingWall = false;
        }
    }
}
