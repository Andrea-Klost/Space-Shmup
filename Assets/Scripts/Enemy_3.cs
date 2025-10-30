using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy_3 : Enemy {
    [Header("Enemy_3 Inscribed")]
    public float lifeTime = 5;
    public Vector2 midPointYRange = new Vector2(1.5f, 3);
    [Tooltip("If true, the Bezier points and path are drawn in the scene pane.")] public bool drawDebugInfo = true;

    [Header("Enemy_3 Private Fields")] 
    [SerializeField] private Vector3[] points; // Points on Bezier curve
    [SerializeField] private float birthTime;

    void Start() {
        points = new Vector3[3]; // Initialize points
        
        // Start position is already set by SpawnEnemy()
        points[0] = pos;
        
        // Set xMin and xMax the same way as Main.SpawnEnemy() does
        float xMin = -bndCheck.camWidth + bndCheck.radius;
        float xMax = bndCheck.camWidth - bndCheck.radius;
        
        // Pick random middle position in the bottom half of the screen
        points[1] = Vector3.zero;
        points[1].x = UnityEngine.Random.Range(xMin, xMax);
        float midYMult = UnityEngine.Random.Range(midPointYRange[0], midPointYRange[1]);
        points[1].y = -bndCheck.camHeight * midYMult;
        
        // Pick random final position above top of the screen;
        points[2] = Vector3.zero;
        points[2].y = pos.y;
        points[2].x = UnityEngine.Random.Range(xMin, xMax);
        
        // Set the birthTime to the current time
        birthTime = Time.time;

        if (drawDebugInfo) DrawDebug();
    }

    public override void Move() {
        // Bezier curves are based on a u value between 0 and 1
        float u = (Time.time - birthTime) / lifeTime;

        // This Enemy_3 has finished its life
        if (u > 1) {
            Destroy(this.gameObject);
            return;
        }
        
        transform.rotation = Quaternion.Euler(u * 180, 0, 0);
        
        // Interpolate the three Bezier curve points
        u = u - 0.1f * Mathf.Sin(u * Mathf.PI * 2); // Speedup middle and slowdown beginning and end
        pos = Utils.Bezier(u, points);
    }

    void DrawDebug() {
        // Draw the three points
        Debug.DrawLine(points[0], points[1], Color.cyan, lifeTime);
        Debug.DrawLine(points[1], points[2], Color.cyan, lifeTime);

        // Draw the Bezier Curve
        float numSections = 20;
        Vector3 prevPoint = points[0];
        Color col;
        Vector3 pt;

        for (int i = 1; i < numSections; i++) {
            float u = i / numSections;
            pt = Utils.Bezier(u, points);
            col = Color.Lerp(Color.cyan, Color.yellow, u);
            Debug.DrawLine(prevPoint, pt, col, lifeTime);
            prevPoint = pt;
        }
    }
}
