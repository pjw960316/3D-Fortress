﻿using UnityEngine;
using UnityEngine.UI;

// 플레이어 캐릭터를 조작하기 위한 사용자 입력을 감지
// 감지된 입력값을 다른 컴포넌트들이 사용할 수 있도록 제공
public class PlayerInput : MonoBehaviour
{
    public string fireButtonName = "Fire1"; // 발사를 위한 입력 버튼 이름
    public string jumpButtonName = "Jump";
    public string moveHorizontalAxisName = "Horizontal"; // 좌우 회전을 위한 입력축 이름
    public string moveVerticalAxisName = "Vertical"; // 앞뒤 움직임을 위한 입력축 이름
    public Slider hp_bar; 

    // 값 할당은 내부에서만 가능
    public Vector2 moveInput { get; private set; }
    public bool isfire { get; private set; } // 감지된 발사 입력값

    public bool isfiredown {get; private set;}
    public bool isfireup {get; private set; }
    public bool jump { get; private set; }

    public int weapon_number { get; private set; }
    private const int max_weapon_cnt = 3;
    public int cur_weapon_cnt { get; private set; } //TODO : 아이템으로 무기 획득
    public int cur_weapon_number { get; private set; }
       
    private void Start()
    {
        cur_weapon_cnt = 4; // 현재 무기를 4개 먹었다고 가정.
        cur_weapon_number = 1;
        UIManager.Instance.UpdateWeaponNumber(cur_weapon_number);
    }

    // 매프레임 사용자 입력을 감지
    private void Update()
    {
        moveInput = new Vector2(Input.GetAxis(moveHorizontalAxisName), Input.GetAxis(moveVerticalAxisName));
        if (moveInput.sqrMagnitude > 1) moveInput = moveInput.normalized;

        jump = Input.GetButton(jumpButtonName);
        
        isfiredown = Input.GetButtonDown(fireButtonName);
        isfire = Input.GetButton(fireButtonName);                
        isfireup = Input.GetButtonUp(fireButtonName);

        if(Input.GetKeyDown(KeyCode.Tab) == true)
        {
            if(cur_weapon_number == cur_weapon_cnt)
            {
                cur_weapon_number = 1;
                UIManager.Instance.UpdateWeaponNumber(cur_weapon_number);
            }
            else
            {
                cur_weapon_number++;
                UIManager.Instance.UpdateWeaponNumber(cur_weapon_number);
            }
            Debug.Log("Player's Current Weapon Number : " + cur_weapon_number);
        }
    }
    
    

      
    
}