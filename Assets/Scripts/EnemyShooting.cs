using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

/// <summary>
/// This script makes a normal enemy randomly shoot projectiles. 
/// </summary>
public class EnemyShooting : MonoBehaviour {
    [Header("Inscribed")] 
    public float timeBeforeFirstShot = 0.25f;
    public float timeBetweenShots = 1f;
    public float projectileSpeed = 40f;
    public bool shootTowardsPlayer = false;
    public GameObject prefabProjectile;

    void Start() {
        // Start shooting after min to compensate for time it takes to get on screen
        Invoke(nameof(Fire), timeBeforeFirstShot); 
    }

    void Fire() { 
        // Spawn projectile and move down
        GameObject projGo = Instantiate(prefabProjectile);
        projGo.transform.position = transform.position;
        Rigidbody rigidB = projGo.GetComponent<Rigidbody>();
        rigidB.velocity = (shootTowardsPlayer) ? 
            (Main.GET_HERO_POSITION() - transform.position).normalized * projectileSpeed 
            :  Vector3.down * projectileSpeed;
        
        Invoke(nameof(Fire), timeBetweenShots);
    }

}
