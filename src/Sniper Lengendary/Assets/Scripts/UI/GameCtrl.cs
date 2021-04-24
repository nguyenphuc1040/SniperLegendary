using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class GameCtrl : MonoBehaviour
{
    public static GameCtrl ins;
    public Slider mouseSensitivity, scopeSensivity;
    public GameObject settingPanel;
    public bool statusSettingPanel;
  
    void Awake()
    {
        ins = this;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(_FPS());
    }

    void Update()
    {
        _escKeyPress();
    }

    public void _escKeyPress(){
        if (Input.GetKeyDown(KeyCode.Escape)){
            statusSettingPanel = !statusSettingPanel;
            settingPanel.SetActive(statusSettingPanel);
            if (statusSettingPanel){
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            } else {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
               
            }
        }
    }
    public void _backMenu(){
        StartCoroutine(_backingmenu());
    }
    IEnumerator _backingmenu(){
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
        {
            yield return null;
        }
        SceneManager.LoadSceneAsync("Menu");
    }
    public bool isRevival;
    public void _revival(){
        Debug.Log("revival");
        isRevival = true;
    }
    public Text fpsText;
    int fps_int;
    IEnumerator _FPS(){
        fps_int = (int)(1.0f / Time.smoothDeltaTime);
        if (fps_int>0)  fpsText.text ="FPS: " + fps_int;
        yield return new WaitForSeconds(1f);
        StartCoroutine(_FPS());
    }
    public Image speakerStatus;
    public Sprite speakerON, speakerOFF;
    public void _setSpeakerStatus(bool stt){
        speakerStatus.sprite = (stt) ? speakerON : speakerOFF;
    }
}
