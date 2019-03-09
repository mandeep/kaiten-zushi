using UnityEngine;

public class DessertPlate : MonoBehaviour {
    private Transform iceCream;
    private const float tau = Mathf.PI / 2f;
    
    void Start() {
        iceCream = transform.GetChild(0);
    }

    
    void Update() {
        iceCream.transform.localPosition = new Vector3(0, Mathf.Sin (Time.time) / 20 / tau,Mathf.Cos ( Time.time ) / tau / 8);        
    }
}
