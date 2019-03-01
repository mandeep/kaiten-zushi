using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PlateMover : MonoBehaviour
{
    private Rigidbody rb;
    public static float speed;
    private Vector3 velocity;
    private bool is_dragged;
    private bool is_on_table;
    private String table_name;
    private float plate_timer;
    private bool check_timer;
    private bool is_touching_belt;
    private bool is_hard_mode;
    
    public void set_draggable(bool draggable) {
        is_dragged = draggable;
    }
    
    public bool is_draggable() {
        return is_dragged;
    }
    
    public String get_table_name() {
        return table_name;
    }

    public bool is_on_belt() {
        return is_touching_belt;
    }
    
    public void set_difficulty(bool mode) {
        is_hard_mode = mode;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        is_dragged = false;
        is_on_table = false;
        is_touching_belt = true;
        is_hard_mode = GameObject.Find("Chef").GetComponent<ChefController>().get_mode();
        table_name = null;
        plate_timer = 0.0f;
        check_timer = false;
        speed = GameObject.FindGameObjectWithTag("Belt").GetComponent<BeltObjects>().get_speed();
    }

    private void Update() {
//        Vector3.MoveTowards(transform.position, velocity, 0.1f);
        if (!is_dragged) {
            transform.Translate(velocity);
        }

        if (check_timer) {
            plate_timer += Time.deltaTime;
        }

        if (plate_timer > 10.0f) {
            GameObject[] tables = GameObject.FindGameObjectsWithTag("Table");
            foreach (GameObject table in tables) {
                int table_plate_count = table.GetComponent<TableCount>().get_plate_count();
                if (table.gameObject.name == table_name) {
                    table.GetComponent<TableCount>().set_plate_count(table_plate_count - 1);
                }
                GameObject.FindGameObjectWithTag("Chef").GetComponent<ChefController>().EatFX(this.gameObject.transform.position);
                Destroy(this.gameObject);
            }
            int current_count = GameObject.FindGameObjectWithTag("Chef").GetComponent<ChefController>().get_finished();
            GameObject.FindGameObjectWithTag("Chef").GetComponent<ChefController>().set_finished(current_count + 1);
        }
    }

    private void OnCollisionStay(Collision other)
    {
        
        if (other.gameObject.name == "Left Belt" || (other.gameObject.name == "Bottomleft Tray" && !is_hard_mode))
        {
            velocity = Vector3.forward * speed * Time.deltaTime;

        } else if (other.gameObject.name == "Top Belt" || (other.gameObject.name == "Topleft Tray" && !is_hard_mode))
        {
            velocity = Vector3.right * speed * Time.deltaTime;

        } else if (other.gameObject.name == "Right Belt" || (other.gameObject.name == "Topright Tray" && !is_hard_mode))
        {
            velocity = Vector3.back * speed * Time.deltaTime;

        } else if (other.gameObject.name == "Bottom Belt" || (other.gameObject.name == "Bottomright Tray" && !is_hard_mode))
        {
            velocity = Vector3.left * speed * Time.deltaTime;

        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Table")) {
            is_on_table = true;
            table_name = other.gameObject.name;
            check_timer = true;
        }
        
        if (other.gameObject.CompareTag("Floor")) {
            int current_count = GameObject.FindGameObjectWithTag("Chef").GetComponent<ChefController>().get_destroyed();
            GameObject.FindGameObjectWithTag("Chef").GetComponent<ChefController>().set_destroyed(current_count + 1);
            GameObject.FindGameObjectWithTag("Chef").GetComponent<ChefController>().DestroyFX(this.gameObject.transform.position);
            Destroy(this.gameObject);
        }

        if (other.gameObject.CompareTag("Plate")) {
            GameObject collided_plate = other.gameObject;
            
            if (collided_plate.GetComponent<PlateMover>().is_on_table) {
                GameObject[] tables = GameObject.FindGameObjectsWithTag("Table");
                foreach (GameObject table in tables) {
                    int table_plate_count = table.GetComponent<TableCount>().get_plate_count();
                    if (table.gameObject.name == collided_plate.GetComponent<PlateMover>().get_table_name()) {
                        table.GetComponent<TableCount>().set_plate_count(table_plate_count - 1);
                    }
                }
            }
            int current_count = GameObject.FindGameObjectWithTag("Chef").GetComponent<ChefController>().get_destroyed();
            GameObject.FindGameObjectWithTag("Chef").GetComponent<ChefController>().set_destroyed(current_count + 1);
            GameObject.FindGameObjectWithTag("Chef").GetComponent<ChefController>().DestroyFX(this.gameObject.transform.position);
            Destroy(this.gameObject);
        }

        if (other.gameObject.CompareTag("Belt")) {
            is_touching_belt = true;
        }

        if (other.gameObject.CompareTag("Tray") && is_hard_mode) {
            velocity = Vector3.zero;
        }
    }

    private void OnCollisionExit(Collision other) {
        if (other.gameObject.CompareTag("Table")) {
            check_timer = false;
            plate_timer = 0.0f;
        }
        if (other.gameObject.CompareTag("Belt")) {
            is_touching_belt = false;
//            is_dragged = true;
        }
    }
}
