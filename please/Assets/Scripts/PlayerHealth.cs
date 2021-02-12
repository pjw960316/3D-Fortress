using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public int player_decreased_hp;

    //이게 깎여야 체력이 증가하는 로직 -> 0
    public const int DECREASED_HP = 0;     
    private void Start()
    {
        player_decreased_hp = 0;
    }

    public void ApplyDamage(int damage){
        player_decreased_hp += damage;
        UIManager.Instance.UpdateHpBar(player_decreased_hp);
    }

    public void GetPotion(int healing) 
    {
        player_decreased_hp -= healing;
        UIManager.Instance.UpdateHpBar(player_decreased_hp);
    }

}
