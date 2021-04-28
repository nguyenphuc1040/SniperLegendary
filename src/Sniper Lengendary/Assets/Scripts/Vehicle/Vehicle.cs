using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
public class Vehicle : MonoBehaviour
{
    public GameObject pointVehical;
    public Transform jumpOutPosition;
    public PhotonView PV;
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
    public Vector3 centerOfMassVector3;
    public bool  IsPowerON;
    void Start()
    {
        PV = GetComponent<PhotonView>();
        mybody = GetComponent<Rigidbody>();
        StartCoroutine(_SoundEngine());
        StartCoroutine(_RoadSound());
        mybody.centerOfMass = centerOfMassVector3;
    }
    void FixedUpdate()
    {
        if (IsPowerON && PV.IsMine){
            _Controller();
            _Steering();
            _UpdateWheels();
           
            if (isRoadSound){
                PV.RPC("_setPitchAudio",RpcTarget.All,soundPitchRoad);
               // EngineAudioSource.pitch = soundPitchRoad;
            } else {
               // EngineAudioSource.pitch = soundPitch;
                PV.RPC("_setPitchAudio",RpcTarget.All,soundPitch);
            }   

        }
      
    }
    float isNotInputV;
    public void _PowerON(bool x){ 
        if (!IsPowerON && x){
     
            PV.RPC("_startEngine",RpcTarget.All);
            StartCoroutine(_PowerStartSound());
        }
        if (IsPowerON && !x){
            PV.RPC("_EngineMute",RpcTarget.All,true);
        }
        IsPowerON = x;
    }
    [PunRPC]
    void _startEngine(){
        audio_source.PlayOneShot(StartEngineCar);
    }
    [PunRPC]
    void _EngineMute(bool x){
        EngineAudioSource.mute = x;
    }
    public void _Controller(){
        backWheelLeft.motorTorque = verticalInput* speedForce*Time.deltaTime;
        backWheelRight.motorTorque = verticalInput* speedForce*Time.deltaTime;
        currentbreakForce = isBreaking ? breakForce : 0;
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
        PV.RPC("_EngineMute",RpcTarget.All,false);
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
        frontWheelLeft.steerAngle =  Mathf.Lerp(frontWheelLeft.steerAngle,currentSteerAngle,0.1f);
        frontWheelRight.steerAngle =  Mathf.Lerp(frontWheelRight.steerAngle,currentSteerAngle,0.1f);
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
