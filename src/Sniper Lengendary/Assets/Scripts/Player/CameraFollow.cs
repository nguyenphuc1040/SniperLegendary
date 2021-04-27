using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraFollow : MonoBehaviour
{
    GameObject follower;
    void FixedUpdate()
    {
        // if (isFollowSmooth){
        //     Vector3 desiredPostion = smoothObject.transform.position + offset;
        //     Vector3 smoothPostion = Vector3.Lerp(transform.position, desiredPostion,smoothSpeed);
        //     transform.position = smoothPostion;
        // }
    }

    void Update(){
       
        
    }

    public void _getFollower(GameObject x){
       follower = x;
       transform.rotation = follower.transform.rotation;
       transform.position = follower.transform.position;
       transform.SetParent(x.transform);
       
    }

    Vector3 offset;
    bool isFollowSmooth;
    public void _SetIsSmooth(bool x){
        isFollowSmooth = x;
    }

}
