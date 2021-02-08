using UnityEngine;
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

    

    private void Start()
    {

    }

    // 매프레임 사용자 입력을 감지
    private void Update()
    {
        //게임오버 상태에서는 사용자 입력을 감지하지 않는다
        if (GameManager.Instance != null
            && GameManager.Instance.isGameover)
        {
            moveInput = Vector2.zero;
            isfiredown = false;
            jump = false;
            return;
        }

        moveInput = new Vector2(Input.GetAxis(moveHorizontalAxisName), Input.GetAxis(moveVerticalAxisName));
        if (moveInput.sqrMagnitude > 1) moveInput = moveInput.normalized;

        jump = Input.GetButtonDown(jumpButtonName);
        
        isfiredown = Input.GetButtonDown(fireButtonName);
        isfire = Input.GetButton(fireButtonName);                
        isfireup = Input.GetButtonUp(fireButtonName);
    }
     

    //플레이어 체력 감소를 UI에 표기 
    //일단 HP 감소 인자가 없으므로 그냥 -30
    private void ChangeHpBar()
    {
        hp_bar.value += 10;
    }   
    
}