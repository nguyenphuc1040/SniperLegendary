using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class MouseLook : MonoBehaviour
{
    public GameObject Parent;
    PlayerCtrl parentComponent;
    public static MouseLook ins;
    public float mouseSensitivity;
    public float defaultSensitivity;
    float xRotation = 0f;
    float yRotation = 0f;
    public bool RotateX,RotateY;
    PhotonView PV;
    private void Awake() {
        ins = this;
        PV = GetComponent<PhotonView>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Start() {
        if (PV.IsMine) {
            defaultSensitivity = 170f;
            parentComponent = Parent.GetComponent<PlayerCtrl>(); 
        }
    }
    private void Update() {
        if (PV.IsMine && !parentComponent.isDied && !GameCtrl.ins.statusSettingPanel){
            float mouseX = Input.GetAxis("Mouse X")*mouseSensitivity*Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y")*mouseSensitivity*Time.deltaTime;

            xRotation -= mouseX;
            yRotation -= mouseY;
            yRotation = Mathf.Clamp(yRotation,-50f,50f);
            if (RotateX) transform.localRotation = Quaternion.Euler(0f, -xRotation,0f);
            if (RotateY) transform.localRotation = Quaternion.Euler(yRotation,0f,0f);
        }
        
    }

    public void _setSentivity(int x){
        mouseSensitivity = (x==0)? defaultSensitivity*GameCtrl.ins.mouseSensitivity.value/100: defaultSensitivity*GameCtrl.ins.scopeSensivity.value/100;
    }
}
