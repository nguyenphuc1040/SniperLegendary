using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPanel : MonoBehaviour
{
    public void _outGame(){
        Application.Quit();
    }
    public void _panelOff(){
        Launcher.ins.isExitPanel = !Launcher.ins.isExitPanel;
        this.gameObject.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) _outGame();
    }
}
