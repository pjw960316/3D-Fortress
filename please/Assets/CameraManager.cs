using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager instance;
   // public GameObject ballCam;

    public GameObject playerCam1;
    public GameObject playerCam2;

    public static CameraManager Instance{
        get{
            if(instance == null){
                instance = GameObject.FindObjectOfType<CameraManager>();
            }
            return instance;
        }
    }
    // Start is called before the first frame update
    public void FollowPlayer(bool turn){
        playerCam1.SetActive(turn);
        playerCam2.SetActive(!turn);
    }

    public void FollowBall(){
        playerCam1.SetActive(false);
        playerCam2.SetActive(false);
    }
}
