using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class FollowBall : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject ball;
    public Transform ballTransform;

    private CinemachineVirtualCamera virtualCamera;

    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

    }

    // Update is called once per frame
    void Update() // missile은 게임 중간에 만들어지는 것이므로 추적할 미사일 프리팹을 찾아줘야 한다.
    {
        
        if(ball == null){
            ball = GameObject.FindWithTag("Missile");

            if(ball != null){
                ballTransform = ball.transform;
                virtualCamera.LookAt = ballTransform;
                
            }
        }
        
    }
}
