using System.Collections;
using System.Collections.Generic;
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

    public PlayerMovement playerMovement1;

    public PlayerMovement playerMovement2;

    private bool isPlayerShoot;
    private int remainTime;

    private int totalTime;

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
    public GameObject[] spawning_objects = new GameObject[4]; //0 : Trap / 1 : Bomb / 2 : potion / 3 : TempItem
    private GameObject map_tile;
    private Renderer bomb_color;
    private Transform cur_bomb_transform; // TODO : 최근의 폭탄 위치에 의해 모두 삭제 되므로 추후에는 시간 조정해서 겹치지 않게 해야 합니다.
    private static HashSet<int> can_allocate_plane;
    public float obstacle_3_max_ht;

    private IEnumerator timercoroutine;


    private void Awake()
    {
        if (Instance != this) Destroy(gameObject);

        
        
    }

    private void Start()
    {
        totalTime = 20;

        can_allocate_plane = new HashSet<int>(); //내가 알기로는 C#은 메모리 해제를 g.c가 알아서 해줌...
        for (int i = 0; i < 100; i++)
        {
            can_allocate_plane.Add(i);
        }           

        Reset();
    }
    
    private void Reset(){
        playerTurn = !playerTurn;
        playerShooter1.isFired = false;
        playerShooter2.isFired = false;

        remainTime = totalTime;

        timercoroutine = Timer(remainTime);

        playerInput1.enabled = playerTurn;
        playerInput2.enabled = !playerTurn;

        UIManager.Instance.power_gauge.value = 0f;

        //반대 진영의 장애물 전체 삭제
        if (playerTurn == true)
        {
            GameObject[] obstacles_will_be_destroyed = GameObject.FindGameObjectsWithTag("BackMapObstacle");
            foreach(var i in obstacles_will_be_destroyed)
            {
                Destroy(i);
            }
        }
        else
        {
            GameObject[] obstacles_will_be_destroyed = GameObject.FindGameObjectsWithTag("FrontMapObstacle");
            foreach (var i in obstacles_will_be_destroyed)
            {
                Destroy(i);
            }
        }

        //장애물이 나타나는 위치를 겹치지 않게 체크하는 Set
        can_allocate_plane = new HashSet<int>();
        for (int i = 0; i < 100; i++)
        {
            can_allocate_plane.Add(i);
        }

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
    }
    IEnumerator RoundRoutine(){
        StartCoroutine(timercoroutine);
        if(playerTurn){
            UIManager.Instance.SetAnnounceText("Player1의 턴");
        }else{
            UIManager.Instance.SetAnnounceText("Player2의 턴");
        }
        

        
        CameraManager.Instance.FollowPlayer(playerTurn);
            
        //카메라 시점 현재 플레이어 따라가도록 설정

        // 마우스 우측키 누르고 있는동안 파워 슬라이드 게이지 왔다갔다 // private enum State pushdown push up 으로 구분? 
        // 다른 ball shooter 스크립트 만들어서 따로 구현?
        
        if(playerTurn){
            
            while(!playerShooter1.isFired){
                if(remainTime <= 3){
                    playerShooter1.isFired = true;
                }
                yield return null;
            }
            
        }else{
            
            while(!playerShooter2.isFired){
                if(remainTime <= 3){
                    playerShooter2.isFired = true;
                }
                yield return null;
            }
           
        }
        
        
        ball = GameObject.FindWithTag("Ball");

        CameraManager.Instance.FollowBall();

        playerInput1.enabled = false;
        playerInput2.enabled = false;

        UIManager.Instance.EnableCrossHair(false);

        while(true){
            if(ball == null)
                break;
            yield return null;
        }

        
        UIManager.Instance.EnableCrossHair(true);

        StopCoroutine(timercoroutine);
        timercoroutine = Timer(3);
        StartCoroutine(timercoroutine);

        CameraManager.Instance.FollowPlayer(playerTurn);
        playerInput1.enabled = playerTurn;
        playerInput2.enabled = !playerTurn;

        yield return new WaitForSeconds(3f);


        // 1초마다 남은시간 ui update 해줘야 함

        // 마우스 떼면 포탄에 화면 가운데에 있는 크로스헤어 방향으로 슬라이더 게이지 value의 힘으로 ball 게임오브젝트 발사
        // 플레이어를 바라보고 있던 카메라가 포탄을 부드럽게 추적

        // ball 게임오브젝트에는 스크립트 달고 OnTriggerEnter로 땅이나 상대 플레이어 닿으면 Physics.OverlapSphere로 범위 안에서 
        // Player가 있다면 AddExplosionForce 함수로 폭발력 가하기 (ball.cs) 

        // 가해진 힘의 세기에 비례해서 플레이어의 체력을 깎기, player에 붙어있는 applyDamage(int damage)함수 발동시켜서 체력 깎기

        // 다시 내 플레이어 시점으로 카메라 이동, remainTime 3초로 줄임 
        StopCoroutine(timercoroutine);
        Reset();
    }

    

    private void SpawnItems()
    {
        if (playerTurn == true)
        {
            map_tile = GameObject.Find("MapFront");
        }
        else
        {
            map_tile = GameObject.Find("MapBack");
        }

        int rand_zero_to_four = Random.Range(0, 4);
        int map_tile_partial = Random.Range(0, 100);
        float map_tile_partial_x = map_tile.transform.GetChild(map_tile_partial).transform.position.x;
        float map_tile_partial_z = map_tile.transform.GetChild(map_tile_partial).transform.position.z;        
        GameObject spawned_object;

        //Spawn 위치 랜덤(=겹치는 문제 발생) -> Spawn을 바닥의 100개 객체에서 고르기 + 중복은 Hashset을 이용해서 Spawn 하지 않음.
        if (can_allocate_plane.Contains(map_tile_partial) == true)
        {
            can_allocate_plane.Remove(map_tile_partial);
            spawned_object = Instantiate(spawning_objects[rand_zero_to_four], new Vector3(map_tile_partial_x, 0.03f, map_tile_partial_z),transform.rotation);
            //턴이 변경되었을 때 반대 진영의 트랩과 포션만 삭제되도록 설정합니다.  
            if (playerTurn == true && (rand_zero_to_four == 0 || rand_zero_to_four == 2))
            {
                spawned_object.tag = "FrontMapObstacle";
            }
            if (playerTurn == false && (rand_zero_to_four == 0 || rand_zero_to_four == 2))
            {
                spawned_object.tag = "BackMapObstacle";
            }
            //Debug.Log(playerTurn + "count " + can_allocate_plane.Count);
        }
        else
        {
            return;
        }
        

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
        Destroy(spawned_object, 30f); //TODO : 코루틴에서 플레이어 턴이 바뀌면 장애물 모두 태그로 찾아서 삭제.                
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
            if (spawned_object == null)
            {
                yield break;
            }
            spawned_object.transform.localScale += new Vector3(0, 0.2f, 0);
            yield return new WaitForSeconds(0.3f);
        }
    }


    IEnumerator Timer(int remain){
        remainTime = remain;
        while(remainTime > 0){
            UIManager.Instance.UpdateTimeText(remainTime);
            yield return new WaitForSeconds(1f);
            remainTime -= 1;
        }
        
    }
}