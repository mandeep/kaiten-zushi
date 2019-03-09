using System;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PlateMover : MonoBehaviour
{
    private Rigidbody rb;
    public static float speed;
    private Vector3 velocity;
    private bool isDragged;
    private bool isOnTable;
    private String tableName;
    private float plateTimer;
    private bool checkTimer;
    private bool isTouchingBelt;
    private bool isHardMode;
    private ChefController chefController;
    private GameObject[] tables;
    private BeltObjects beltController;
    
    public void SetDraggable(bool draggable) {
        isDragged = draggable;
    }
    
    public bool IsDraggable() {
        return isDragged;
    }
    
    public String GetTableName() {
        return tableName;
    }

    public bool IsOnBelt() {
        return isTouchingBelt;
    }
    
    public void SetDifficulty(bool mode) {
        isHardMode = mode;
    }

    private void Awake() {
        chefController = GameObject.Find("Chef").GetComponent<ChefController>();
        tables = GameObject.FindGameObjectsWithTag("Table");
        beltController = GameObject.FindGameObjectWithTag("Belt").GetComponent<BeltObjects>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        isDragged = false;
        isOnTable = false;
        isTouchingBelt = true;
        isHardMode = chefController.GetMode();
        tableName = null;
        plateTimer = 0.0f;
        checkTimer = false;
        speed = beltController.GetSpeed();
    }

    private void Update() {
        if (!isDragged) {
            transform.Translate(velocity);
        }

        if (checkTimer) {
            plateTimer += Time.deltaTime;
        }

        if (plateTimer > 10.0f) {
            foreach (GameObject table in tables) {
                int tablePlateCount = table.GetComponent<TableCount>().GetPlateCount();
                if (table.gameObject.name == tableName) {
                    table.GetComponent<TableCount>().SetPlateCount(tablePlateCount - 1);
                }
                chefController.EatFX(this.gameObject.transform.position);
                Destroy(this.gameObject);
            }
            int currentCount = chefController.GetFinished();
            chefController.SetFinished(currentCount + 1);
        }
    }

    private void OnCollisionStay(Collision other)
    {
        
        if (other.gameObject.name == "Left Belt" || (other.gameObject.name == "Bottomleft Tray" && !isHardMode))
        {
            velocity = Vector3.forward * speed * Time.deltaTime;

        } else if (other.gameObject.name == "Top Belt" || (other.gameObject.name == "Topleft Tray" && !isHardMode))
        {
            velocity = Vector3.right * speed * Time.deltaTime;

        } else if (other.gameObject.name == "Right Belt" || (other.gameObject.name == "Topright Tray" && !isHardMode))
        {
            velocity = Vector3.back * speed * Time.deltaTime;

        } else if (other.gameObject.name == "Bottom Belt" || (other.gameObject.name == "Bottomright Tray" && !isHardMode))
        {
            velocity = Vector3.left * speed * Time.deltaTime;

        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Table")) {
            isOnTable = true;
            tableName = other.gameObject.name;
            checkTimer = true;
        }
        
        if (other.gameObject.CompareTag("Floor")) {
            int currentCount = chefController.GetDestroyed();
            chefController.SetDestroyed(currentCount + 1);
            chefController.DestroyFX(this.gameObject.transform.position);
            Destroy(this.gameObject);
        }

        if (other.gameObject.CompareTag("Plate")) {
            GameObject collidedPlate = other.gameObject;
            
            if (collidedPlate.GetComponent<PlateMover>().isOnTable) {
                foreach (GameObject table in tables) {
                    int tablePlateCount = table.GetComponent<TableCount>().GetPlateCount();
                    if (table.gameObject.name == collidedPlate.GetComponent<PlateMover>().GetTableName()) {
                        table.GetComponent<TableCount>().SetPlateCount(tablePlateCount - 1);
                    }
                }
            }
            int currentCount = chefController.GetDestroyed();
            chefController.SetDestroyed(currentCount + 1);
            chefController.DestroyFX(this.gameObject.transform.position);
            Destroy(this.gameObject);
        }

        if (other.gameObject.CompareTag("Belt")) {
            isTouchingBelt = true;
        }

        if (other.gameObject.CompareTag("Tray") && isHardMode) {
            velocity = Vector3.zero;
        }
    }

    private void OnCollisionExit(Collision other) {
        if (other.gameObject.CompareTag("Table")) {
            checkTimer = false;
            plateTimer = 0.0f;
        }
        if (other.gameObject.CompareTag("Belt")) {
            isTouchingBelt = false;
        }
    }
}
