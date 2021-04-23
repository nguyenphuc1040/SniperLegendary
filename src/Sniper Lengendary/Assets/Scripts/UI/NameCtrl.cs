using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NameCtrl : MonoBehaviour
{
    public static NameCtrl ins;
    public InputField namePlayer;
    public Button submitNameBtn,changeNameBtn;
    public Animator menuAnimator;
    public GameObject OverlayPanel;
    void Start()
    {
      
        ins = this;
    }

    public void _setNameFirstTime(){
        if (!PlayerPrefs.HasKey("setNameFirstTim123433131")){
            PlayerPrefs.SetInt("setNameFirstTim123433131",0);
            StartCoroutine(_timeAnimator());
        } else {
            namePlayer.text = ManagerCtrl.ins._GetUserName();
            namePlayer.interactable = false;
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
        _changeName();
    }
}
