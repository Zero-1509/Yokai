using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restart : MonoBehaviour
{
    Vector2 PlayerStartPos;
    public static bool SceneRestart = false;
    // Start is called before the first frame update
    void Start()
    {
        PlayerStartPos = transform.position;
        SceneRestart = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTouchGround()
    {
        Stamina_and_Health.Stamina = 100;
        transform.position = PlayerStartPos+(Vector2.up*5f);
        SceneRestart = true;
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
            PlayerStartPos = collision.transform.position;
        } 
        if (collision.gameObject.CompareTag("Cam"))
        {
            PlayerStartPos = (Vector2)transform.position-gameObject.GetComponent<BoxCollider2D>().offset;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
       
    }
}
