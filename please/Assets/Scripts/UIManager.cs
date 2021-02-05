using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    
    public Slider powerSlide;
    public Image crossHair;
    public Text remainTimeText;
    public Text announceText;

    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<UIManager>();
            
            return instance;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        powerSlide = GetComponent<Slider>();
        crossHair = GetComponent<Image>();


    }

    public void UpdateTimeText(int remainTime){
        remainTimeText.text = "Remain Time : " + remainTime;
    }

    public void SetAnnounceText(string text){
        announceText.text = text;
    }

}
