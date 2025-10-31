using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoundsCheck))]
public class ProjectileEnemy : MonoBehaviour {
    private BoundsCheck bndCheck;
    
    void Awake() {
        bndCheck = GetComponent<BoundsCheck>();
    }
    
    void Update() {
        // Remove if offscreen
        if (!bndCheck.LocIs(BoundsCheck.eScreenLocs.onScreen)) {
            Destroy(gameObject);
        }
    }
}
