using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(BoundsCheck))]
public class Enemy : MonoBehaviour {
    [Header("Inscribed")] 
    public float speed = 10f; // m/s
    public float fireRate = 0.3f; // Seconds/Shot
    public float health = 10;
    public int score = 100;

    private BoundsCheck bndCheck;

    void Awake() {
        bndCheck = GetComponent<BoundsCheck>();
    }

    public Vector3 pos {
        get {
            return this.transform.position;
        }
        set {
            this.transform.position = value;
        }
    }

    void Update() {
        Move();
        
        // Destroy if off the bottom of the screen
        if (!bndCheck.isOnScreen) {
            if (pos.y < bndCheck.camHeight - bndCheck.radius) {
                Destroy(gameObject);
            }
        }
    }

    public virtual void Move() {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }
}
