using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Vehicle : MonoBehaviour
{
    public CharacterController characterController;
    public GameObject pointVehical;
    PhotonView PV;
    public GameObject WheelFL, WheelFR, WheelBL, WheelBR;
    public bool isOwned;
    public float speed,z,x;
    void Start()
    {
        PV = GetComponent<PhotonView>();
        StartCoroutine(_SpeedCtrl());
        StartCoroutine(_Phanh());
    }
    void Update()
    {
        _Controller();
    }
    Vector3 move;
    bool isPhanh;
    public void _Controller(){
        if (Input.GetKeyDown(KeyCode.Space)){
                isPhanh = true;
        } else if (Input.GetKeyUp(KeyCode.Space))isPhanh = false;         
        if (characterController.isGrounded){
            float direct= speed;
            move = transform.forward*direct;
               
        }
        move.y = -9.8f*Time.deltaTime;
        characterController.Move(move*Time.deltaTime);
        if (x!=0 && speed!=0) {
            WheelFR.transform.Rotate(Vector3.forward*-100f*x*Time.deltaTime);
            WheelFL.transform.Rotate(Vector3.forward*-100f*x*Time.deltaTime);
           // transform.Rotate(Vector3.up*90f*x*Time.deltaTime);
            var tmp = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y+ x*100f*Time.deltaTime, transform.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, tmp , Time.deltaTime * 10);
        }
    }
    IEnumerator _SpeedCtrl(){
        yield return new WaitForSeconds(0.1f);
        if (!isPhanh){
            if (z>0) {
            if (speed<37) speed +=0.9f;
            } else 
            if (z<0){
                if (speed>-30) speed -=0.7f;
                
            }  else {
                if (speed>0) speed -=1.1f;
                if (speed<0) speed +=2.1f;
                if (speed<2f && speed>-2f) speed = 0;
            }
        }
        Debug.Log(speed);
        StartCoroutine(_SpeedCtrl());
    }

    IEnumerator _Phanh(){
        yield return new WaitForSeconds(0.1f);
        if (isPhanh) {
            speed = speed/1.3f;
        }
        StartCoroutine(_Phanh());
    }

    public void _setZ(float x){ 
        z= x; 
    }
    public void _setX(float x){
        this.x = x;
    }
}
