using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {
    [Header("Inscribed")] 
    public Transform playerTransform; // HeroShip
    public Transform[] panels; // The scrolling foregrounds
    [Tooltip("Speed at which the panels move in Y")]
    public float scrollSpeed = -30f;
    [Tooltip("Controls how much the panels react to player movement (Default: 0.25)")]
    public float motionMult = 0.25f;

    private float panelHt; // Height of panels
    private float depth; // Depth of panels (pos.z)

    void Start() {
        panelHt = panels[0].localScale.y;
        depth = panels[0].position.z;
        
        // Set initial positions of panels 
        panels[0].position = new Vector3(0, 0, depth);
        panels[1].position = new Vector3(0, panelHt, depth);
    }

    void Update() {
        float tY, tX = 0;
        tY = Time.time * scrollSpeed % panelHt + (panelHt * 0.5f);

        if (playerTransform != null) {
            tX = -playerTransform.transform.position.x * motionMult;
            tY += -playerTransform.transform.position.y * motionMult;
        }
        
        // Position panels[0]
        panels[0].position = new Vector3(tX, tY, depth);
        
        // Position panels[1]
        if (tY >= 0) {
            panels[1].position = new Vector3(tX, tY - panelHt, depth);
        }
        else {
            panels[1].position = new Vector3(tX, tY + panelHt, depth);
        }
    }
}
