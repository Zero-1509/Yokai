using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public LayerMask DetectLayer;
    [SerializeField] float radius;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position,radius, DetectLayer);
        //Debug.Log(col);
        if (col)
        {
            Debug.Log("Hit "+ col.name);
            Destroy(col.gameObject);
        }
        else
        {

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
