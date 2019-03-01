using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class OpenSettings : MonoBehaviour {
    private bool panel_state;
    private GameObject yawSlider;
    private GameObject rollSlider;
    private GameObject pitchSlider;
  
    public void Start() {
        gameObject.SetActive(false);
        panel_state = false;
        
    }
    
    public void set_active() {
        
        if (panel_state) {
            panel_state = false;
            gameObject.SetActive(false);
        }
        else {
            panel_state = true;
            gameObject.SetActive(true);
            yawSlider = GameObject.Find("YawSlider");
            rollSlider = GameObject.Find("RollSlider");
            pitchSlider = GameObject.Find("PitchSlider");
        
            if (yawSlider != null && rollSlider != null && pitchSlider != null) {
                yawSlider.SetActive(false);
                rollSlider.SetActive(false);
                pitchSlider.SetActive(false);
            }
        }
        
    }
}
