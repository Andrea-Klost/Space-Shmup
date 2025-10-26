using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy {
    [Header("Enemy_1 Inscribed")] 
    [Tooltip("# of seconds for a full sine wave")] public float waveFrequency = 2;
    [Tooltip("Sine wave width in meters")] public float waveWidth = 4;
    [Tooltip("Amount the ship will roll with the sine wave")] public float waveRotY = 45;

    private float x0; // Initial x value of position
    private float birthTime;

    private void Start() {
        x0 = pos.x; // Set x0 to x position
        birthTime = Time.time;
    }

    public override void Move() {
        // Move left and right in a sine wave
        Vector3 tempPos = pos;
        // theta adjusts based on time
        float age = Time.time - birthTime;
        float theta = Mathf.PI * 2 * age / waveFrequency;
        float sin = Mathf.Sin(theta);
        tempPos.x = x0 + waveWidth * sin;
        pos = tempPos;
        
        // rotate a bit about y
        Vector3 rot = new Vector3(0, sin * waveRotY, 0);
        this.transform.rotation = Quaternion.Euler(rot);
        
        base.Move();
        
        // print(bndCheck.isOnScreen);
    }
}
