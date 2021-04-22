using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraFollow : MonoBehaviour
{
    GameObject follower;
    void Update()
    {
        
    }

    public void _getFollower(GameObject x){
       follower = x;
       transform.rotation = Quaternion.identity;
       transform.position = follower.transform.position;
       transform.SetParent(x.transform);
    }
}
