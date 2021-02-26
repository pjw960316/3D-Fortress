using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager instance;
   // public GameObject ballCam;

    public GameObject playerCam1;
    public GameObject playerCam2;

    public GameObject missileCam;
    public static CameraManager Instance{
        get{
            if(instance == null){
                instance = GameObject.FindObjectOfType<CameraManager>();
            }
            return instance;
        }
    }
    // Start is called before the first frame update

    //Cinemachine 카메라는 여러대를 켜두고 priority 를 조정하여 가장 높은 priority의 카메라가 메인으로 보이는데 
    //setactive를 사용하여 직접 꺼주면 살아있는 카메라중 가장 우선순위가 높은 카메라가 메인이 된다.
    
    public void FollowPlayer(bool turn){
        playerCam1.SetActive(turn);
        playerCam2.SetActive(!turn);
        missileCam.SetActive(false);
    }

    public void FollowBall(){
        playerCam1.SetActive(false);
        playerCam2.SetActive(false);
        missileCam.SetActive(true);
    }

}
