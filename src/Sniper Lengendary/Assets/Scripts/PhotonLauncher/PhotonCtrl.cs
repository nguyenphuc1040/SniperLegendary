using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using System.Linq;
using UnityEngine.SceneManagement;
public class PhotonCtrl : MonoBehaviourPunCallbacks
{
    public static PhotonCtrl ins;
    public Text Ping,RoomID;
    public bool NewPlayerJoin;
    private void Awake() {
        ins = this;
        RoomID.text = "CODE: #"+PhotonNetwork.CurrentRoom.Name;
    }
    public void _PressMenu(){
        StartCoroutine(_backingmenu());
    }
    
    IEnumerator _backingmenu(){
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
        {
            yield return null;
        }
        SceneManager.LoadSceneAsync("Lobby");
    }

    public void _PressPlayAgain(){
    }
    public void _SetPing(float x){
        Ping.text = (int)x+"ms";
    }
    public void _NewJoin(bool x){
        NewPlayerJoin = x;
    }
    bool isJoined;
    public int c1,c2;
    private void Update() {
        if (c1!=c2) _NewJoin(true);
    }
    private void FixedUpdate() {
        c1 =  PhotonNetwork.PlayerList.Count();
    }
    private void LateUpdate() {
        c2 = PhotonNetwork.PlayerList.Count();
    }
    public bool MineDead;
    public void _MineIsDead(bool x){
        MineDead = x;
    }
}
