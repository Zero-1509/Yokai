using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public LayerMask DetectLayer;
    [SerializeField] float radius;

    public float ExpPoints;
    // Start is called before the first frame update
    void Start(){
        ExpPoints = PlayerPrefs.GetFloat("Experience");
    }

    // Update is called once per frame
    void Update(){

        Collider2D col = Physics2D.OverlapCircle(transform.position,radius, DetectLayer);

        if (col){
            Debug.Log("Hit "+ col.name);
            Destroy(col.gameObject);
            float GotXP = Random.Range(30, 70);
            ExpPoints += GotXP;
            PlayerPrefs.SetFloat("Experience", ExpPoints);
        }
    }
    private void OnDrawGizmos(){
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
