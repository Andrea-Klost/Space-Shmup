using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {
    private static Main S;

    [Header("Inscribed")]
    public GameObject[] prefabEnemies; // Array of enemy prefabs
    public float enemySpawnPerSecond = 0.5f; // Enemies spawned per second
    public float enemyInsetDefault = 1.5f; // Inset from sides
    public float gameRestartDelay = 2;
    
    private BoundsCheck bndCheck;

    void Awake() {
        S = this;
        bndCheck = GetComponent<BoundsCheck>();
        Invoke(nameof(SpawnEnemy), 1f / enemySpawnPerSecond);
    }
    
    public void SpawnEnemy() { 
        // Pick random enemy prefab
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);
        
        // Position Enemy above the screen at a random x position
        float enemyInset = enemyInsetDefault;
        if (go.GetComponent<BoundsCheck>() != null) {
            enemyInset = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }
        
        // Set initial position for spawned enemy
        Vector3 pos = Vector3.zero;
        float xMin = -bndCheck.camWidth + enemyInset;
        float xMax = bndCheck.camWidth - enemyInset;
        pos.x = Random.Range(xMin, xMax);
        pos.y = bndCheck.camHeight + enemyInset;
        go.transform.position = pos;
        
        // Invoke again
        Invoke(nameof(SpawnEnemy), 1f / enemySpawnPerSecond);
    }

    void DelayedRestart() {
        Invoke(nameof(Restart), gameRestartDelay);
    }

    void Restart() {
        // Reload scene
        SceneManager.LoadScene("__Scene_0");
    }

    static public void HERO_DIED() {
        S.DelayedRestart();
    }
}
