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
        // Remove if off the bottom of the screen
        if (bndCheck.LocIs(BoundsCheck.eScreenLocs.offDown)) {
            Destroy(gameObject);
        }
    }
}
