using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Enemy_2 : Enemy {
    [Header("Enemy_2 Inscribed")]
    public float lifeTime = 10;
    [Tooltip("Determines how much the sine wave eases the interpolation")] public float sinEccentricity = 0.6f;
    public AnimationCurve rotCurve;
    
    [Header("Enemy_2 Private")]
    [SerializeField] private float birthTime;
    [SerializeField] private Vector3 p0, p1;

    private Quaternion baseRotation;
    
    void Start() {
        // Pick point on left side of the screen
        p0 = Vector3.zero;
        p0.x = -bndCheck.camWidth - bndCheck.radius;
        p0.y = UnityEngine.Random.Range(-bndCheck.camHeight, bndCheck.camHeight);
        
        // Pick point on right side of the screen
        p1 = Vector3.zero;
        p1.x = bndCheck.camWidth + bndCheck.radius;
        p1.y = UnityEngine.Random.Range(-bndCheck.camHeight, bndCheck.camHeight);
        
        // Random chance to swap sides
        if (UnityEngine.Random.value > 0.5f) {
            p0.x *= -1;
            p1.x *= -1;
        }

        birthTime = Time.time;
        
        // Set up initial rotation
        transform.position = p0;
        transform.LookAt(p1, Vector3.back);
        baseRotation = transform.rotation;
    }

    public override void Move() {
        // u will be between 0 and 1 (for linear interpolation)
        float u = (Time.time - birthTime) / lifeTime;
        
        // If u > 1 it has been longer than lifeTime since birthTime
        if (u > 1) { 
            // Finished life, destroy
            Destroy(this.gameObject);
            return;
        }
        
        // Use rotCurve to set rotation about Y
        float shipRot = rotCurve.Evaluate(u) * 360;
        transform.rotation = baseRotation * Quaternion.Euler(-shipRot, 0, 0);
        
        // Adjust u by adding a U curve based on a Sine wave
        u = u + sinEccentricity * (Mathf.Sin(u * Mathf.PI * 2));
        
        // Interpolate the two linear interpolation points
        pos = (1 - u) * p0 + u * p1;
    }
}
