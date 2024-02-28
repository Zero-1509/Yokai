using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restart : MonoBehaviour
{
    Vector2 PlayerStartPos;
    // Start is called before the first frame update
    void Start()
    {
        PlayerStartPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTouchGround()
    {
        transform.position = PlayerStartPos+(Vector2.up*5f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("RES"))
        {
            OnTouchGround();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CP"))
        {
            PlayerStartPos = transform.position;
        }
    }
}
