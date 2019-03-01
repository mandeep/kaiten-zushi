using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialPlate : MonoBehaviour {

    private Transform sauce1;

    private Transform sauce2;
    // Start is called before the first frame update
    void Start() {
        sauce1 = transform.GetChild(4);
        sauce2 = transform.GetChild(5);
    }

    // Update is called once per frame
    void Update()
    {
          Vector3 axis = new Vector3(0.0f, 1.0f, 0.0f);
          sauce1.RotateAround(transform.position, axis, 90.0f * 2 * Time.deltaTime);
          sauce2.RotateAround(transform.position, axis, 90.0f * Time.deltaTime);
    }
}
