using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public bool playerTurn;
    private float remainTime = 60f;
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GameManager>();
            
            return instance;
        }
    }

    private int score;
    public bool isGameover { get; private set; }

    private void Awake()
    {
        if (Instance != this) Destroy(gameObject);
    }
    
    private void Reset(){
        playerTurn = !playerTurn;
        StartCoroutine(RoundRoutine());
    }
    
    IEnumerator RoundRoutine(){

        UIManager.Instance.SetAnnounceText(playerTurn + "의 턴");

        //카메라 시점 현재 플레이어 따라가도록 설정

        // 마우스 우측키 누르고 있는동안 파워 슬라이드 게이지 왔다갔다 // private enum State pushdown push up 으로 구분? 
        // 다른 ball shooter 스크립트 만들어서 따로 구현?

        // 1초마다 남은시간 ui update 해줘야 함

        // 마우스 떼면 포탄에 화면 가운데에 있는 크로스헤어 방향으로 슬라이더 게이지 value의 힘으로 ball 게임오브젝트 발사
        // 플레이어를 바라보고 있던 카메라가 포탄을 부드럽게 추적

        // ball 게임오브젝트에는 스크립트 달고 OnTriggerEnter로 땅이나 상대 플레이어 닿으면 Physics.OverlapSphere로 범위 안에서 
        // Player가 있다면 AddExplosionForce 함수로 폭발력 가하기 (ball.cs) 

        // 가해진 힘의 세기에 비례해서 플레이어의 체력을 깎기, player에 붙어있는 applyDamage(int damage)함수 발동시켜서 체력 깎기

        // 다시 내 플레이어 시점으로 카메라 이동, remainTime 3초로 줄임
        
        yield return new WaitForSeconds(3f);

        Reset();

    }

}