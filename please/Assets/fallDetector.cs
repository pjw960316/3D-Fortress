﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallDetector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other){
        if(other.tag.Equals("Player")){
            StartCoroutine(GameManager.Instance.Die(other.name));
        }
    }
}
