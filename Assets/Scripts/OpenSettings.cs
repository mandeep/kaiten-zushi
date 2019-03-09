using UnityEngine;

public class OpenSettings : MonoBehaviour {
    private bool panelState;
    private GameObject yawSlider;
    private GameObject rollSlider;
    private GameObject pitchSlider;
  
    public void Start() {
        gameObject.SetActive(false);
        panelState = false;
        
    }
    
    public void Enable() {
        
        if (panelState) {
            panelState = false;
            gameObject.SetActive(false);
        }
        else {
            panelState = true;
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
