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
    public float powerUpSpawnChance = 0.25f;
    
    protected BoundsCheck bndCheck;
    
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
        if (bndCheck.LocIs(BoundsCheck.eScreenLocs.offDown)) {
            Destroy(gameObject);
        }
    }

    public virtual void Move() {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }

    void OnCollisionEnter(Collision coll) {
        GameObject otherGo = coll.gameObject;
        // If hit by ProjectileHero destroy projectile and self
        if (otherGo.GetComponent<ProjectileHero>() != null) {
            health--; // Reduce health on hit
            if (health <= 0) { // If health is 0 destroy
                Main.ENEMY_DESTROYED(this);
                Destroy(gameObject);
            }
            Destroy(otherGo); // Destroy projectile
        }
        else {
            Debug.Log("Enemy hit by non-ProjectileHero: " + otherGo.name);
        }
    }
}
