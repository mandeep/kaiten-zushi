using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class DessertPlate : MonoBehaviour {
    private Transform ice_cream;
    
    
    void Start() {
        ice_cream = transform.GetChild(0);
    }

    
    void Update()
    {;
        float tau = Mathf.PI / 2f;
        ice_cream.transform.localPosition = new Vector3(0, Mathf.Sin (Time.time) / 20 / tau,Mathf.Cos ( Time.time ) / tau / 8);        
    }
}
