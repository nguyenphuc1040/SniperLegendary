using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
public class PlayerMain : MonoBehaviour
{
    PhotonView PV;
    
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine) CreatePlayerMain();
    }
    public void CreatePlayerMain(){
        if (PV.IsMine){
            PhotonNetwork.Instantiate(Path.Combine("Player","PlayerController"),StartPosition.ins._getPosition().position,Quaternion.identity);
        }
    }
    private void Update() {
        if (GameCtrl.ins.isRevival){
            GameCtrl.ins.isRevival = false;

            StartCoroutine(_countTime(2f));
        }
    }

    IEnumerator _countTime(float time){
        yield return new WaitForSeconds(time);
        CreatePlayerMain();
    }
}
