using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public bool playerTurn;

    private GameObject ball;
    public PlayerInput playerInput1;
    public PlayerInput playerInput2;

    public PlayerShooter playerShooter1;
    public PlayerShooter playerShooter2;

    private bool isPlayerShoot;
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

    private float time_span = 0f;
    public float spawn_time_limit;
    public GameObject[] spawning_objects = new GameObject[4];
    private GameObject map_tile;
    private Renderer bomb_color;
    private Transform cur_bomb_transform; // TODO : 최근의 폭탄 위치에 의해 모두 삭제 되므로 추후에는 시간 조정해서 겹치지 않게 해야 합니다.

    public float obstacle_3_max_ht;

    private void Awake()
    {
        if (Instance != this) Destroy(gameObject);

        
        Reset();
    }

    private void Start()
    {      
        
    }
    
    private void Reset(){
        playerTurn = !playerTurn;
        playerShooter1.isFired = false;
        playerShooter2.isFired = false;

        playerInput1.enabled = playerTurn;
        playerInput2.enabled = !playerTurn;
        
        StartCoroutine(RoundRoutine());
    }

    //파워 슬라이드의 코루틴 사용에 대해 이야기가 필요할 것 같아서 일단 하던 방식으로 구현함
    private void FixedUpdate()
    {
        time_span += Time.deltaTime;
        if(time_span > spawn_time_limit)
        {
            SpawnItems();
            time_span = 0f;
        }
        if(playerInput1.isfire || playerInput2.isfire)
        {
            UIManager.Instance.MovePowerGage();
        }        
        else
        {
            //power_gauge.value = 0;
        }
    }
    IEnumerator RoundRoutine(){

        UIManager.Instance.SetAnnounceText(playerTurn + "의 턴");

        
            CameraManager.Instance.FollowPlayer(playerTurn);
            
            
       
        
        //카메라 시점 현재 플레이어 따라가도록 설정

        // 마우스 우측키 누르고 있는동안 파워 슬라이드 게이지 왔다갔다 // private enum State pushdown push up 으로 구분? 
        // 다른 ball shooter 스크립트 만들어서 따로 구현?
        
        if(playerTurn){
            while(!playerShooter1.isFired){
                yield return null;
            }
        }else{
            while(!playerShooter2.isFired){
                yield return null;
            }
        }
        
        
        ball = GameObject.FindWithTag("Ball");
        Debug.Log(ball);

        CameraManager.Instance.FollowBall();
        
        while(true){
            if(ball == null)
                break;
            yield return null;
        }


        CameraManager.Instance.FollowPlayer(playerTurn);
        remainTime = 3f;

        yield return new WaitForSeconds(3f);


        // 1초마다 남은시간 ui update 해줘야 함

        // 마우스 떼면 포탄에 화면 가운데에 있는 크로스헤어 방향으로 슬라이더 게이지 value의 힘으로 ball 게임오브젝트 발사
        // 플레이어를 바라보고 있던 카메라가 포탄을 부드럽게 추적

        // ball 게임오브젝트에는 스크립트 달고 OnTriggerEnter로 땅이나 상대 플레이어 닿으면 Physics.OverlapSphere로 범위 안에서 
        // Player가 있다면 AddExplosionForce 함수로 폭발력 가하기 (ball.cs) 

        // 가해진 힘의 세기에 비례해서 플레이어의 체력을 깎기, player에 붙어있는 applyDamage(int damage)함수 발동시켜서 체력 깎기

        // 다시 내 플레이어 시점으로 카메라 이동, remainTime 3초로 줄임 

        Reset();
    }

    

    private void SpawnItems()
    {
        int rand_zero_to_four = 0;
        float start_value = 0;
        float rand_x = 0;
        float rand_z = 0;

        if (playerTurn == true)
        {
            start_value = -4f;
            map_tile = GameObject.Find("MapFront");         
        }
        else
        { 
            start_value = 16f;
            map_tile = GameObject.Find("MapBack");
        }

        rand_zero_to_four = Random.Range(0, 4);
        rand_x = Random.Range(-4f , 4f);
        rand_z = Random.Range(start_value, start_value + 8);        
        GameObject spawned_object = Instantiate(spawning_objects[rand_zero_to_four], new Vector3(rand_x,0.03f,rand_z), transform.rotation);

        if (rand_zero_to_four == 1) //Bomb
        {
            cur_bomb_transform = spawned_object.transform;
            spawned_object.transform.position += new Vector3(0, 0.5f, 0);
            spawned_object.transform.localScale = new Vector3(1, 1, 1);
            spawned_object.GetComponent<Renderer>().material.color = Color.red;
            //TODO : 정확한 시간 계산이 필요함.
            StartCoroutine("ChangeBombColor", spawned_object); //2.7초 진행
            Invoke("DestroyMapByBomb", 2.7f);
        }

        if (rand_zero_to_four == 3) //TempItem
        {
            StartCoroutine("UpperHeight",spawned_object);
        }
        Destroy(spawned_object, 60f); //TODO : 코루틴에서 플레이어 턴이 바뀌면 장애물 모두 태그로 찾아서 삭제.                
    }

    IEnumerator ChangeBombColor(GameObject spawned_object)
    {
        int max_change_cnt = 9;
        for(int i=0; i<max_change_cnt; i++)
        {
            if(spawned_object.GetComponent<Renderer>().material.color == Color.black)
            {
                spawned_object.GetComponent<Renderer>().material.color = Color.red;
            }
            else
            {
                spawned_object.GetComponent<Renderer>().material.color = Color.black;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }
        
    private void DestroyMapByBomb()
    {
        float bomb_x = cur_bomb_transform.position.x;
        float bomb_z = cur_bomb_transform.position.z;
        float map_tile_partial_x;
        float map_tile_partial_z;
        for (int i=0; i<100; i++)
        {
            map_tile_partial_x = map_tile.transform.GetChild(i).transform.position.x;
            map_tile_partial_z = map_tile.transform.GetChild(i).transform.position.z;
            if (bomb_x-1.5f <= map_tile_partial_x && map_tile_partial_x <= bomb_x+1.5f && bomb_z - 1.5f <= map_tile_partial_z && map_tile_partial_z <= bomb_z + 1.5f)
            {
                map_tile.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

    }
    IEnumerator UpperHeight(GameObject spawned_object)
    {
        while (spawned_object.transform.localScale.y < obstacle_3_max_ht)
        {
            spawned_object.transform.localScale += new Vector3(0, 0.2f, 0);
            yield return new WaitForSeconds(0.3f);
        }
    }

}