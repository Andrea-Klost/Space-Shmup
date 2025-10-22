using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {
    public static Hero S { get; private set; }

    [Header("Inscribed")] 
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;

    [Header("Dynamic")] [Range(0, 4)]
    public float shieldLevel = 1;

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
    
}
