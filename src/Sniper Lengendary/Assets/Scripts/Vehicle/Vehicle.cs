using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
public class Vehicle : MonoBehaviour
{
    public GameObject pointVehical;
    public Transform jumpOutPosition;
    PhotonView PV;
    Rigidbody mybody;
    public bool isOwned;
    float currentSteerAngle, currentbreakForce;
    private float horizontalInput,verticalInput;
    private bool isBreaking;
    [SerializeField]
    private float speedForce, breakForce, maxSteerAngle;

    public WheelCollider frontWheelLeft,frontWheelRight, backWheelLeft,backWheelRight;
    public Transform WheelFL, WheelFR, WheelBL, WheelBR;
    public AudioSource audio_source, EngineAudioSource;
    public AudioClip StartEngineCar;
    public bool  IsPowerON;
    void Start()
    {
        PV = GetComponent<PhotonView>();
        mybody = GetComponent<Rigidbody>();
        EngineAudioSource.mute = true;
        StartCoroutine(_SoundEngine());
        StartCoroutine(_RoadSound());
    }
    void FixedUpdate()
    {
        if (IsPowerON){
            _Controller();
            _Steering();
            _UpdateWheels();
           
            if (isRoadSound){
                EngineAudioSource.pitch = soundPitchRoad;
            } else {
                EngineAudioSource.pitch = soundPitch;
            }   

        }
      
    }
    float isNotInputV;
    public void _PowerON(bool x){ 
        if (!IsPowerON && x){
     
            audio_source.PlayOneShot(StartEngineCar);
            StartCoroutine(_PowerStartSound());
        }
        if (IsPowerON && !x){
            EngineAudioSource.mute = true;
        }
        IsPowerON = x;
    }
  
    public void _Controller(){
        isNotInputV = (verticalInput==0) ? 100f : 0f;
        backWheelLeft.motorTorque = verticalInput*speedForce;
        backWheelRight.motorTorque = verticalInput*speedForce;
        currentbreakForce = isBreaking ? breakForce : isNotInputV;
        _ApplyBreaking();
        
    }
    void _ApplyBreaking(){
        frontWheelLeft.brakeTorque = currentbreakForce;
        frontWheelRight.brakeTorque = currentbreakForce;
        backWheelLeft.brakeTorque = currentbreakForce;
        backWheelRight.brakeTorque = currentbreakForce;
    }
    IEnumerator _PowerStartSound(){
        yield return new WaitForSeconds(1.4f);
        EngineAudioSource.mute = false;
        soundPitch = 0.2f;
        
    }
    public void _SetInputDirection(float x, float z) {
        this.horizontalInput =x;
        this.verticalInput =z;
    }
    public void _SetInputBreaking(bool x){
        this.isBreaking = x;
    }

    void _Steering(){
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontWheelLeft.steerAngle = currentSteerAngle;
        frontWheelRight.steerAngle = currentSteerAngle;
    }

    void _UpdateWheels(){
        _UpdateSingleWheel(frontWheelLeft,WheelFL);
        _UpdateSingleWheel(frontWheelRight,WheelFR);
        _UpdateSingleWheel(backWheelLeft,WheelBL);
        _UpdateSingleWheel(backWheelRight,WheelBR);
    }

    void _UpdateSingleWheel(WheelCollider x, Transform y){
        Vector3 pos;
        Quaternion rot;
        x.GetWorldPose(out pos, out rot);
        y.rotation = rot;
        y.position = pos;
    }
    float soundPitch, soundPitchRoad;
    public bool isRoadSound;
    IEnumerator _SoundEngine(){
        yield return new WaitForSeconds(0.1f);
        if (!isRoadSound){
            soundPitchRoad = soundPitch;
            if (verticalInput!=0){
    
                if (soundPitch<1.5f) soundPitch+=0.005f;
                    
                } else {
                    if (soundPitch>0.23f) soundPitch-=0.03f;
            }
        }
      
        StartCoroutine(_SoundEngine());
    }
    IEnumerator _RoadSound(){
        yield return new WaitForSeconds(0.1f);
        if (isRoadSound){
            if (soundPitchRoad<1.7f) soundPitchRoad+=0.2f;
            soundPitch = soundPitchRoad;
        }
        StartCoroutine(_RoadSound());
    }

    [PunRPC] // isOwned
    void _setOwned(bool x){
        isOwned = x;
    }
    [PunRPC] 
    void _setPitchAudio(float pitch){
        EngineAudioSource.pitch = pitch;
    }

    [PunRPC]
    void _PowerOn(bool x){
        IsPowerON = x;
    }
}
