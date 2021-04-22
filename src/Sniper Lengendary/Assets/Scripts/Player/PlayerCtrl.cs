using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;
using dataObj;
public class PlayerCtrl : MonoBehaviourPunCallbacks
{
    public CharacterController characterController;
    PhotonView PV;
    dataPlayer data = new dataPlayer();
    public Animator playerAnimator, handAnimator, cameraAnimator, weaponAnimator;
    private float speed;
    public Transform barrelPosition;
    public GameObject bulletPrefab;
    public AudioSource audio_source;
    public AudioClip shotAudioClip,reloadAudioClip,shotScopeAudioClip,stepFoot;
    bool isHavingBullet, timeBreath, isLoaded;
    public GameObject pointCameraFollow,CamHere;
    CameraFollow MainCam;
    private void Awake() {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine){
            blood = 3;
            CamHere = GameObject.Find("CamHere");
            MainCam = CamHere.transform.Find("CameraMain").GetComponent<CameraFollow>();
           // MainCam = GameObject.Find("CameraMain").GetComponent<CameraFollow>();    
            MainCam._getFollower(pointCameraFollow);
            characterController = GetComponent<CharacterController>();   
        }
        
    }
    void Start()
    {
        
        isLoaded = true;
        isHavingBullet=true;
        timeBreath = true;
        speed = 3f;
        if (PV.IsMine) StartCoroutine(_GetPing());
       
    }
    IEnumerator _GetPing(){
        UIGamePlayCtrl.ins._SetPing(PhotonNetwork.GetPing());
        yield return new WaitForSeconds(1f);
        StartCoroutine(_GetPing()); 
    }
    void Update()
    {
        if (PV.IsMine &&!isDied && !GameCtrl.ins.statusSettingPanel) {
            _playerMove();
            _keyboardController();
        }
        if (PV.IsMine && GameCtrl.ins.statusSettingPanel) playerAnimator.SetTrigger("idle");
    }
    Vector3 move;
    public bool isDied;
    void _playerMove(){
        
        if (characterController.isGrounded){
            float x = Input.GetAxis("Horizontal")*speed;
            float z = Input.GetAxis("Vertical")*speed;
            move = transform.right*x + transform.forward*z;
            if(x!=0 || z!=0 || (x!=0 && z!=0)){
           // playerAnimator.SetTrigger("walk");
                playerAnimator.SetFloat("run",speed*1.5f/3);
            
            } else {
              //  playerAnimator.SetFloat("run",0f);
                playerAnimator.SetTrigger("idle");
            }
            
            if (Input.GetKeyDown(KeyCode.LeftShift) && timeBreath){
                StartCoroutine(_holderbreath());
                
            }
            if (Input.GetKeyDown(KeyCode.Space)){
                move.y = 4.5f;
            }
        } else {
            playerAnimator.SetFloat("run",0f);
            playerAnimator.SetTrigger("idle");
        }
        move.y -= 9.8f*Time.deltaTime;      
        characterController.Move(move*Time.deltaTime);
      //  if (move.y<-20f) _isDamaging(3);
    }
    IEnumerator _holderbreath(){
        timeBreath = false;
        playerAnimator.SetTrigger("holdbreath");
        cameraAnimator.SetTrigger("holderbreath");
        yield return new WaitForSeconds(4f);
        
        cameraAnimator.SetTrigger("idle");
        StartCoroutine(_countTimeBreath());
    }    
    IEnumerator _countTimeBreath(){
        yield return new WaitForSeconds(5f);
        timeBreath = true;
    }

    void _keyboardController(){
        if (ScopeMode.ins.isScope || !isLoaded) speed = 3f; 
            else speed = (Input.GetKey(KeyCode.LeftShift)) ? 6f : 3f;
        if (Input.GetMouseButtonDown(0) && isHavingBullet){
            isHavingBullet=false;
            PV.RPC("_cameraAnimatorRPC",RpcTarget.All,"shot");
            //cameraAnimator.SetTrigger("shot");
            audio_source.PlayOneShot(shotAudioClip);
            PV.RPC("_audioPlay",RpcTarget.All,0);
            PhotonNetwork.Instantiate(Path.Combine("Player","Bullet"),barrelPosition.position,barrelPosition.rotation);           
        }
        if (Input.GetMouseButtonUp(0) && isLoaded){
    
            if (ScopeMode.ins.isScope) ScopeMode.ins._scopeTransition();
            StartCoroutine(_reloadHandAndWeapon());
            StartCoroutine(_reloadBullet());
        }
        if (Input.GetMouseButtonDown(1) && isLoaded){
            ScopeMode.ins._scopeTransition();
        }

    }
    IEnumerator _reloadHandAndWeapon(){
        yield return new WaitForSeconds(0.4f);
      //  audio_source.PlayOneShot(reloadAudioClip);
        PV.RPC("_handAnimatorRPC",RpcTarget.All,"reload");
       // handAnimator.SetTrigger("reload");
        PV.RPC("_weaponAnimatorRPC",RpcTarget.All,"reload");
       // weaponAnimator.SetTrigger("reload");
    }
    [PunRPC]
    void _handAnimatorRPC(string trigger){
        handAnimator.SetTrigger(trigger);
    }
    [PunRPC]
    void _weaponAnimatorRPC(string trigger){
        weaponAnimator.SetTrigger(trigger);
    }
    [PunRPC]
    void _cameraAnimatorRPC(string trigger){
        cameraAnimator.SetTrigger(trigger);
    }
    IEnumerator _reloadBullet(){
        isLoaded = false;
        yield return new WaitForSeconds(1.8f);
        isHavingBullet=true;
        isLoaded = true;
    }
    [PunRPC]
    void _audioPlay(int x){
        audio_source.PlayOneShot(shotAudioClip);
    }
    int blood;
    public void _isDamaging(int damage){
        if (PV.IsMine) {
            blood-=damage;
            if(blood<=0) _isDied();
        }
       
    }
    public void _isDied(){
        if (PV.IsMine){
            playerAnimator.SetTrigger("died");
            ScopeMode.ins.Weapon.SetActive(false);
            isDied = true;
            GameCtrl.ins._revival();
            UICtrl.ins._ScopeOnOff(false);
            StartCoroutine(_countTime(1.8f));
        }      
    }
    IEnumerator _countTime(float time){
        MainCam._getFollower(CamHere);
        yield return new WaitForSeconds(time);
        _destroyGameObject();
    }
    void _destroyGameObject(){
        PhotonNetwork.Destroy(gameObject);
    }
}
