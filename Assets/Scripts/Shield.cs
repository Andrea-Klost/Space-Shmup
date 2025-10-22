using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shield : MonoBehaviour {
    [Header("Inscribed")] 
    public float rotationsPerSecond = 0.1f;

    [Header("Dynamic")] 
    public int levelShown = 0;

    Material mat;

    void Start() {
        mat = GetComponent<Renderer>().material;
    }

    void Update() {
        // Read current level from Hero
        int currLevel = Mathf.FloorToInt(Hero.S.shieldLevel);
        // If different from shown level
        if (levelShown != currLevel) {
            levelShown = currLevel;
            // Adjust texture offset
            mat.mainTextureOffset = new Vector2(0.2f * levelShown, 0);
        }
        
        // Rotate shield
        float rZ = -(rotationsPerSecond * Time.time * 360) % 360f;
        transform.rotation = Quaternion.Euler(0, 0, rZ);
    }
}
