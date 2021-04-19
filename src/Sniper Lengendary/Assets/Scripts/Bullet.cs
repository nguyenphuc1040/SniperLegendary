using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody mybody;
    public float F;
    Vector3 mPrevPos;
    public GameObject blood;
    void Start()
    {
        //mybody.AddForce(transform.forward*F);
        mPrevPos=transform.position;
        
    }

    private void Update() {
        mPrevPos = transform.position;
        transform.Translate(Vector3.forward*F);
        transform.Translate(Vector3.up*-(1/2*98*1)*Time.deltaTime);
        RaycastHit[] hits = Physics.RaycastAll(new Ray(mPrevPos,(transform.position-mPrevPos).normalized),(transform.position-mPrevPos).magnitude);
        for (int i=0; i<hits.Length;i++){
            Instantiate(blood,hits[i].point,Quaternion.identity);
           // Debug.Log(hits[i].collider.point.transform.position);
        }
    }

}
