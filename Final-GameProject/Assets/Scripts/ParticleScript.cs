using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ParticleScript : MonoBehaviour
{
    ParticleSystem Particles;
    public LayerMask DetectLayer;
    // Start is called before the first frame update
    void Start()
    {
        Particles = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnParticleCollision(GameObject col)
    {
        //Debug.Log("Hit Something!!");

    }
}
