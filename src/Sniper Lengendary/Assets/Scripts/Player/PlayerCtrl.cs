using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public CharacterController characterController;
    public Animator playerAnimator, handAnimator, cameraAnimator, weaponAnimator;
    private float speed;
    public Transform barrelPosition;
    public GameObject bulletPrefab;
    public AudioSource audio_source;
    public AudioClip shotAudioClip,reloadAudioClip,shotScopeAudioClip,stepFoot;
    bool isHavingBullet, timeBreath, isLoaded;
  
    private void Awake() {
        
    }
    void Start()
    {
        isLoaded = true;
        isHavingBullet=true;
        timeBreath = true;
        speed = 3f;
        characterController = GetComponent<CharacterController>();
        ScopeMode.ins.weaponCamera.SetActive(true);
        
    }

    void Update()
    {
        if (characterController.isGrounded){
            
        }
        _playerMove();
        _keyboardController();
        
    }
    Vector3 move;
    bool isDied;
    void _playerMove(){
        
        if (characterController.isGrounded){
            float x = Input.GetAxis("Horizontal")*speed;
            float z = Input.GetAxis("Vertical")*speed;
            move = transform.right*x + transform.forward*z;
            if(x!=0 || z!=0){
           // playerAnimator.SetTrigger("walk");
                playerAnimator.SetFloat("run",speed*1.5f/3);
            
            } else {
                playerAnimator.SetFloat("run",0f);
                playerAnimator.SetTrigger("idle");
            }
            
            if (Input.GetKeyDown(KeyCode.LeftShift) && timeBreath){
                StartCoroutine(_holderbreath());
                
            }
            if (Input.GetKeyDown(KeyCode.Space)){
                move.y = 4.5f;
            }
            if (isDied) _isDied();
        } else {
            playerAnimator.SetFloat("run",0f);
            playerAnimator.SetTrigger("idle");
        }
        move.y -= 9.8f*Time.deltaTime;      
        characterController.Move(move*Time.deltaTime);
        if (move.y<-20f) isDied=true;
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
            cameraAnimator.SetTrigger("shot");
            audio_source.PlayOneShot(shotAudioClip);
            Instantiate(bulletPrefab,barrelPosition.position,barrelPosition.rotation);           
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
        handAnimator.SetTrigger("reload");
        weaponAnimator.SetTrigger("reload");
    }
    IEnumerator _reloadBullet(){
        isLoaded = false;
        yield return new WaitForSeconds(1.8f);
        isHavingBullet=true;
        isLoaded = true;
    }
    int blood = 3;
    public void _isDamaging(int damage){
        blood-=damage;
        if(blood<=0) _isDied();
    }
    public void _isDied(){
        playerAnimator.SetTrigger("died");
    }
}
