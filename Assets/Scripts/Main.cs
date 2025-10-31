using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {
    private static Main S;

    [Header("Inscribed")]
    public GameObject[] prefabEnemies; // Array of enemy prefabs
    public GameObject prefabPowerUp; // PowerUp prefab
    public float enemySpawnPerSecond = 0.5f; // Enemies spawned per second
    public float enemyInsetDefault = 1.5f; // Inset from sides
    public float gameRestartDelay = 2;
    public bool spawnEnemies = true;
    public Transform playerTransform; // HeroShip
    
    private BoundsCheck bndCheck;
    private bool _heroDied = false;
    
    void Awake() {
        S = this;
        bndCheck = GetComponent<BoundsCheck>();
        if (spawnEnemies)
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
        S._heroDied = true;
        S.DelayedRestart();
    }

    public static void ENEMY_DESTROYED(Enemy e) {
        if (Random.value <= e.powerUpSpawnChance) {
            // Instantiate new PowerUp
            GameObject go = Instantiate(S.prefabPowerUp);
            go.transform.position = e.transform.position; // Set PowerUp position to enemy's last position
        }
    }

    public static Vector3 GET_HERO_POSITION() {
        if (S._heroDied) // Avoid null reference during transition after hero dies
            return Vector3.zero;
        return S.playerTransform.position;
    }
}
