using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour {
    private GameObject difficultyButton;

    // Start is called before the first frame update
    void Start() {
        difficultyButton = GameObject.Find("DifficultyButton");
        difficultyButton.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
