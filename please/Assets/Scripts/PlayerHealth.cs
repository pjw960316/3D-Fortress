using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public int player_decreased_hp;

    public bool isDead;
    public Slider hpBar;
    public Text hpText;
    public int maxHealth = 10;

    public int curHelath;

    //이게 깎여야 체력이 증가하는 로직 -> 0
    public const int DECREASED_HP = 0;     
    private void Start()
    {
        hpBar.value = maxHealth;
        isDead = false;
        curHelath = maxHealth;
    }

    public void ApplyDamage(int damage){
        // player_decreased_hp += damage;
        // UIManager.Instance.UpdateHpBar(player_decreased_hp);
        //Debug.Log("Damage:" + damage);
        //Debug.Log("cur:" + curHelath);
        curHelath -= damage;
        hpBar.value = curHelath;
        hpText.text = curHelath.ToString();

        if(curHelath <= 0){
            GameManager.Instance.Die(gameObject.name);
        }
    }

    public void GetPotion(int healing) 
    {

        //player_decreased_hp -= healing;
        //UIManager.Instance.UpdateHpBar(player_decreased_hp);

        hpBar.value += healing;
    }

}
