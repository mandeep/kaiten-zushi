using System.Collections.Generic;
using UnityEngine;

public class BeltObjects : MonoBehaviour {
    private List<GameObject> collisions;
    private float speed;
    private bool speedChanged;
    

    private void Start() {
        collisions = new List<GameObject>();
        speedChanged = false;
        speed = 1.0f;
    }

    public void SetSpeed(float value) {
        speed = value;
        speedChanged = true;
    }

    public float GetSpeed() {
        return speed;
    }

    private void Update() {
        if (speedChanged) {
            PlateMover.speed = speed;

            speedChanged = false;
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

    public List<GameObject> GetColliders() {
        return collisions;
    }
}
