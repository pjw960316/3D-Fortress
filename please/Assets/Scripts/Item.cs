using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    //trigger를 체크하기 위해 플레이어의 아래 부분 collider를 증가시킴.
        
    private PlayerMovement player_movement;
    private PlayerHealth player_health;
    private const float player_stunned_time = 3f;
    private const int HEALING_AMOUNT = 10;
        
    private void OnTriggerEnter(Collider other)
    {
        if(GameManager.Instance.playerTurn)
        {
            player_movement = GameObject.Find("Player1").GetComponent<PlayerMovement>();
            player_health = GameObject.Find("Player1").GetComponent<PlayerHealth>();
        }
        else
        {
            player_movement = GameObject.Find("Player2").GetComponent<PlayerMovement>();
            player_health = GameObject.Find("Player2").GetComponent<PlayerHealth>();
        } //고마워~

        
        if (gameObject.name == "Trap(Clone)") //TODO : 많은 부분이 겹치면 멈추도록. //TODO : 플레이어에게 무적시간을 주어 다시 안걸리게. 
        {
            player_movement.is_player_stunned = true;
            Invoke("CancleStun", player_stunned_time);
        }
        else if (gameObject.name == "Bomb(Clone)")
        {
                     
            

        }
        else if (gameObject.name == "Potion(Clone)")
        {
            Debug.Log("potion");
            player_health.GetPotion(HEALING_AMOUNT);
        }
        else
        {

        }
    }

    private void CancleStun()
    {
        player_movement.is_player_stunned = false;

        player_movement.BecomeInvincible();
    }
}
