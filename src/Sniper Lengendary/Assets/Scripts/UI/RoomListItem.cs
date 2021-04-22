using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class RoomListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] Text text;
    public RoomInfo info;
    public void SetUp(RoomInfo _info){
        info = _info;
        text.text = _info.Name;
    }
    public void OnClick(){
        Launcher.ins._JoinRoom(info);
    }
}
