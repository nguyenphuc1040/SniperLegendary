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
    GameObject CamRotate;
    public GameObject bodyPlayer;
    private void Awake() {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine){
            weaponComponent = weapons.GetComponent<ScopeMode>();
            CamHere = GameObject.Find("CamHere");
            CamRotate = GameObject.Find("CamRotate");
            MainCam = CamHere.transform.Find("CameraMain").GetComponent<CameraFollow>();
            MainCam._getFollower(pointCameraFollow);
            MainCam._SetIsSmooth(false);
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
        if (PV.IsMine) {
            StartCoroutine(_GetPing());
            UICtrl.ins._setBlood(data.getBlood());
        }
       
    }
    IEnumerator _GetPing(){
        UIGamePlayCtrl.ins._SetPing(PhotonNetwork.GetPing());
        yield return new WaitForSeconds(1f);
        StartCoroutine(_GetPing()); 
    }
    void Update()
    {
        if (PV.IsMine){
          
            if (GameCtrl.ins.statusSettingPanel) playerAnimator.SetBool("idlebool",true);
            if (isBoarding){
                 _ControllerVehical();

            }
                else {
                    if (!isDied && !GameCtrl.ins.statusSettingPanel) {
                        _playerMove();
                        _keyboardController();
                    }
                }
        }
    }
    Vehicle CarComponent;
    public bool isBoarding;
    GameObject CarObj;
    public GameObject PointCamVehical;
    void FixedUpdate(){
        if (PV.IsMine){
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray,out hit, 3f))
            {
                if (Input.GetKeyDown(KeyCode.F))
                    if (hit.collider.gameObject.tag == "Vehical")
                    {
                        Debug.Log("Len xe nao");
                        CarObj = hit.collider.gameObject;
                        CarComponent = CarObj.GetComponent<Vehicle>();
                        isBoarding = true;
                        MainCam._getFollower(CamRotate.GetComponent<CamRotate>().Move);
                        CamRotate.GetComponent<CamRotate>()._getFollower(CarComponent.pointVehical);
                        CamRotate.GetComponent<CamRotate>()._setRotate(true);
                       // MainCam._smoothCamera(CarComponent.pointVehical);
                        transform.position = CarObj.transform.position;
                        transform.SetParent(CarObj.transform);
                        PV.RPC("_setActiveBody",RpcTarget.All,false);
                    
                    }    
                else  
                if (Input.GetKeyDown(KeyCode.F) && isBoarding){
                    isBoarding = false;
                    MainCam._getFollower(pointCameraFollow);
                  //  MainCam._SetIsSmooth(false);
                }
            }
        }
        
    }
    [PunRPC]
    void _setActiveBody(bool x){
        bodyPlayer.SetActive(x);
    }
    float moveVehical;
    void _ControllerVehical(){
        moveVehical = Input.GetAxis("Vertical");
        CarComponent._setZ(moveVehical);
        CarComponent._setX(Input.GetAxis("Horizontal"));
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
        if (move.y<-20f && characterController.isGrounded) _isDamaging(1,"nhảy lầu");
    }
    void _updateRun(bool x){
        playerAnimator.SetBool("idlebool",x);
        if (!x) playerAnimator.SetFloat("run",speed*1.5f/3); else playerAnimator.SetFloat("run",0f);
    }
    

    public GameObject Bullet;
    void _keyboardController(){
        if (weaponComponent.isScope || !isLoaded) speed = 3f; 
            else speed = (Input.GetKey(KeyCode.LeftShift)) ? 20f : 3f;
        if (Input.GetMouseButtonDown(0) && isHavingBullet){
            isLoaded = false;
            isHavingBullet=false;
            cameraAnimator.SetFloat("idle",1f);
            cameraAnimator.SetBool("shotbool",true);
            StartCoroutine(_shotDone());
            PV.RPC("_setInfoBullet",RpcTarget.All,data.getNamePlayer());
            PhotonNetwork.Instantiate(Path.Combine("Player","Bullet"),barrelPosition.position,barrelPosition.rotation);           
        }
        if (Input.GetMouseButtonUp(0) && !isLoaded){
    
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

        yield return new WaitForSeconds(1.8f);
        isHavingBullet=true;
        isLoaded = true;
    }

    public void _isDamaging(int damage, string nameShotYou){
            data.setBlood(data.getBlood()-damage);
            UICtrl.ins._setBlood(data.getBlood());
            if(data.getBlood()<=0) _isDied(nameShotYou);
    }
    [PunRPC]
    void _showInfoKiller(string st){
        UICtrl.ins._showInfoKiller(st);
    }
    public void _isDied(string nameShotYou){
            playerAnimator.SetBool("diedbool",true);
            weaponComponent.Weapon.SetActive(false);
            isDied = true;
     
            UICtrl.ins._ScopeOnOff(false);
            string tmp = nameShotYou + " đã tiêu diệt " + data.getNamePlayer();
            PV.RPC("_showInfoKiller",RpcTarget.All,tmp);
            StartCoroutine(_countTime(1.8f));     
    }
    IEnumerator _countTime(float time){
        MainCam._getFollower(CamHere);
        GameCtrl.ins._revival();
        yield return new WaitForSeconds(time);
        if (PV.IsMine) PhotonNetwork.Destroy(gameObject);

    }
}
