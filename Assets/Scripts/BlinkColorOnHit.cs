using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class BlinkColorOnHit : MonoBehaviour {
    private static float blinkDuration = 0.1f; // # of seconds to show damage
    private static Color blinkColor = Color.red;

    [Header("Dynamic")] 
    public bool showingColor = false;
    public float blinkCompleteTime; // Time to stop showing color

    private Material[] materials; // All materials of this and its children
    private Color[] originalColors;
    private BoundsCheck bndCheck;

    void Awake() {
        bndCheck = GetComponentInParent<BoundsCheck>();
        // Get materials and colors for this GameObject and its children
        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++) {
            originalColors[i] = materials[i].color;
        }
    }

    void Update() {
        if (showingColor && Time.time > blinkCompleteTime) RevertColors();
    }

    void OnCollisionEnter(Collision coll) {
        // Check for collision with ProjectileHero
        ProjectileHero p = coll.gameObject.GetComponent<ProjectileHero>();
        if (p != null) {
            if (bndCheck != null && !bndCheck.isOnScreen) return; // Do not show damage if offscreen
            SetColors();
        }
    }

    /// <summary>
    /// Sets the Albedo color of all materials in the materials array to blinkColor, sets showingColor to true, and
    /// sets the time that colors should be reverted.
    /// </summary>
    void SetColors() {
        foreach (Material m in materials) {
            m.color = blinkColor;
        }
        showingColor = true;
        blinkCompleteTime = Time.time + blinkDuration;
    }

    /// <summary>
    /// Reverts all materials in the materials array to their original colors and sets showingColor to false.
    /// </summary>
    void RevertColors() {
        for (int i = 0; i < materials.Length; i++) {
            materials[i].color = originalColors[i];
        }
        showingColor = false;
    }
}
