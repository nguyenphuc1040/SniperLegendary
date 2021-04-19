using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScopeMode : MonoBehaviour
{
    public static ScopeMode ins;
    public Animator weaponAnimator;
    public Camera scopeCamera;
    public GameObject scopeOverlay, weaponCamera, aimShot;
    public bool isScope;
    float zoomNumber;
    private void Awake() {
        if (ins==null) ins = this;
        zoomNumber = 10f;
    }
    private void Start() {

    }

    private void Update(){
        scopeCamera.fieldOfView = zoomNumber;
        _zoomChange();
        if (Input.GetMouseButtonDown(0)){
            _scopeTransition();
        }
    }

    void _zoomChange(){
        if(Input.GetAxis("Mouse ScrollWheel")>0 && zoomNumber>5){
            zoomNumber --;
        }
        if(Input.GetAxis("Mouse ScrollWheel")<0 && zoomNumber<20){
            zoomNumber++;
        }
    }

    public void _scopeTransition(){
        isScope = !isScope;
        if (isScope){
            weaponAnimator.SetTrigger("scope");
            StartCoroutine(_turnOnScope());
        } else {
            weaponAnimator.SetTrigger("idle");
            StartCoroutine(_turnOffScope());
        }
    }
    IEnumerator _turnOffScope(){
        yield return new WaitForSeconds(0.1f);
        _SetActiveObj(true,false,true);
    }
    IEnumerator _turnOnScope(){
        yield return new WaitForSeconds(0.15f);
        _SetActiveObj(false,true,false);
    }

    void _SetActiveObj(bool aim, bool scopeoverlay, bool weaponcam){
        aimShot.SetActive(aim);
        scopeOverlay.SetActive(scopeoverlay);
        weaponCamera.SetActive(weaponcam);
    }
}
