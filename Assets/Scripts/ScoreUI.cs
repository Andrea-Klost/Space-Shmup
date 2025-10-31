using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour {
    [Header("Inscribed")]
    public GameObject scoreTextObject;
    public GameObject highScoreTextObject;

    private Text _scoreText;
    private Text _highScoreText;

    void Awake() {
        _scoreText = scoreTextObject.GetComponent<Text>();
        _highScoreText = highScoreTextObject.GetComponent<Text>();
    }

    void Start() {
        // Get updates whenever score changes
        Main.SUBSCRIBE_TO_SCORE(this);
        // Get initial scores
        UpdateScore();
        UpdateHighScore();
    }

    public void UpdateScore() {
        _scoreText.text = "Score: " + Main.GET_SCORE().ToString("#,0");
    }

    public void UpdateHighScore() {
        _highScoreText.text = "Best: " + Main.GET_HIGH_SCORE().ToString("#,0");
    }
}
