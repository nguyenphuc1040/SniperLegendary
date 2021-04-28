using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPosition : MonoBehaviour
{
    public static StartPosition ins;
    public Transform[] startPos = new Transform[4];
    public Transform[] vehicalPos = new Transform[1];
    public Transform[] vehicalPosClient = new Transform[5];
    public bool[] ispos = new bool[5];
    void Awake()
    {
        ins = this;
    }
    public Transform _getPosition(){
        return startPos[Random.Range(0,startPos.Length)];
    }
    public Transform _getVehicalPos(){
        return vehicalPos[0];
    }
}
