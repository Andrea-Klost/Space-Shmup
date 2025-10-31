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
    public float minTimeBetweenShots = 0.25f;
    public float maxTimeBetweenShots = 1f;
    public float projectileSpeed = 40f;
    public GameObject prefabProjectile;

    void Start() {
        Invoke(nameof(Fire), UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots));
    }

    void Fire() { 
        // Spawn projectile and move down
        GameObject projGo = Instantiate(prefabProjectile);
        projGo.transform.position = transform.position;
        Rigidbody rigidB = projGo.GetComponent<Rigidbody>();
        rigidB.velocity = Vector3.down * projectileSpeed;
        
        Invoke(nameof(Fire), UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots));
    }

}
