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
    public bool isDashing;
    private float DashingTime = 0.3f;
    private float DashCD = 5f;

    public static bool InMotion;

    public GameObject AttackPoint;
    public GameObject DefendPoint;


    public bool isDefending;

    [SerializeField] int Limit;
    [SerializeField] int Level;
    /*private void Awake()
    {
        OnLoad();
    }*/
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
    }
    // Update is called once per frame
    void FixedUpdate()
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
                //Debug.Log("Other Combo Incomplete!!");
                Invoke("ResetStart", 1f);
            }
        }
    }
    public void OnAttack()
    {
        //Debug.Log("Combo Started!! " + Btn_Pressed_Counter);
        anim.SetTrigger("Attack");
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
            StartCoroutine(Dash());
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
    }
    void Jump()
    {
        //Debug.Log("Jumped");
        canjump = false;
        rb.gravityScale = 1f;
        rb.AddForce(transform.up * JumpForce);
    }
    public void Hide(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            this.gameObject.SetActive(false);
        }
    }
    public IEnumerator Dash()
    {
        isDashing = true;
        if (isDashing && canDash)
        {
            //Debug.Log("Dashing!!");
            rb.velocity = transform.right * -10;
            yield return new WaitForSeconds(DashingTime);
        }
        canDash = false;
        isDashing = false;
        yield return new WaitForSeconds(DashCD);
        canDash = true;
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
        if(collision.gameObject.CompareTag("Ground"))
        {
            canjump = true;
            rb.gravityScale = 3f;
        }
    }
}
