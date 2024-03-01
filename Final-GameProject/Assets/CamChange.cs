using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamChange : MonoBehaviour
{
    public CinemachineConfiner2D Confiner;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EditorOnly"))
        {
            Confiner.m_BoundingShape2D = collision;
        }
    }
}
