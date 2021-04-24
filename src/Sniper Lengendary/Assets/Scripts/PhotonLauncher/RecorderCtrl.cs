using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Voice.Unity;
using Photon.Voice.PUN;
using Photon.Realtime;
public class RecorderCtrl : MonoBehaviourPun
{
    public KeyCode speakButton = KeyCode.T;
    public Recorder VoiceRecorder;  
    bool voiceStatus;
    PhotonView PV;
    void Start()
    {
        PV = GetComponent<PhotonView>();
        voiceStatus = true;
        VoiceRecorder.TransmitEnabled = voiceStatus;
    }

    void Update()
    {
        if (PV.IsMine){
            if (Input.GetKeyDown(speakButton)){
                voiceStatus = !voiceStatus;
                VoiceRecorder.TransmitEnabled = voiceStatus;
            }
            GameCtrl.ins._setSpeakerStatus(voiceStatus);
        }
    }
}
