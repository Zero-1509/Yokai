using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesScript : MonoBehaviour
{
    bool isNotActive = false;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isNotActive)
        {
            StartCoroutine(Reconstruct());
        }
    }
    public void Destruct()
    {
        this.gameObject.GetComponent<Collider2D>().enabled = false;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        
        isNotActive = true;
    }
    public void Construct()
    {
        this.gameObject.GetComponent<Collider2D>().enabled = true;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        anim.Rebind();
        isNotActive = false;
    }

    IEnumerator Reconstruct()
    {
        yield return new WaitForSeconds(2);
        Construct();
    }
}
