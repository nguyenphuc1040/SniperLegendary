using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
public class UIGamePlayCtrl : MonoBehaviourPunCallbacks
{
    public static UIGamePlayCtrl ins;
    public Text ping,roomID;
    void Awake(){
        ins = this;

    }
    void Start()
    {
        roomID.text = "CODE:#" + PhotonNetwork.CurrentRoom.Name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void _SetPing(int x){
        ping.text = x+"ms";
    }
}
