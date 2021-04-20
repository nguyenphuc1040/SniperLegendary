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
    bool isHavingBullet, timeBreath;
  
    private void Awake() {
        
    }
    void Start()
    {
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

    void _playerMove(){
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right*x + transform.forward*z;
        characterController.Move(move*speed*Time.deltaTime);
        
        if(move!=Vector3.zero){
           // playerAnimator.SetTrigger("walk");
            playerAnimator.SetFloat("run",speed*2/3);
            
        } else {
            playerAnimator.SetFloat("run",0f);
            playerAnimator.SetTrigger("idle");
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && timeBreath){
            StartCoroutine(_holderbreath());
            
        }

        
    }
    IEnumerator _holderbreath(){
        timeBreath = false;
        playerAnimator.SetTrigger("holderbreath");
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
        if (ScopeMode.ins.isScope) speed = 3f; 
            else speed = (Input.GetKey(KeyCode.LeftShift)) ? 6f : 3f;
        if (Input.GetMouseButtonDown(0) && isHavingBullet){
            isHavingBullet=false;
            cameraAnimator.SetTrigger("shot");
            audio_source.PlayOneShot(shotAudioClip);
            Instantiate(bulletPrefab,barrelPosition.position,barrelPosition.rotation);           
        }
        if (Input.GetMouseButtonUp(0)){
    
            if (ScopeMode.ins.isScope) ScopeMode.ins._scopeTransition();
            StartCoroutine(_reloadHandAndWeapon());
            StartCoroutine(_reloadBullet());
        }
        
    }
    IEnumerator _reloadHandAndWeapon(){
        yield return new WaitForSeconds(0.4f);
      //  audio_source.PlayOneShot(reloadAudioClip);
        handAnimator.SetTrigger("reload");
        weaponAnimator.SetTrigger("reload");
    }
    IEnumerator _reloadBullet(){
        yield return new WaitForSeconds(1.8f);
        isHavingBullet=true;
    }
}
