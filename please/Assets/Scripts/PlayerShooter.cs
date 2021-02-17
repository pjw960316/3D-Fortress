using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{   
    public bool isFired;
    
    public Rigidbody ball_rigid;
    public GameObject ball;
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
            //Debug.Log(UIManager.Instance.powerSlide.value);
            Fire();            
        }
    }

    private void Fire()
    {
        isFired = true;  
        if(playerInput.cur_weapon_number == 1)
        {
            UseWeapon1();
        }
        else if (playerInput.cur_weapon_number == 2)
        {
            UseWeapon2();
        }
        else if (playerInput.cur_weapon_number == 3)
        {
            UseWeapon3();
        }

    }

    private void UseWeapon1() //일단은 기본무기로 민현이 코드 유지.
    {
        Debug.Log("Weapon_1 Fire !!!");
        Rigidbody ballInstance = Instantiate(ball_rigid, playerCamera.transform.position, Quaternion.Euler(Vector3.zero));
        ballInstance.velocity = fireForce * playerCamera.transform.forward;
        Destroy(ballInstance.gameObject, destroyTime);
    }

    private void UseWeapon2() //색상 변경 총알
    {
        Debug.Log("Weapon_2 Fire !!!");
        GameObject ball_instance = Instantiate(ball, playerCamera.transform.position, Quaternion.Euler(Vector3.zero));
        ball_instance.GetComponent<Rigidbody>().velocity = fireForce * playerCamera.transform.forward;
        ball_instance.GetComponent<Renderer>().material.color = Color.red;       
        Destroy(ball_instance.gameObject, destroyTime);
    }

    private void UseWeapon3() // 3발 사격
    {
        Debug.Log("Weapon_3 Fire !!!");        
    }
}
