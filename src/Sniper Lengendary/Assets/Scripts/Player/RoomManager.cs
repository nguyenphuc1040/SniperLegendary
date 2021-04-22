using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager ins;
    private void Awake() {
        if (ins!=null) {
            Destroy(gameObject);
        } else {
            ins = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public override void OnEnable(){
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable(){
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode){
        if (scene.buildIndex == 1){ // scene game play online
           PhotonNetwork.Instantiate(Path.Combine("Player", "PlayerMain"),Vector2.zero, Quaternion.identity);
        }
    }
}