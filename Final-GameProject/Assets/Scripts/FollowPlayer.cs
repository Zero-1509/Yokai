using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    float StartY;
    public GameObject Cam;
    // Start is called before the first frame update
    void Start()
    {
        StartY = gameObject.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Cam.transform.position.x, StartY,-10);
    }
}
