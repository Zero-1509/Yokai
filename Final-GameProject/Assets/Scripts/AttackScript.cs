using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public LayerMask DetectLayer;
    [SerializeField] Vector2 radius;

    public float ExpPoints;
    // Start is called before the first frame update
    void Start(){
        //ExpPoints = PlayerPrefs.GetFloat("Experience");
    }

    // Update is called once per frame
    void Update(){

        Collider2D col = Physics2D.OverlapCapsule(transform.position, radius, CapsuleDirection2D.Vertical, 22.78f,DetectLayer);
        if (col){
           if(col.tag == "HebikawaL"||col.tag == "HebikawaR"||col.tag == "HebikawaH")
            {
                col.GetComponent<Enemy_Stats>().HUpdates(10);
                this.gameObject.SetActive(false);
            }
            else
            {
                col.GetComponent<TutorialKill>().health -= 1;
                this.gameObject.SetActive(false);
            }
            /*float GotXP = Random.Range(30, 70);
            ExpPoints += GotXP;
            PlayerPrefs.SetFloat("Experience", ExpPoints);*/
        }
    }
    private void OnDrawGizmos(){
        Gizmos.color = Color.blue;
    }
}
