﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    
    public Slider powerSlide;
    
    public Slider hpBar;

    public Text winText;

    public Image crossHair;
    public Text remainTimeText;
    public Text announceText;

    public Slider power_gauge;
    private bool power_gauge_is_up = true;

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
        // powerSlide = GetComponent<Slider>();
        // crossHair = GetComponent<Image>();
        

    }

    public void UpdateTimeText(int remainTime){
        remainTimeText.text = "Remain Time : " + remainTime;
    }

    public void SetAnnounceText(string text){
        announceText.text = text;
    }

    public void UpdateHpBar(int value){
        hpBar.value = value;
    }


    public void MovePowerGage()
    {
        if(power_gauge.value == 0)
        {
            power_gauge.value = 1;
            power_gauge_is_up = true;
            return;
        }
        if (power_gauge.value == 100)
        {
            power_gauge.value = 99;
            power_gauge_is_up = false;
            return;
        }
        if (power_gauge_is_up == true)
        {
            power_gauge.value += 1;
        }
        else
        {
            power_gauge.value -= 1;
        }      
    }

    public void EnableCrossHair(bool enabled){
        crossHair.enabled = enabled;
    }

    public void EnalbeWinText(string player, bool enabled){
        winText.gameObject.SetActive(true);
        if(player == "player1"){
            
        }
        winText.text = player + " WIN!!!";
    }

}
