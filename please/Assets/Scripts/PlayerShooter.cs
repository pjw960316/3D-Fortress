using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool isfired;
    
    public Rigidbody ball;
    private PlayerInput playerInput;
    private Camera playerCamera;
    public float destroyTime;
    
    private float fireForce;
    void Start()
    {
        isfired = false;
        fireForce = 1;
        playerCamera = Camera.main;
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInput.isfireup && !isfired){
            fireForce = UIManager.Instance.powerSlide.value;
            Debug.Log(UIManager.Instance.powerSlide.value);
            Fire();
            
            //test
            isfired = false;
        }
    }

    private void Fire(){
        isfired = true;

        Rigidbody ballInstance = Instantiate(ball, playerCamera.transform.position, playerCamera.transform.rotation);
        ballInstance.velocity = fireForce * playerCamera.transform.forward;
        Destroy(ballInstance.gameObject, destroyTime);
        
    }
}
