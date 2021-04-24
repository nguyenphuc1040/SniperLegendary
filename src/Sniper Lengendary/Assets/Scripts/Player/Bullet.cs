using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
public class Bullet : MonoBehaviour
{
    private float speed;
    PhotonView PV;
    Vector3 mPrevPos;
    public GameObject blood,bulletTrade;
    public string nameHost;
    private void Awake() {
        speed = 790;
    }
    void Start()
    {   
        PV = GetComponent<PhotonView>();
        mPrevPos=transform.position;   
        if (PV.IsMine){
            transform.localScale = new Vector3(0.1f,0.1f,0.1f);
            StartCoroutine(_Destroy());
        }
    }

    private void Update() {
    
        mPrevPos = transform.position;
        if (PV.IsMine) transform.localScale += new Vector3(0.1f,0.1f,0.1f);
        transform.Translate(Vector3.forward*speed* Time.deltaTime);
        transform.Translate(Vector3.up*-4.9f/2*Time.deltaTime);
        _rayCastOne();
        
    }
    void _rayCastOne(){
        RaycastHit[] hits = Physics.RaycastAll(new Ray(mPrevPos,(transform.position-mPrevPos).normalized),(transform.position-mPrevPos).magnitude);
        for (int i=0; i<hits.Length; i++){
            if (hits[i].collider.gameObject.tag=="headPlayer"){
                PhotonNetwork.Instantiate(Path.Combine("Player","blood"),hits[i].point,Quaternion.LookRotation(hits[i].normal));
                hits[i].collider.gameObject.GetComponent<isDamage>()._isDamaging(3,nameHost);
            }
            if (hits[i].collider.gameObject.tag=="bodyPlayer"){
                PhotonNetwork.Instantiate(Path.Combine("Player","blood"),hits[i].point,Quaternion.LookRotation(hits[i].normal));
                hits[i].collider.gameObject.GetComponent<isDamage>()._isDamaging(1,nameHost);
            }
            if (hits[i].collider.gameObject.tag=="construction"){
                PhotonNetwork.Instantiate(Path.Combine("Player","bulletTrade"),hits[i].point,Quaternion.LookRotation(hits[i].normal));
            }
            if (PV.IsMine) PhotonNetwork.Destroy(gameObject);
            Debug.Log(hits[i].collider.gameObject.tag);
        }
    }
    IEnumerator _Destroy(){
        yield return new WaitForSeconds(4f);
        PhotonNetwork.Destroy(gameObject);
    }
    // void _rayCastAll(){
    //     RaycastHit[] hits = Physics.RaycastAll(new Ray(mPrevPos,(transform.position-mPrevPos).normalized),(transform.position-mPrevPos).magnitude);
    //     for (int i=0; i<hits.Length;i++){
    //         if (hits[i].collider.gameObject.tag=="headPlayer"){
    //             Instantiate(blood,hits[i].point,Quaternion.identity);
    //             hits[i].collider.gameObject.GetComponent<isDamage>()._isDamaging(3);
    //         }
    //         if (hits[i].collider.gameObject.tag=="bodyPlayer"){
    //             hits[i].collider.gameObject.GetComponent<isDamage>()._isDamaging(1);
    //         }
    //     }
    // }

}
