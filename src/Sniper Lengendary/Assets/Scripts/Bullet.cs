﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed;
    Vector3 mPrevPos;
    public GameObject blood;
    private void Awake() {
        speed = 790;
    }
    void Start()
    {
        mPrevPos=transform.position;   
    }

    private void Update() {
        mPrevPos = transform.position;
        transform.Translate(Vector3.forward*speed* Time.deltaTime);
        transform.Translate(Vector3.up*-0.49f*Time.deltaTime);
        RaycastHit[] hits = Physics.RaycastAll(new Ray(mPrevPos,(transform.position-mPrevPos).normalized),(transform.position-mPrevPos).magnitude);
        for (int i=0; i<hits.Length;i++){
            Instantiate(blood,hits[i].point,Quaternion.identity);
           // Debug.Log(hits[i].collider.point.transform.position);
        }
    }

}
