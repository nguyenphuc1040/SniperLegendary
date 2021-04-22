using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
[RequireComponent(typeof(PhotonView))]
public class isDamage : MonoBehaviour
{
    public GameObject PlayerObj;
    PhotonView PV;
    PlayerCtrl parent;
    void Start()
    {
        PV = GetComponent<PhotonView>();
        parent = PlayerObj.GetComponent<PlayerCtrl>();
    }

    public void _isDamaging(int damage){
        if (PV.IsMine){
            Debug.Log("as");
            parent._isDamaging(damage);
        }
    }
}
