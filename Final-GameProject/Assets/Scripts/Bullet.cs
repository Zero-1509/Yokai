using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    float bulletDamage;
    // Start is called before the first frame update
    void Start()
    {
        bulletDamage = 12;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity= -transform.right*250f*Time.deltaTime;

        Destroy(gameObject, 5f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit "+ collision.collider.gameObject);
            collision.gameObject.GetComponent<Stamina_and_Health>().Health -= bulletDamage;
            Destroy(gameObject);
            
        }
    }
}
