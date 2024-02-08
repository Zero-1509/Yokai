using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendScript : MonoBehaviour
{
    public float width;
    public float height;
    public float angle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D col = Physics2D.OverlapBox(transform.position, new Vector2(width, height), angle);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position, new Vector2(width, height));
    }
}
