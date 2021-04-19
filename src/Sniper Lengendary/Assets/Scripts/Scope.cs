using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : MonoBehaviour
{
    public static Scope ins;
    public Animator anim;
    public bool isScope;
    public Camera mainCam;
    public GameObject ScopeOverLay,Weapon,Point;
    float Zoom;
    void Start()
    {
        ins = this;
        Zoom = 60f;
    }
	
    // Update is called once per frame
    void Update()
    {
       
	    mainCam.fieldOfView = Zoom;
    }
    public void _ScopeTransition(){
        isScope = !isScope;
        if (isScope){
            anim.SetTrigger("Scope"); 
            StartCoroutine(_Scope());
            
            
        } else {
            anim.SetTrigger("Idle");
            ScopeOverLay.SetActive(false);
            Weapon.SetActive(true);

            Point.SetActive(true);
            Zoom = 60f;
        }
    }
    IEnumerator _Scope(){
        Point.SetActive(false);
        yield return new WaitForSeconds(0.14f);
        ScopeOverLay.SetActive(true);	
        Weapon.SetActive(false);
  
        Zoom = 60f;
    }
}
