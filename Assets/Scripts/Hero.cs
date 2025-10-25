using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hero : MonoBehaviour {
    public static Hero S { get; private set; }

    [Header("Inscribed")] 
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;

    [Header("Dynamic")] [Range(0, 4)]
    public float shieldLevel = 1;

    [Tooltip("This field holds a reference to the last triggering GameObject")]
    private GameObject lastTriggerGo = null;
    
    private void Awake() {
        if (S == null) {
            S = this;
        }
        else {
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S");
        }
    }

    void Update() {
        // Pull info from input
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");
        
        // Change transform based on axis
        Vector3 pos = transform.position;
        pos.x += hAxis * speed * Time.deltaTime;
        pos.y += vAxis * speed * Time.deltaTime;
        transform.position = pos;
        
        // Rotate ship in direction of movement
        transform.rotation = Quaternion.Euler(vAxis * pitchMult, hAxis * rollMult, 0); 
    }

    void OnTriggerEnter(Collider other) {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;

        // If hit twice in a row by same source skip damage
        if (go == lastTriggerGo) return;
        lastTriggerGo = go;
        
        Enemy enemy = go.GetComponent<Enemy>();
        if (enemy != null) { // If triggered by an enemy decrease shield level and destroy enemy
            shieldLevel--;
            Destroy(go);
        }
        else {
            Debug.LogWarning("Shield triggered by non-Enemy: " + go.name);
        }
    }
}
