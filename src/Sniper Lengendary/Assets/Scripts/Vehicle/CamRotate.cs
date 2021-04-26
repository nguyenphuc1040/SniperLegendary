using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    float mouseSensitivity = 50f;
    float xRotation = 0f;
    float yRotation = 0f;
    bool isRotate;
    public GameObject Move;
    void Update()
    {
        if (isRotate){
            float mouseX = Input.GetAxis("Mouse X")*mouseSensitivity*Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y")*mouseSensitivity*Time.deltaTime;
            xRotation -= mouseX;
            yRotation -= mouseY;
            yRotation = Mathf.Clamp(yRotation,-50f,50f);
            transform.localRotation = Quaternion.Euler(yRotation, -xRotation,0f);
        }
        
    }
    GameObject follower;
    public void _getFollower(GameObject x){
       follower = x;
       transform.rotation = follower.transform.rotation;
       transform.position = follower.transform.position;
       transform.SetParent(x.transform);
       _setRotate(true);
       
    }
    public void _setRotate(bool x){
        isRotate = x;
    }
}
