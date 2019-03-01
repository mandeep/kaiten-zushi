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
    {
//        float y = Mathf.Sin(Time.time);
//        float x = Mathf.Cos(Time.time);
//        ice_cream.transform.Rotate(x, y, 0);
        float tau = Mathf.PI / 2f;
        ice_cream.transform.localPosition = new Vector3(0, Mathf.Sin (Time.time) / 20 / tau,Mathf.Cos ( Time.time ) / tau / 8);
//        ice_cream.transform.position = new Vector3(0.0f, 1.0f, 0.0f);
//        Vector3 y_axis = new Vector3(0.0f, 1.0f, 0.0f);
//        Vector3 x_axis = new Vector3(1.0f, 0.0f, 0.0f);
//        Vector3 z_axis = new Vector3(0.0f, 0.0f, 1.0f);
//        Vector3 position = new Vector3(transform.position.x, transform.position.y + 0.35f, transform.position.z);
//        empty.RotateAround(transform.parent.position, y_axis, 90.0f * Time.deltaTime);
//        ice_cream.RotateAround(position, y_axis, 90.0f * Time.deltaTime);
        
    }
}
