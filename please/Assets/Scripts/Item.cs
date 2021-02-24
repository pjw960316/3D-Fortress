using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{            
    private PlayerMovement player_movement;
    private PlayerHealth player_health;
    private const float player_stunned_time = 3f;
    private const int HEALING_AMOUNT = 10;

    private RaycastHit hit;

    //Potion & Trap은 Bomb와 벽과 다르게 아래로 떨어지지 않기 때문에 삭제 되지 않으므로
    //Potion & Trap은 구멍에서 생성시 OR 구멍이 생기면 삭제합니다.
    private void FixedUpdate()
    {
        Debug.DrawRay(transform.position, new Vector3(0, -1, 0), Color.green);
        /*if (Physics.Raycast(transform.position, Vector3.down, out hit, 5f))
        {
            Debug.Log(hit.collider.name);
        }*/
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 5f) == false)
        {
            if (gameObject.name == "Trap(Clone)" || gameObject.name == "Potion(Clone)") //Tag로 find 하면 좋지만 , Tag를 Obstacle로 모두 만들어서 , 어디서 오류날지 모름.
            {
                Debug.Log("No Floor");
                Destroy(gameObject);
            }
        }
    }
    //MapFront와 MapBack의 충돌을 계속 감지하는 쓸데없는 확인이 발생.
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Player")
        {
            return;
        }

        //아이템을 획득한 플레이어가 1인지 2인지.
        if(GameManager.Instance.playerTurn)
        {
            player_movement = GameObject.Find("Player1").GetComponent<PlayerMovement>();
            player_health = GameObject.Find("Player1").GetComponent<PlayerHealth>();
        }
        else
        {
            player_movement = GameObject.Find("Player2").GetComponent<PlayerMovement>();
            player_health = GameObject.Find("Player2").GetComponent<PlayerHealth>();
        }

        if (gameObject.name == "Trap(Clone)") 
        {
            Debug.Log("Step Trap!!!");
            player_movement.is_player_stunned = true;
            Invoke("CancleStun", player_stunned_time);
        }
        
        else if (gameObject.name == "Potion(Clone)")
        {
            Debug.Log("Get potion!!!");
            player_health.GetPotion(HEALING_AMOUNT);
            Destroy(gameObject);
        }       
    }

    private void CancleStun()
    {
        player_movement.is_player_stunned = false;
        player_movement.BecomeInvincible();
    }
}
