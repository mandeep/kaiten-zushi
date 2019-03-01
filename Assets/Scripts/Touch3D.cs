using System.Collections.Generic;
using System.Linq.Expressions;
using System.Numerics;
using System.Timers;
//using NUnit.Framework.Constraints;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Touch3D : MonoBehaviour {
    private Camera cam;
    private GameObject plate;
    private bool is_belt_touched;
    private bool is_plate_touched;
    private bool is_chef_touched;
    private bool mode;
    private GameObject buttonText;
    private GameObject pitchSlider;
    private GameObject rollSlider;
    private GameObject yawSlider;
    private float previous_roll_value;
    private float previous_yaw_value;
    private float previous_pitch_value;
    private Quaternion initial_rotation;
    private Vector3 initial_position;

    private void Awake() {
        initial_rotation = transform.rotation;
        initial_position = transform.position;
    }

    void Start() {
        cam = GetComponent<Camera>();
        is_belt_touched = false;
        is_plate_touched = false;
        is_chef_touched = false;
        plate = null;
        mode = false;
        buttonText = GameObject.Find("CameraText");
        pitchSlider = GameObject.Find("PitchSlider");
        rollSlider = GameObject.Find("RollSlider");
        yawSlider = GameObject.Find("YawSlider");
        previous_roll_value = rollSlider.GetComponent<Slider>().value;
        previous_yaw_value = yawSlider.GetComponent<Slider>().value;
        previous_pitch_value = pitchSlider.GetComponent<Slider>().value;
        
    }
    
    public void set_mode() {

        
        if (buttonText != null) {
            if (mode) {
                buttonText.GetComponent<Text>().text = "Restaurant";

                    yawSlider.SetActive(false);
                    rollSlider.SetActive(false);
                    pitchSlider.SetActive(false);
                    rollSlider.GetComponent<Slider>().value = 0;
                    yawSlider.GetComponent<Slider>().value = 0;
                    pitchSlider.GetComponent<Slider>().value = 0;
                    transform.rotation = initial_rotation;
                    transform.position = initial_position;
            }
            else {
                buttonText.GetComponent<Text>().text = "Player";

                    yawSlider.SetActive(true);
                    rollSlider.SetActive(true);
                    pitchSlider.SetActive(true);
                
            }
        }
        
        mode = !mode;
    }

    public void set_roll(float roll) {
        float delta = roll - previous_roll_value;
        transform.Rotate(Vector3.up * delta, Space.Self);
        previous_roll_value = roll;
    }
    
    public void set_yaw(float yaw) {
        float delta = yaw - previous_yaw_value;
        transform.Rotate(Vector3.right * delta, Space.Self);
        previous_yaw_value = yaw;
    }

    public void set_pitch(float pitch) {
        float delta = pitch - previous_pitch_value;
        transform.Translate(Vector3.forward * delta, Space.Self);
        previous_pitch_value = pitch;
        ;
    }
    
    // because eventSystem's pointerovergameobject method is broken this method has to be used
    // to detect UI over gameObjects. credit: https://forum.unity.com/threads/ispointerovereventsystemobject-always-returns-false-on-mobile.265372/
    private bool IsPointerOverUIObject() {
        // Referencing this code for GraphicRaycaster https://gist.github.com/stramit/ead7ca1f432f3c0f181f
        // the ray cast appears to require only eventData.position.
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
 
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }


    void FixedUpdate() {
        if (Input.touchCount > 0) {
            
            Touch touch = Input.GetTouch(0);
            if (!IsPointerOverUIObject()) {
            if (touch.phase == TouchPhase.Began) {
                
                Ray ray = cam.ScreenPointToRay(touch.position);
                RaycastHit hit;
                
                if (Physics.Raycast(ray, out hit)) {
                    if (hit.collider.gameObject.CompareTag("Plate") && !is_belt_touched && !is_chef_touched) {
                        plate = hit.collider.gameObject;
                        plate.GetComponent<Renderer>().material.color = Color.cyan;
                        is_plate_touched = true;
                    } 
                    else if (hit.collider.gameObject.CompareTag("Belt") && !is_plate_touched && !is_chef_touched) {
                        GameObject chef = GameObject.FindGameObjectWithTag("Chef");
                        ChefController belt_panel_object = chef.GetComponent<ChefController>();
                        
                        if (belt_panel_object != null) {
                            bool belt_panel_state = belt_panel_object.get_belt_panel();
                            if (belt_panel_state) {
                                belt_panel_object.set_belt_panel(false);
                            }
                            else {
                                belt_panel_object.set_belt_panel(true);
                            }
                        }

                        GameObject[] belts = GameObject.FindGameObjectsWithTag("Belt");

                        foreach (GameObject belt in belts) {
                            if (!is_belt_touched) {
                                belt.GetComponent<Renderer>().material.color = Color.red;
                            }
                            else {
                                belt.GetComponent<Renderer>().material.color = Color.grey;
                            }
                            
                            
                            List<GameObject> colliders = belt.GetComponent<BeltObjects>().get_colliders();
                            foreach (GameObject belt_collider in colliders) {
                                if (belt_collider) {
                                    if (belt_collider.GetComponent<PlateMover>().is_draggable()) {
                                        belt_collider.GetComponent<PlateMover>().set_draggable(false);
                                    }
                                    else {
                                        belt_collider.GetComponent<PlateMover>().set_draggable(true);
                                    }
                                }
                            }
                        }
                        
                        if (is_belt_touched) {
                            is_belt_touched = false;

                        }
                        else {
                            is_belt_touched = true;
                        }
                        
                        

                        if (chef.GetComponent<ChefController>().is_belt_paused()) {
                            chef.GetComponent<ChefController>().set_pause(false);    
                        }
                        else {
                            chef.GetComponent<ChefController>().set_pause(true);
                        }
                    }
                    else if (hit.collider.gameObject.CompareTag("Chef") && !is_plate_touched && !is_belt_touched) {
                        GameObject chef_boyardee = hit.collider.gameObject;

                        if (!is_chef_touched) {
                            chef_boyardee.GetComponent<Renderer>().material.color = Color.green;    
                        }
                        else {
                            chef_boyardee.GetComponent<Renderer>().material.color = new Color(1.0f, 0.4426f, 0.1179f);
                        }
                        
                        
                        ChefController chef_panel_object = hit.collider.gameObject.GetComponent<ChefController>();
                        bool chef_panel_state = chef_panel_object.get_panel();
                        if (chef_panel_state) {
                            chef_panel_object.set_panel(false);
                        }
                        else {
                            chef_panel_object.set_panel(true);
                        }
                        
                        if (is_chef_touched) {
                            is_chef_touched = false;

                        }
                        else {
                            is_chef_touched = true;
                        }
                    }    
                }
            }
            else if (touch.phase == TouchPhase.Moved && plate != null && !is_belt_touched && !is_chef_touched) {
                
                plate.GetComponent<Rigidbody>().isKinematic = true;
                plate.GetComponent<Rigidbody>().detectCollisions = false;
                plate.GetComponent<Rigidbody>().useGravity = false;
                plate.GetComponent<PlateMover>().set_draggable(true);
                Vector3 target = cam.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y,10.0f));
                plate.transform.position = target;
//                plate.GetComponent<PlateMover>().set_draggable(false);

            }
            else if (touch.phase == TouchPhase.Ended && plate != null) {
                if (is_plate_touched) {
                    plate.GetComponent<Renderer>().material.color = Color.white;
                }
                
                plate.GetComponent<Rigidbody>().isKinematic = false;
                plate.GetComponent<Rigidbody>().detectCollisions = true;
                plate.GetComponent<Rigidbody>().useGravity = true;
                plate.GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePosition;  

                is_plate_touched = false;

//                GameObject floor = GameObject.FindGameObjectWithTag("Floor");
//                List<GameObject> floor_colliders = floor.GetComponent<FloorObjects>().get_colliders();
//                foreach (GameObject floor_collider in floor_colliders) {
//                    if (floor_collider) {
//                        if (floor_collider.gameObject.CompareTag("Plate")) {
//                            Destroy(floor_collider);
//                        }
//                    }
//                }
}
            }
        }
    }

    private void LateUpdate() {
                        
        // freeze position and set not draggable if the plate has landed on a belt so that
        // the plate moves again. we also need to have this in LateUpdate so that the plate's
        // OnCollisionEnter method has time to finish
        if (plate != null) {
            bool is_touching_belt = plate.GetComponent<PlateMover>().is_on_belt();
            if (is_touching_belt) {
                plate.GetComponent<PlateMover>().set_draggable(false);
                plate.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }
}
