using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
//using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.XR.WSA.Input;


public class ChefController : MonoBehaviour
{
    public GameObject[] plates;
    public GameObject[] effects;
    public GameObject spot_light;
    private LinkedList<GameObject> clones; 
    private float rate;
    private Vector3 spawn_point;
    private int count;
    private bool is_spawn = true;
    private bool is_paused = false;
    public GameObject panel;
    private bool panel_state;
    private bool rate_changed;
    public AudioClip explosion;
    public AudioClip groan;
    private AudioSource source;
    private int plates_destroyed;
    private int plates_finished;
    private bool mode;
    public GameObject belt_panel;
    private bool belt_panel_state;

    public bool get_mode() {
        return mode;
    }

    public void set_mode() {
        GameObject buttonText = GameObject.Find("DifficultyText");
        GameObject[] active_plates = GameObject.FindGameObjectsWithTag("Plate");
        
        if (buttonText != null) {
            if (mode) {
                buttonText.GetComponent<Text>().text = "Easy";
            }
            else {
                buttonText.GetComponent<Text>().text = "Hard";
            }
        }
        
        mode = !mode;
        
        foreach (GameObject plate in active_plates) {
            plate.GetComponent<PlateMover>().set_difficulty(mode);
        }

    }
    
    public void set_destroyed(int value) {
        plates_destroyed = value;
    }
    
    public void set_finished(int value) {
        plates_finished = value;
    }
    
    public int get_destroyed() {
        return plates_destroyed;
    }
    
    public int get_finished() {
        return plates_finished;
    }
    
    public void set_spawn(bool spawning) {
        is_spawn = spawning;
    }

    public bool is_spawning() {
        return is_spawn;
    }
    
    public void set_pause(bool paused) {
        is_paused = paused;
    }

    public bool is_belt_paused() {
        return is_paused;
    }
    
    public void set_rate(float new_rate) {
        rate = new_rate;
        rate_changed = true;
    }

 
    // Start is called before the first frame update
    void Start()
    {   
        spawn_point = new Vector3(0.0f, 1.05f, 5.0f);
        clones = new LinkedList<GameObject>();
        count = 0;
        panel_state = false;
        panel.SetActive(false);
        rate = 15;
        rate_changed = false;
        source = GetComponent<AudioSource>();
        plates_finished = 0;
        plates_destroyed = 0;
        belt_panel_state = false;
        mode = false;
        belt_panel.SetActive(false);
        InvokeRepeating("Spawn", 60 / rate, 60 / rate);
        
    }

    public void set_belt_panel(bool state) {
        belt_panel.SetActive(state);
        belt_panel_state = state;
    }

    public bool get_belt_panel() {
        return belt_panel_state;
    }
    
    public void set_panel(bool state) {
        panel.SetActive(state);
        panel_state = state;
    }

    public bool get_panel() {
        return panel_state;
    }

    private void Spawn()
    {
        if (!is_paused) {

            
            Collider[] hit_colliders = Physics.OverlapBox(spawn_point,
                plates[count].gameObject.transform.localScale / 2, Quaternion.identity);

            foreach (Collider hit in hit_colliders) {
                if (hit.CompareTag("Plate") && hit != null) {
                    is_spawn = false;
                }
                else {
                    is_spawn = true;
                }
            }

            if (is_spawn) {
                GameObject clone = Instantiate(plates[count], spawn_point, Quaternion.identity);
                clones.AddLast(clone);
                count += 1;
                count %= 3;
            }
        }
       
    }
 
    void Update()
    {

        if (rate_changed) {
            CancelInvoke("Spawn");
            InvokeRepeating("Spawn", 60 / rate, 60 / rate);
            rate_changed = false;
        }
  
        if (clones.Count > 0)
        {   
            GameObject clone = clones.First();
            

            if (clone == null) {
                clones.Remove(clone);
            }

            Vector3 destroy_point = new Vector3(-0.5f, 1.05f, 5.0f);
            
            if (clone != null && Vector3.Distance(clone.transform.position, destroy_point) < 0.2)
            {
                clones.Remove(clone);
//                source.PlayOneShot(explosion);
                plates_destroyed += 1;
                DestroyFX(clone.transform.position);
                Destroy(clone);
            }

            if (clone != null) {
                spot_light.transform.LookAt(clones.First().transform);
            }
        }
        else {
            spot_light.transform.LookAt(spawn_point);
        }
        
        GameObject destroyed_obj = GameObject.Find("Destroyed");
        Text destroyed_text = destroyed_obj.GetComponent<Text>();
        destroyed_text.text = "Plates Destroyed: " + plates_destroyed;
        
        GameObject finished_obj = GameObject.Find("Finished");
        Text finished_text = finished_obj.GetComponent<Text>();
        finished_text.text = "Plates Finished: " + plates_finished;
    }

    public void DestroyFX(Vector3 position) {
        source.PlayOneShot(explosion);
        GameObject explosion_effect = Instantiate(effects[0], position, Quaternion.identity);
        Destroy(explosion_effect, 2.0f);
//        float timer = 0.0f;
//        while (timer < 5.0f) {
//            timer += Time.deltaTime;
//        }
//        Destroy(explosion_effect);

    }
    
    public void EatFX(Vector3 position) {
        source.PlayOneShot(groan, 0.25f);
        GameObject fireworks_effect = Instantiate(effects[1], position, Quaternion.identity);
        Destroy(fireworks_effect, 2.0f);
//        float timer = 0.0f;
//        while (timer < 5.0f) {
//            timer += Time.deltaTime;
//        }
//        Destroy(fireworks_effect);
    }
}
