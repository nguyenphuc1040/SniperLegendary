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
                Cursor.lockState = CursorLockMode.None;
            } else {
                Cursor.lockState = CursorLockMode.Locked;
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
}
