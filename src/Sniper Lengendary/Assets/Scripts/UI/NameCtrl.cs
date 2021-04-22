using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NameCtrl : MonoBehaviour
{
    public InputField namePlayer;
    public Button submitNameBtn,changeNameBtn;
    public Animator menuAnimator;
    public GameObject OverlayPanel;
    void Start()
    {
        _setNameFirstTime();
        
    }

    void _setNameFirstTime(){
        if (!PlayerPrefs.HasKey("setNameFirstTime1")){
            PlayerPrefs.SetInt("setNameFirstTime",0);
            _changeName();
            StartCoroutine(_timeAnimator());
        }
    }
    void Update()
    {
        if (namePlayer.text==""){
            submitNameBtn.interactable = false;
        } else {
            submitNameBtn.interactable = true;
        }
    }
    public void _changeName(){
        OverlayPanel.SetActive(true);
        namePlayer.interactable = true;
        menuAnimator.SetTrigger("changeName");
        changeNameBtn.interactable = false;
    }
    public void _submitName(){
        OverlayPanel.SetActive(false);
        namePlayer.interactable = false;
        menuAnimator.SetTrigger("idle");
        changeNameBtn.interactable = true;
        if (ManagerCtrl.ins!=null) ManagerCtrl.ins._SetUserName(namePlayer.text);
    }
    IEnumerator _timeAnimator(){
        yield return new WaitForSeconds(0.2f);
        menuAnimator.SetTrigger("changeName");
    }
}
