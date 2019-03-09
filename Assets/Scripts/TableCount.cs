using UnityEngine;

public class TableCount : MonoBehaviour {
    private int plateCount;


    public int GetPlateCount() {
        return plateCount;
    }
    
    public void SetPlateCount(int num) {
        plateCount = num;
    }


    private void Start() {
        plateCount = 0;
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Plate")) {
            plateCount += 1;
        }

        if (plateCount > 4) {
            Destroy(other.gameObject);
            plateCount -= 1;
        }
    }

    private void OnCollisionExit(Collision other) {
        if (other.gameObject.CompareTag("Plate")) {
            plateCount -= 1;
        }
    }
}
