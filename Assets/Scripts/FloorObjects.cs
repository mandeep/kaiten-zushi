using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorObjects : MonoBehaviour
{
    private List<GameObject> collisions;

    private void Start() {
        collisions = new List<GameObject>();
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Plate") && other.gameObject) {
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
