using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public static Player ins;
    public CharacterController controller;
    public float speed;
    
    public float gravity;
    
    public Transform groundCheck;
    public float groundDistance = 50f;
    public LayerMask groundMask;
    public float jumpHeight;
	public Animator animatorCamera;
	public Animator animatorWeapon;
    Vector3 velocity;
	public GameObject SniperGun,Bullet;	
	public AudioSource audio_source;
	public AudioClip shotm24noscope,shotm24withscope, reloadm24;
    bool isGrounded;
	bool timeShot;
	private void Start() {
		timeShot=true;
	}
    void Update()
    {
		isGrounded = Physics.CheckSphere(groundCheck.position,groundDistance, groundMask);
		
		if (isGrounded && velocity.y <0){
			velocity.y = 0f;
		}
		float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");
		Vector3 move = transform.right* x + transform.forward * z;
		controller.Move(move * speed * Time.deltaTime);


		if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
		{
			velocity.y = Mathf.Sqrt(jumpHeight * -1f * gravity);
		}
		if (Input.GetKeyDown(KeyCode.LeftShift)){
			speed *=2;
		}
		if (Input.GetKeyUp(KeyCode.LeftShift)){
			speed /=2;
		}
 		if (Input.GetMouseButtonDown(1) && timeShot){
	        
	        Scope.ins._ScopeTransition();
	    }
		if (Input.GetMouseButtonDown(0) && timeShot){
			timeShot=false;
			Instantiate(Bullet,SniperGun.transform.position,SniperGun.transform.rotation);
			// Rigidbody insBulletRigidbody = insBullet.GetComponent<Rigidbody>();
			// insBulletRigidbody.AddForce(Vector3.forward*1000000);
			if (Scope.ins.isScope) audio_source.PlayOneShot(shotm24withscope);
			animatorCamera.SetTrigger("shot");
			if(!Scope.ins.isScope){
				audio_source.PlayOneShot(shotm24noscope);
				animatorWeapon.SetTrigger("shot");
				StartCoroutine(_timeShot());
			} else {
				//audio_source.PlayOneShot(reloadm24);
				StartCoroutine(_timeShot());
				animatorWeapon.SetTrigger("reloadshot");
			}
		
		}
		if (Input.GetMouseButtonUp(0)){
			if(Scope.ins.isScope){
				StartCoroutine(_ScopeDown());
				
			}
			
		}
	
		controller.Move(velocity * Time.deltaTime);
    }

	IEnumerator _timeShot(){
		yield return new WaitForSeconds(2f);
		timeShot=true;
	}

	IEnumerator _ScopeDown(){
		yield return new WaitForSeconds(0.3f);
		if (Scope.ins.isScope) {
			Scope.ins._ScopeTransition();
			audio_source.PlayOneShot(reloadm24);
			StartCoroutine(_timeShot());
		}
	}
}
