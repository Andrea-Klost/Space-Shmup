using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

[RequireComponent(typeof(EnemyShield))]
public class Enemy_4 : Enemy {
    [Header("Enemy_4 Inscribed")]
    public float duration = 4; // Duration of interpolation movement
    
    private EnemyShield[] allShields;
    private EnemyShield thisShield;
    private Vector3 p0, p1; // Points to interpolate
    private float timeStart; // Birthtime
    
    void Start() {
        allShields = GetComponentsInChildren<EnemyShield>();
        thisShield = GetComponent<EnemyShield>();
        
        // Initially place p0 and p1 to current position from Main.SpawnEnemy()
        p0 = p1 = pos;
        InitMovement();
    }

    void InitMovement() {
        p0 = p1; // Set p0 to old p1
        // Assign a new on-screen location to p1
        float widMinRad = bndCheck.camWidth - bndCheck.radius;
        float hgtMinRad = bndCheck.camHeight - bndCheck.radius;
        p1.x = UnityEngine.Random.Range(-widMinRad, widMinRad);
        p1.y = UnityEngine.Random.Range(-hgtMinRad, hgtMinRad);
        
        // Make sure that it moves to a different quadrant
        if (p0.x * p1.x > 0 && p0.y * p1.y > 0) {
            if (Mathf.Abs(p0.x) > Mathf.Abs(p0.y)) {
                p1.x *= -1;
            }
            else {
                p1.y *= -1;
            }
        }
        
        // Reset time
        timeStart = Time.time;
    }
    
    public override void Move() {
        float u = (Time.time - timeStart) / duration;

        if (u >= 1) {
            InitMovement();
            u = 0;
        }

        u = u - 0.15f * Mathf.Sin(u * 2 * Mathf.PI); // Easing: Sine -0.15
        pos = (1 - u) * p0 + u * p1; // Simple linear interpolation 
    }

    /// <summary>
    /// Enemy_4 collisions are handled differently to enable shield protection
    /// </summary>
    /// <param name="coll"></param>
    private void OnCollisionEnter(Collision coll) {
        GameObject otherGo = coll.gameObject;
        
        // Make sure this was hit by a ProjectileHero
        ProjectileHero p = otherGo.GetComponent<ProjectileHero>();
        if (p != null) {
            // Destroy ProjectileHero
            Destroy(otherGo);

            // Only damage if on screen
            if (bndCheck.isOnScreen) {
                // Find GameObject of Enemy_4 that was hit
                GameObject hitGo = coll.contacts[0].thisCollider.gameObject;
                if (hitGo == otherGo) {
                    hitGo = coll.contacts[0].otherCollider.gameObject;
                }

                float dmg = 1;
                
                // Find EnemyShield that was hit (if any)
                bool shieldFound = false;
                foreach (EnemyShield es in allShields) {
                    if (es.gameObject == hitGo) {
                        es.TakeDamage(dmg);
                        shieldFound = true;
                    }
                }

                if (!shieldFound) thisShield.TakeDamage(dmg);
                
                // If thisShield is still active, then it has not been destroyed
                if (thisShield.isActive) return;
                
                // Destroy this Enemy_4
                Destroy(gameObject);
            }
            else {
                Debug.Log("Enemy_4 hit by non-ProjectileHero: " + otherGo.name);
            }
        }
    }
}
