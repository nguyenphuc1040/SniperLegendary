using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Photon.Realtime;
public class PlayerMain : MonoBehaviourPunCallbacks
{
    PhotonView PV;
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine) {
            CreatePlayerMain();
            if (PhotonNetwork.IsMasterClient){
                
                PhotonNetwork.Instantiate(Path.Combine("Player","Aventador"),StartPosition.ins._getVehicalPos().position,Quaternion.identity);
            } else {
                if (PhotonNetwork.CurrentRoom.PlayerCount-1<5)
                    PhotonNetwork.Instantiate(Path.Combine("Player","Aventador"),StartPosition.ins.vehicalPosClient[PhotonNetwork.CurrentRoom.PlayerCount-1].position,Quaternion.identity);       
            }
        }
    }
    public void CreatePlayerMain(){
        if (PV.IsMine){
            PhotonNetwork.Instantiate(Path.Combine("Player","PlayerController"),StartPosition.ins._getPosition().position,Quaternion.identity);
        }
    }
    private void Update() {
        if (GameCtrl.ins.isRevival && PV.IsMine){
            GameCtrl.ins.isRevival = false;

            StartCoroutine(_countTime(2f));
        }
    }

    IEnumerator _countTime(float time){
        yield return new WaitForSeconds(time);
        CreatePlayerMain();
    }
}
