using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Close : MonoBehaviour
{
    Animator anim;
    public GameObject Left;
    public GameObject Right;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.enabled = false;
        Left.GetComponent<Collider2D>().enabled = false;
        Right.GetComponent<Collider2D>().enabled = false;
    }

    public void ColEnable()
    {
        Left.GetComponent<Collider2D>().enabled = true;
        Right.GetComponent<Collider2D>().enabled = true;
    }
    public void ColDisable()
    {
        Left.GetComponent<Collider2D>().enabled = false;
        Right.GetComponent<Collider2D>().enabled = false;
    }
    public void AnimFalse()
    {
        anim.enabled = false;
    }
    public void AnimTrue()
    {
        anim.enabled = true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.enabled = true;
        }
    }
}
