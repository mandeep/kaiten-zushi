using System.Collections.Generic;
using UnityEngine;

public class BeltObjects : MonoBehaviour {
    private List<GameObject> collisions;
    private float speed;
    private bool speed_changed;
    

    private void Start() {
        collisions = new List<GameObject>();
        speed_changed = false;
        speed = 1.0f;
    }

    public void set_speed(float value) {
        speed = value;
        speed_changed = true;
    }

    public float get_speed() {
        return speed;
    }

    private void Update() {
        if (speed_changed) {
            PlateMover.speed = speed;

            speed_changed = false;
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Plate") && other.gameObject != null) {
            collisions.Add(other.gameObject);
        }
        
    }

    private void OnCollisionExit(Collision other) {
        if (other.gameObject.CompareTag("Plate")) {
            collisions.Remove(other.gameObject);
        }
    }

    public List<GameObject> get_colliders() {
        return collisions;
    }
}
