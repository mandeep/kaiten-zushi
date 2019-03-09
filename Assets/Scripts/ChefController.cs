using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ChefController : MonoBehaviour
{
    public GameObject[] plates;
    public GameObject[] effects;
    [FormerlySerializedAs("spot_light")] public GameObject spotLight;
    private LinkedList<GameObject> clones; 
    private float rate;
    private Vector3 spawnPoint;
    private int count;
    private bool isSpawn = true;
    private bool isPaused = false;
    public GameObject panel;
    private bool panelState;
    private bool rateChanged;
    public AudioClip explosion;
    public AudioClip groan;
    private AudioSource source;
    private int platesDestroyed;
    private int platesFinished;
    private bool mode;
    [FormerlySerializedAs("belt_panel")] public GameObject beltPanel;
    private bool beltPanelState;

    public bool GetMode() {
        return mode;
    }

    public void SetMode() {
        GameObject buttonText = GameObject.Find("DifficultyText");
        GameObject[] activePlates = GameObject.FindGameObjectsWithTag("Plate");
        
        if (buttonText != null) {
            if (mode) {
                buttonText.GetComponent<Text>().text = "Easy";
            }
            else {
                buttonText.GetComponent<Text>().text = "Hard";
            }
        }
        
        mode = !mode;
        
        foreach (GameObject plate in activePlates) {
            plate.GetComponent<PlateMover>().SetDifficulty(mode);
        }

    }
    
    public void SetDestroyed(int value) {
        platesDestroyed = value;
    }
    
    public void SetFinished(int value) {
        platesFinished = value;
    }
    
    public int GetDestroyed() {
        return platesDestroyed;
    }
    
    public int GetFinished() {
        return platesFinished;
    }
    
    public void SetPause(bool paused) {
        isPaused = paused;
    }

    public bool IsBeltPaused() {
        return isPaused;
    }
    
    public void SetRate(float new_rate) {
        rate = new_rate;
        rateChanged = true;
    }


    void Start()
    {   
        spawnPoint = new Vector3(0.0f, 1.05f, 5.0f);
        clones = new LinkedList<GameObject>();
        count = 0;
        panelState = false;
        panel.SetActive(false);
        rate = 15;
        rateChanged = false;
        source = GetComponent<AudioSource>();
        platesFinished = 0;
        platesDestroyed = 0;
        beltPanelState = false;
        mode = false;
        beltPanel.SetActive(false);
        InvokeRepeating("Spawn", 60 / rate, 60 / rate);
        
    }

    public void SetBeltPanel(bool state) {
        beltPanel.SetActive(state);
        beltPanelState = state;
    }

    public bool GetBeltPanel() {
        return beltPanelState;
    }
    
    public void SetPanel(bool state) {
        panel.SetActive(state);
        panelState = state;
    }

    public bool GetPanel() {
        return panelState;
    }

    private void Spawn()
    {
        if (!isPaused) {

            
            Collider[] hitColliders = Physics.OverlapBox(spawnPoint,
                plates[count].gameObject.transform.localScale / 2, Quaternion.identity);

            foreach (Collider hit in hitColliders) {
                if (hit.CompareTag("Plate") && hit != null) {
                    isSpawn = false;
                }
                else {
                    isSpawn = true;
                }
            }

            if (isSpawn) {
                GameObject clone = Instantiate(plates[count], spawnPoint, Quaternion.identity);
                clones.AddLast(clone);
                count += 1;
                count %= 3;
            }
        }
       
    }
 
    void Update()
    {

        if (rateChanged) {
            CancelInvoke("Spawn");
            InvokeRepeating("Spawn", 60 / rate, 60 / rate);
            rateChanged = false;
        }
  
        if (clones.Count > 0)
        {   
            GameObject clone = clones.First();
            

            if (clone == null) {
                clones.Remove(clone);
            }

            Vector3 destroyPoint = new Vector3(-0.5f, 1.05f, 5.0f);
            
            if (clone != null && Vector3.Distance(clone.transform.position, destroyPoint) < 0.2)
            {
                clones.Remove(clone);
                platesDestroyed += 1;
                DestroyFX(clone.transform.position);
                Destroy(clone);
            }

            if (clone != null) {
                spotLight.transform.LookAt(clones.First().transform);
            }
        }
        else {
            spotLight.transform.LookAt(spawnPoint);
        }
        
        GameObject destroyedObj = GameObject.Find("Destroyed");
        Text destroyedText = destroyedObj.GetComponent<Text>();
        destroyedText.text = "Plates Destroyed: " + platesDestroyed;
        
        GameObject finishedObj = GameObject.Find("Finished");
        Text finishedText = finishedObj.GetComponent<Text>();
        finishedText.text = "Plates Finished: " + platesFinished;
    }

    public void DestroyFX(Vector3 position) {
        source.PlayOneShot(explosion);
        GameObject explosionEffect = Instantiate(effects[0], position, Quaternion.identity);
        Destroy(explosionEffect, 2.0f);
    }
    
    public void EatFX(Vector3 position) {
        source.PlayOneShot(groan, 0.25f);
        GameObject fireworksEffect = Instantiate(effects[1], position, Quaternion.identity);
        Destroy(fireworksEffect, 2.0f);
    }
}
