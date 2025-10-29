using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

[RequireComponent(typeof(EnemyShield))]
public class Enemy_4 : Enemy {
    private EnemyShield[] allShields;
    private EnemyShield thisShield;

    void Start() {
        allShields = GetComponentsInChildren<EnemyShield>();
        thisShield = GetComponent<EnemyShield>();
    }

    public override void Move() {
        
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
