using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSound : MonoBehaviour
{
    public AudioSource  audio_source_player;
    public AudioClip stepFootAudioClip,reloadAudioClip;
    public void _stepPlayAudio(){
        audio_source_player.PlayOneShot(stepFootAudioClip);
    }
    public void _reloadBullet(){ 
        audio_source_player.PlayOneShot(reloadAudioClip);
    }
}
