using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class objDestroy : MonoBehaviour
{
    PhotonView PV;
    public float time;
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine) StartCoroutine(_destroy(time));
    }
    IEnumerator _destroy(float time){
        yield return new WaitForSeconds(time);
        PhotonNetwork.Destroy(gameObject);
    }
}
