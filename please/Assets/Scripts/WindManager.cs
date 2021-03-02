using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindManager : MonoBehaviour
{
    private float time_span = 0f;
    private int change_wind_path_time = 0;
    public static Vector3 wind_path; 

    private static WindManager instance;
    public static WindManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<WindManager>();
            return instance;
        }
    }
        
    private void FixedUpdate()
    {
        time_span += Time.deltaTime;
        if(change_wind_path_time < time_span)
        {
            time_span = 0;
            change_wind_path_time = Random.Range(25, 45); //바람의 변화 주기는 랜덤.

            /*
             * TODO : 50fps에서 부는 바람
             * 바람의 값을 변화시켜 대충 테스트 해본 결과 : 살짝 쏘면 바람에 영향을 많이 받고 강하게 쏘면 덜 받으므로
             * 바람 느낌은 확실히 든다.
             * x축 : 미사일 좌우 , z축 : 미사일 앞뒤(쏘는 방향과 반대인데 너무 약하면 바람에 밀림)
             * */
            wind_path.x = Random.Range(-100f, 100f)/800f;
            wind_path.y = 0f;
            wind_path.z = Random.Range(-100f, 100f)/800f;
            Debug.Log("현재 바람의 방향+힘 : " + wind_path.x + " " + wind_path.y + " " + wind_path.z); //position으로 움직임.
            
            //민현이에게 : wind_path에 값을 곱해서 UI에 나타내면 좋을듯.
        }
    }
}
