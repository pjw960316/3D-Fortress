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
    void Update()
    {
        if(ball == null){
            ball = GameObject.FindWithTag("Ball");
            if(ball != null){
                ballTransform = ball.transform;
                virtualCamera.LookAt = ballTransform;
                virtualCamera.Follow = ballTransform;
            }
        }
    }
}
