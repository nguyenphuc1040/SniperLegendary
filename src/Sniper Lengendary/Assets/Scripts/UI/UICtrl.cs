using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICtrl : MonoBehaviour
{
    public static UICtrl ins;
    public GameObject ScopeOverlay,aimShot,weaponCam;
    private void Awake() {
        ins = this;
        _ScopeOnOff(false);
    }
    public void _ScopeOnOff(bool x){
        ScopeOverlay.SetActive(x);
        weaponCam.SetActive(!x);
        aimShot.SetActive(!x);
    }
}
