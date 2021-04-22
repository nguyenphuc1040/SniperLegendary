using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class ScopeMode : MonoBehaviour
{
    PhotonView PV;
    public static ScopeMode ins;
    public Animator playerAnimator, handAnimator;
    public Animator weaponAnimator;
    public bool isScope;
    public GameObject mouseLookx, mouseLooky;
    public GameObject camScope, Weapon;
    float zoomNumber;
    private void Awake() {
        if (ins==null) ins = this;
        PV = GetComponent<PhotonView>();
        if (PV.IsMine){
            zoomNumber = 10f;
            camScope = GameObject.Find("CameraScope");
        }
       
    }
    private void Start() {
    }

    private void Update(){
        if (PV.IsMine){
            camScope.GetComponent<Camera>().fieldOfView = zoomNumber;
            _zoomChange();
        }
    

    }

    void _zoomChange(){
        if(Input.GetAxis("Mouse ScrollWheel")>0 && zoomNumber>3.75f && isScope){
            zoomNumber --;
        }
        if(Input.GetAxis("Mouse ScrollWheel")<0 && zoomNumber<20 && isScope){
            zoomNumber++;
        }
    }

    public void _scopeTransition(){
        isScope = !isScope;
        if (isScope){
            weaponAnimator.SetTrigger("scope");
            handAnimator.SetTrigger("scope");
            
            StartCoroutine(_turnOnScope());
            
        } else {
            weaponAnimator.SetTrigger("idle");
            handAnimator.SetTrigger("idle");
            Weapon.SetActive(true);
            StartCoroutine(_turnOffScope());
        }
    }
    IEnumerator _turnOffScope(){
        yield return new WaitForSeconds(0.1f);
        _SetActiveObj(true);
        _mouseSentivityChange(0);
    }
    IEnumerator _turnOnScope(){
        yield return new WaitForSeconds(0.15f);
        Weapon.SetActive(false);
        _SetActiveObj(false);
        _mouseSentivityChange(1);
    }

    void _SetActiveObj(bool weaponcam){
        if (UICtrl.ins!=null){
            UICtrl.ins._ScopeOnOff(!weaponcam);
        }
    }

    void _mouseSentivityChange(int x){
        mouseLookx.GetComponent<MouseLook>()._setSentivity(x);
        mouseLooky.GetComponent<MouseLook>()._setSentivity(x);
    }
}
