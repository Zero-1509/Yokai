using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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


    public bool canDash;
    public bool isDashing;
    private float DashingTime = 0.3f;
    private float DashCD = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        canDash = true;
        isDashing = false;
        anim = GetComponent<Animator>();
        rb= GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x + moveVector.x*speed * Time.deltaTime, transform.position.y + moveVector.y*speed*Time.deltaTime,0);
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

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!isholding && context.performed)
        {
            anim.SetTrigger("Attack");
            Attack();
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
            StartCoroutine(Dash(7));
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
    void Attack()
    {
        Debug.Log("Attack");
    }
    void SpecialAttack()
    {
        StartCoroutine(Dash(12));
    }
    void Jump()
    {
        Debug.Log("Jumped");
        canjump = false;
        rb.gravityScale = 1f;
        rb.AddForce(Vector2.up * JumpForce);
    }


    public IEnumerator Dash(int DPower)
    {
        isDashing = true;
        if (isDashing && canDash)
        {
            rb.velocity = moveVector * DPower;
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            canjump = true;
            rb.gravityScale = 3f;
        }
    }
}
