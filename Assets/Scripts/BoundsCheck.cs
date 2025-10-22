using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps a GameObject on screen.
/// Only works when using an orthographic main camera.
/// </summary>
public class BoundsCheck : MonoBehaviour {
    public enum eType {
        center,
        inset,
        outset
    };

    [Header("Inscribed")] 
    public eType boundsType = eType.center;
    public float radius = 1f;
    
    [Header("Dynamic")] 
    public float camWidth;
    public float camHeight; 
    
    void Awake() {
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;
    }

    void LateUpdate() {
        // Find the checkRadius that will enable selected bounds type
        float checkRadius = 0;
        if (boundsType == eType.inset) checkRadius = -radius;
        else if (boundsType == eType.outset) checkRadius = radius;
        
        Vector3 pos = transform.position;
        
        // Restrict X Position to camera width
        if (pos.x > camWidth + checkRadius)
            pos.x = camWidth + checkRadius;
        else if (pos.x < -camWidth - checkRadius)
            pos.x = -camWidth - checkRadius;
        
        // Restrict Y Position to camera width
        if (pos.y > camHeight + checkRadius)
            pos.y = camHeight + checkRadius;
        else if (pos.y < -camHeight - checkRadius)
            pos.y = -camHeight - checkRadius;

        transform.position = pos;
    }
}
