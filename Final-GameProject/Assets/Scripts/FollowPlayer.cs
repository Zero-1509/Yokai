using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    float StartY;
    // Start is called before the first frame update
    void Start()
    {
        StartY = gameObject.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x, StartY);
    }
}
