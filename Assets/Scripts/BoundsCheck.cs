using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Checks if a GameObject is on screen and can force it to stay on screen.
/// Only works when using an orthographic main camera.
/// </summary>
public class BoundsCheck : MonoBehaviour {
    [System.Flags]
    public enum eScreenLocs {
        onScreen = 0, // 0000
        offRight = 1, // 0001
        offLeft = 2, // 0010
        offUp = 4, // 0100
        offDown = 8 // 1000
    }
    
    public enum eType {
        center,
        inset,
        outset
    };

    [Header("Inscribed")] 
    public eType boundsType = eType.center;
    public float radius = 1f;
    public bool keepOnScreen = true;
        
    [Header("Dynamic")] 
    public float camWidth;
    public float camHeight;
    public eScreenLocs screenLocs = eScreenLocs.onScreen;
    
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
        screenLocs = eScreenLocs.onScreen;
        
        // Restrict X Position to camera width
        if (pos.x > camWidth + checkRadius) {
            pos.x = camWidth + checkRadius;
            screenLocs |= eScreenLocs.offRight;
        }
        if (pos.x < -camWidth - checkRadius) {
            pos.x = -camWidth - checkRadius;
            screenLocs |= eScreenLocs.offLeft;
        }

        // Restrict Y Position to camera width
        if (pos.y > camHeight + checkRadius) {
            pos.y = camHeight + checkRadius;
            screenLocs |= eScreenLocs.offUp;
        }
        if (pos.y < -camHeight - checkRadius) {
            pos.y = -camHeight - checkRadius;
            screenLocs |= eScreenLocs.offDown;
        }
        
        if (keepOnScreen && !isOnScreen) {
            transform.position = pos;
            screenLocs = eScreenLocs.onScreen;
        }
    }

    public bool isOnScreen {
        get { return (screenLocs == eScreenLocs.onScreen); }
    }

    public bool LocIs(eScreenLocs checkLoc) {
        if (checkLoc == eScreenLocs.onScreen) return isOnScreen;
        return ((screenLocs & checkLoc) == checkLoc);
    }
}
