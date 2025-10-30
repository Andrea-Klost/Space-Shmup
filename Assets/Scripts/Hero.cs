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
    public GameObject projectilePrefab;
    public float projectileSpeed = 40;

    [Header("Dynamic")] [Range(0, 4)] [SerializeField]
    private float _shieldLevel = 1;
    
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

        if (Input.GetKeyDown(KeyCode.Space)) {
            TempFire();
        }
    }
    
    void TempFire() {
        GameObject projGo = Instantiate<GameObject>(projectilePrefab);
        projGo.transform.position = transform.position;
        Rigidbody rigidB = projGo.GetComponent<Rigidbody>();
        rigidB.velocity = Vector3.up * projectileSpeed;
    }
    
    void OnTriggerEnter(Collider other) {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;

        // If hit twice in a row by same source skip damage
        if (go == lastTriggerGo) return;
        lastTriggerGo = go;
       
        Enemy enemy = go.GetComponent<Enemy>();
        PowerUp power = go.GetComponent<PowerUp>();
        if (enemy != null) { // If triggered by an enemy decrease shield level and destroy enemy
            shieldLevel--;
            //Destroy(go);
        }
        else if (power != null) { // If PowerUp increase shield and destroy PowerUp
            shieldLevel++;
            Destroy(go);
        }
        else {
            
            Debug.LogWarning("Shield triggered by non-Enemy: " + go.name);
        }
    }

    public float shieldLevel {
        get { return _shieldLevel; }
        private set {
            _shieldLevel = Mathf.Min(value, 4); // Clamp to 4
            // If reduced to negative destroy hero
            if (value < 0) {
                Destroy(this.gameObject);
                Main.HERO_DIED();   
            }
        }
    }
}
