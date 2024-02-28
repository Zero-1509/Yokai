using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject TargetObj;
    // Start is called before the first frame update
    Vector3 offset;
    void Start()
    {
        offset = transform.position - TargetObj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(BasicScript.InMotion)
            transform.position =new Vector2(transform.position.x+offset.x*Time.deltaTime,transform.position.y);
    }
}
