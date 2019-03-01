using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableCount : MonoBehaviour {
    private int plate_count;


    public int get_plate_count() {
        return plate_count;
    }
    
    public void set_plate_count(int num) {
        plate_count = num;
    }


    private void Start() {
        plate_count = 0;
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Plate")) {
            plate_count += 1;
        }

        if (plate_count > 4) {
            Destroy(other.gameObject);
            plate_count -= 1;
        }
    }

    private void OnCollisionExit(Collision other) {
        if (other.gameObject.CompareTag("Plate")) {
            plate_count -= 1;
        }
    }
}
