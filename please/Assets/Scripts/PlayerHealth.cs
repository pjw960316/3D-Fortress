using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public int player_hp;
    public const int init_hp = 100;      
    private void Start()
    {
        player_hp = init_hp;
    }

    public void ApplyDamage(int damage){
        player_hp -= damage;
        UIManager.Instance.UpdateHpBar(player_hp);
    }

}
