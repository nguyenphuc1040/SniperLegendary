using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
[RequireComponent(typeof(PhotonView))]
public class AnimationSound : MonoBehaviour
{
    PhotonView PV;
    public AudioSource  audio_source_player;
    public AudioClip stepFootAudioClip,reloadAudioClip,shotClip;
    private void Start() {
        PV = GetComponent<PhotonView>();
    }
    public void _stepPlayAudio(){
        audio_source_player.PlayOneShot(stepFootAudioClip);
    }
    // public void _reloadBullet(){ 
    //     if (PV.IsMine)  PV.RPC("_reloadRPC",RpcTarget.All," ");
    // }

    // [PunRPC]
    public void _reloadRPC(){
        audio_source_player.PlayOneShot(reloadAudioClip);
    }
    public void _shot(){
        audio_source_player.PlayOneShot(shotClip);
    }
    
}
