using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isDamage : MonoBehaviour
{
    public GameObject PlayerObj;
    PlayerCtrl parent;
    void Start()
    {
        parent = PlayerObj.GetComponent<PlayerCtrl>();
    }

    public void _isDamaging(int damage){
        parent._isDamaging(damage);
    }
}
