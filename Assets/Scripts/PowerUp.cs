using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class PowerUp : MonoBehaviour {
    [Header("Inscribed")]
    [Tooltip("Seconds before despawning")] public float lifetime = 5f;

    void Start() {
        Invoke(nameof(Despawn), lifetime);    
    }
    
    void Despawn() {
        Destroy(this.gameObject);
    }
}
