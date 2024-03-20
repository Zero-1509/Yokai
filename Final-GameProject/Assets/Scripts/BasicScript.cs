using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BasicScript : MonoBehaviour
{

    public float speed = 4f;    //Player Speed
    Vector2 moveVector;         //Input Vector
    bool isFacingright;         //Facing
    public Animator anim;       //Animation
    Rigidbody2D rb;             //Physics Rigidbody
    
    public Transform GroundObj;             //GroundDetect Pos
    bool canjump;                           //Jump Count
    [SerializeField] float JumpForce = 7f;  //Jump Power
    
    public bool canDash;                        //DashRestrict
    public static bool isDashing;               //Checking Dash
    public float DashingTime = 0.1f;            //Time for which Dash is activated
    public float DashingPower;                  //Power for Dash
    public static bool InMotion;                //Check if character in motion
    bool TouchingWall;                          //Check if Touching the wall
    bool GrabbingWall;                          //Check if Player is grabbing wall 
    [SerializeField] int WallSpeed;             //Speed of going up
    [SerializeField] float GroundCheckRadius;   //Radius for checking ground dis
    [SerializeField] LayerMask GroundCheckMask; //Layer of ground
    

    bool isholding;                 //SpecialAttack P1
    public bool isDefending;        //Check if character is defending (Not in game for now)
    int Btn_Pressed_Counter;        //Combo Count Check;
    int Sec_Combo_BtnPressed;       //Second Combo Check
    public GameObject AttackPoint;  //For activating & deactivating Attack after 1 use
    public GameObject DefendPoint;  //For activating & deactivating Defence Point


    Vector2 newpos; // SaveVector

    [SerializeField] int Limit;
    [SerializeField] int Level;

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
    

    // Start is called before the first frame update

    #region Movement
        private void Update(){
            Collider2D GroundDetect = Physics2D.OverlapCircle(GroundObj.position, GroundCheckRadius, GroundCheckMask);
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
                transform.position = new Vector3(transform.position.x + moveVector.x * speed * Time.deltaTime, transform.position.y, 0);
            }
            else
            {
                InMotion = false;
            }
        }
        public void WallGrab(InputAction.CallbackContext context)
        {
            if (context.ReadValue<float>() > 0.5f)
            {
                GrabbingWall = true;
            }
            if (context.canceled)
            {
                rb.gravityScale = 1f;
                rb.drag = 1;
                GrabbingWall = false;
            }
       
        }
        public void WallClimb(InputAction.CallbackContext context)
        {
            if (TouchingWall)
            {
                if (GrabbingWall)
                {
                    if (context.action.inProgress)
                    {
                        rb.gravityScale = 0f;
                        rb.drag = 0;
                        rb.velocity = transform.up*WallSpeed;
                    }
                    else
                    {
                        rb.gravityScale = 1f;
                        rb.drag = 1; 
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
        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                moveVector = context.ReadValue<Vector2>();
            }
            if (context.canceled)
            {
                moveVector = Vector2.zero;
            }
            anim.SetFloat("speed", Mathf.Abs(moveVector.x));
            if(moveVector.x > 0 && isFacingright) {
                Flip();
            }
            if(moveVector.x < 0 && !isFacingright) {
                Flip();
            }
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
                    StartCoroutine(Dash());
                }
            }
        }
        void Jump()
        {
            canjump = false;
            rb.gravityScale = 1f;
            anim.SetTrigger("Jump");
            rb.velocity = new Vector2(rb.velocity.x, JumpForce*Time.fixedDeltaTime);
        }
        void Flip()
        {
            isFacingright = !isFacingright;
            transform.Rotate(0, 180, 0);
        }
        public IEnumerator Dash()
        {
            isDashing = true;
            rb.velocity = new Vector2(transform.right.x*DashingPower,rb.velocity.y);
            Stamina_and_Health.Stamina -= 27;
            yield return new WaitForSeconds(DashingTime);
            isDashing = false;
        }

    #endregion

    #region Attacks
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
                    Invoke("ResetStart", 1f);
                }
            }
        }
        public void OnAttack()
        {
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
            anim.SetTrigger("Attack2");
        
        }
        public void OnAttack3()
        {
            ResetStart();
            anim.SetTrigger("Attack3");
        }
        void ResetStart()
        {
            Btn_Pressed_Counter = 0;
            Sec_Combo_BtnPressed = 0;
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
            anim.SetTrigger("Special");
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
    #endregion

    #region Save & Load
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
    #endregion
    private void OnCollisionEnter2D(Collision2D collision)
    {
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
