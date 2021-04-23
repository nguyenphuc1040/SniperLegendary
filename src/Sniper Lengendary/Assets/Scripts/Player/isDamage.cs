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

    public void _isDamaging(int damage, string nameHost){
        if (PV.IsMine){
            Debug.Log(nameHost);
            parent._isDamaging(damage,nameHost);
        }
    }
}
