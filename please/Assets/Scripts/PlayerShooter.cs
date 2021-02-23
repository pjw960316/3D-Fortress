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
    private const int SHOTGUN_BULLET_CNT = 3;
    private const float DISTANCE_DERIVEDBULLET_AND_ENEMY = 7f;
    void Start()
    {
        isFired = false;
        playerCamera = Camera.main;
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInput.isfireup && !isFired){
            fireForce = UIManager.Instance.powerSlide.value * 0.3f;
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
        else if (playerInput.cur_weapon_number == 4)
        {
            UseWeapon4();
        }

    }

    private void UseWeapon1() //일단은 기본무기로 민현이 코드 유지.
    {
        Debug.Log("Weapon_1 Fire !!!");
        Rigidbody ballInstance = Instantiate(ball_rigid, playerCamera.transform.position + playerCamera.transform.forward * 4f, Quaternion.Euler(Vector3.zero)); // 하늘을 보고 쐈을때 공 생성위치가 바닥 아래에서 생성되는것 방지
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
        GameObject[] ball_instance = new GameObject[SHOTGUN_BULLET_CNT];
        for (int i = 0; i < SHOTGUN_BULLET_CNT; i++)
        {
            ball_instance[i] = Instantiate(ball, playerCamera.transform.position + new Vector3(i,0,0), Quaternion.Euler(i * 30, 0, 0));
            ball_instance[i].GetComponent<Rigidbody>().velocity = fireForce * playerCamera.transform.forward;
        }
        for (int i = 0; i < SHOTGUN_BULLET_CNT; i++)
        {
            Destroy(ball_instance[i].gameObject, destroyTime);
        }        
    }
    private void UseWeapon4() // 유도탄 // TODO : 아직 유도탄의 Destroy()시점을 명확하게 못해서 구현 X
    {
        Debug.Log("Weapon_4 Fire !!!");
        GameObject ball_instance = Instantiate(ball, playerCamera.transform.position, Quaternion.Euler(Vector3.zero));
        ball_instance.GetComponent<Rigidbody>().velocity = fireForce * playerCamera.transform.forward;
        ball_instance.tag = "GuidedMissile";
        StartCoroutine("ShootDerivedBall" , ball_instance);
    }
    IEnumerator ShootDerivedBall(GameObject ball_instance)
    {
        GameObject enemy_player = null;
        if (gameObject.name == "Player1")
        {
            Debug.Log("P2 : " + GameObject.Find("Player2").transform.position);
            enemy_player = GameObject.Find("Player2");
        }
        else
        {
            Debug.Log("P1 : " + GameObject.Find("Player1").transform.position);
            enemy_player = GameObject.Find("Player1");
        }

        while (true)
        {
            if (GameObject.FindGameObjectWithTag("GuidedMissile") == false) //Except 
            {
                yield break;
            }
            Debug.Log("distance : " + Vector3.Distance(ball_instance.transform.position, enemy_player.transform.position));
            //유도탄과 상대의 거리가 x 미만이면 유도하는 코루틴 실행하고 현재 코루틴 종료
            if (Vector3.Distance(ball_instance.transform.position, enemy_player.transform.position) < DISTANCE_DERIVEDBULLET_AND_ENEMY) 
            {
                ball_instance.GetComponent<Rigidbody>().velocity = playerCamera.transform.forward * 0f; //포탄의 속도 제거          
                ball_instance.GetComponent<Rigidbody>().useGravity = false; //중력 제거
                StartCoroutine(ChaseEnemy(ball_instance, enemy_player.transform.position));
                yield break;
            }            
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator ChaseEnemy(GameObject ball_instance , Vector3 enemy_position)
    {       
        while(true)
        {
            if (GameObject.FindGameObjectWithTag("GuidedMissile") == false) //Except 
            {
                yield break;
            }
            Debug.Log("chase");
            
            ball_instance.transform.position -= (ball_instance.transform.position - enemy_position)/30; //TODO : 유도 되고 3초안에 맞아야 함.
            yield return new WaitForSeconds(0.1f);
        }
        
    }
}
