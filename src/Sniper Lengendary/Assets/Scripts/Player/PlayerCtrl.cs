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
    public AudioClip shotAudioClip;
    bool isHavingBullet,  isLoaded;
    public GameObject pointCameraFollow,CamHere, weapons;
    ScopeMode weaponComponent;
    CameraFollow MainCam;
    private void Awake() {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine){
            weaponComponent = weapons.GetComponent<ScopeMode>();
            CamHere = GameObject.Find("CamHere");
            MainCam = CamHere.transform.Find("CameraMain").GetComponent<CameraFollow>();
            MainCam._getFollower(pointCameraFollow);
            characterController = GetComponent<CharacterController>();   
            _initData();
        }
        
    }
    void _initData(){
        if (ManagerCtrl.ins!=null) data.setNamePlayer(ManagerCtrl.ins._GetUserName());
        data.setBlood(3);
        data.setKilled(0);
        data.setDied(0);
    }
    void Start()
    {
        
        isLoaded = true;
        isHavingBullet=true;
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
        if (PV.IsMine && GameCtrl.ins.statusSettingPanel) playerAnimator.SetBool("idlebool",true);
    }
    Vector3 move;
    public bool isDied;
    void _playerMove(){
        
        if (characterController.isGrounded){
            float x = Input.GetAxis("Horizontal")*speed;
            float z = Input.GetAxis("Vertical")*speed;
            move = transform.right*x + transform.forward*z;
            _updateRun((x==0f && z==0f));
            if (Input.GetKeyDown(KeyCode.Space)){
                move.y = 4.5f;
            }
        } else _updateRun(true);
        move.y -= 9.8f*Time.deltaTime;      
        characterController.Move(move*Time.deltaTime);
      //  if (move.y<-20f) _isDamaging(3);
    }
    void _updateRun(bool x){
        playerAnimator.SetBool("idlebool",x);
        if (!x) playerAnimator.SetFloat("run",speed*1.5f/3); else playerAnimator.SetFloat("run",0f);
    }
    

    public GameObject Bullet;
    public Animation breathAnimation;
    void _keyboardController(){
        if (weaponComponent.isScope || !isLoaded) speed = 3f; 
            else speed = (Input.GetKey(KeyCode.LeftShift)) ? 6f : 3f;
        if (Input.GetMouseButtonDown(0) && isHavingBullet){
            isHavingBullet=false;
            Debug.Log("stho");
            cameraAnimator.SetFloat("idle",1f);
            cameraAnimator.SetBool("shotbool",true);
            StartCoroutine(_shotDone());
            PV.RPC("_setInfoBullet",RpcTarget.All,data.getNamePlayer());
            PhotonNetwork.Instantiate(Path.Combine("Player","Bullet"),barrelPosition.position,barrelPosition.rotation);           
        }
        if (Input.GetMouseButtonUp(0) && isLoaded){
    
            if (weaponComponent.isScope) weaponComponent._scopeTransition();
            StartCoroutine(_reloadHandAndWeapon());
            StartCoroutine(_reloadBullet());
        }
        if (Input.GetMouseButtonDown(1) && isLoaded){
            weaponComponent._scopeTransition();
        }
        if (Input.GetKey(KeyCode.LeftShift) && isHavingBullet){
            cameraAnimator.SetFloat("idle",0f);
        } else {
            cameraAnimator.SetFloat("idle",1f);
        }
    }
    IEnumerator _shotDone(){
        yield return new WaitForSeconds(0.1f);
        cameraAnimator.SetBool("shotbool",false);
    }
    [PunRPC]
    void _setInfoBullet(string name){
        Bullet.GetComponent<Bullet>().nameHost = name;
    }
    IEnumerator _reloadHandAndWeapon(){
        yield return new WaitForSeconds(0.4f);
        weaponComponent._weaponSetBool(false,false,true);
        weaponComponent._handSetBool(false,false,true);
        yield return new WaitForSeconds(0.1f);
        weaponComponent._weaponSetBool(false,true,false);
        weaponComponent._handSetBool(false,true,false);
    }

    IEnumerator _reloadBullet(){
        isLoaded = false;
        yield return new WaitForSeconds(1.8f);
        isHavingBullet=true;
        isLoaded = true;
    }

    public void _isDamaging(int damage, string nameShotYou){
        if (PV.IsMine) {
            Debug.Log(data.getBlood());
            data.setBlood(data.getBlood()-damage);
            Debug.Log(data.getBlood());
            if(data.getBlood()<=0) _isDied(nameShotYou);
        }
       
    }
    [PunRPC]
    void _showInfoKiller(string st){
        UICtrl.ins._showInfoKiller(st);
    }
    public void _isDied(string nameShotYou){
        if (PV.IsMine){
            playerAnimator.SetBool("diedbool",true);
            weaponComponent.Weapon.SetActive(false);
            isDied = true;
            GameCtrl.ins._revival();
            UICtrl.ins._ScopeOnOff(false);
            string tmp = nameShotYou + " đã tiêu diệt " + data.getNamePlayer();
            PV.RPC("_showInfoKiller",RpcTarget.All,tmp);
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
