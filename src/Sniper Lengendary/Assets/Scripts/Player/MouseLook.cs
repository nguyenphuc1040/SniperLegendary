using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 50f;
    float xRotation = 0f;
    float yRotation = 0f;
    public bool RotateX,RotateY;
    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update() {
        float mouseX = Input.GetAxis("Mouse X")*mouseSensitivity*Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y")*mouseSensitivity*Time.deltaTime;

        xRotation -= mouseX;
        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation,-50f,50f);
        if (RotateX) transform.localRotation = Quaternion.Euler(0f, -xRotation,0f);
        if (RotateY) transform.localRotation = Quaternion.Euler(yRotation,0f,0f);
    }
}
