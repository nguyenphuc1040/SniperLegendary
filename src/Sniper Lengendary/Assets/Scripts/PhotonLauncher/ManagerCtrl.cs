using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerCtrl : MonoBehaviour
{
    private const string username = "username player";
    public bool isPlayPanel;
    public static ManagerCtrl ins;
    public int idScene;
    public int idPlayer;
    private void Awake() {
        _Makeins();
      //  _IsFirstTime();
    }
    void Start()
    {
    
    
    }
    // private void _IsFirstTime(){
    //     if (!PlayerPrefs.HasKey("NewPlayer")){
    //         PlayerPrefs.SetInt(FPS,30);
    //         PlayerPrefs.SetInt(level,0);
    //         PlayerPrefs.SetInt(Language,1);
    //         PlayerPrefs.SetInt(Joyst,1);
    //         PlayerPrefs.SetString(username,"");
    //         PlayerPrefs.SetInt("NewPlayer",0);
    //         PlayerPrefs.SetInt(PxCoin,0);
    //         foreach (var item in Warrior)
    //         {   
    //             PlayerPrefs.SetInt(item,0);
    //         }
    //         PlayerPrefs.SetInt(Warrior[1],1);
    //         PlayerPrefs.SetInt(RingScreenSetting,3);
    //     }
    // }

    void _Makeins(){
        if (ins!=null) {
            Destroy(gameObject);
        } else {
            ins = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    void Update()
    {
        
    }

    public void _SetUserName(string x){
        PlayerPrefs.SetString(username,x);
    }
    public string _GetUserName(){
        return PlayerPrefs.GetString(username);
    }
    public AudioSource audio_source;
    public AudioClip pressAudio;
    public void _BtnPress(){
        audio_source.PlayOneShot(pressAudio);
    }

}
