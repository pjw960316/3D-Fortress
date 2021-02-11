using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isFired;
    
    public Rigidbody ball;
    private PlayerInput playerInput;
    private Camera playerCamera;
    public float destroyTime;
    
    private float fireForce;
    void Start()
    {
        isFired = false;
        fireForce = 1;
        playerCamera = Camera.main;
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInput.isfireup && !isFired){
            fireForce = UIManager.Instance.powerSlide.value;
            Debug.Log(UIManager.Instance.powerSlide.value);
            Fire();
            
        }
    }

    private void Fire(){
        isFired = true;

        Rigidbody ballInstance = Instantiate(ball, playerCamera.transform.position, Quaternion.Euler(Vector3.zero));
        ballInstance.velocity = fireForce * playerCamera.transform.forward;
        Destroy(ballInstance.gameObject, destroyTime);
        
    }
}
