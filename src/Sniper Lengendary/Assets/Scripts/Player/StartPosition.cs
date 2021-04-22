using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPosition : MonoBehaviour
{
    public static StartPosition ins;
    public Transform[] startPos = new Transform[4];
    void Awake()
    {
        ins = this;
    }
    public Transform _getPosition(){
        return startPos[Random.Range(0,startPos.Length)];
    }

}
