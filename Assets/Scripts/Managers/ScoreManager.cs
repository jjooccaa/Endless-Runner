using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] TextMeshProUGUI scoreText;

    public const string HIGH_SCORE = "highScore";

    int score = 0;
    public int Score
    { get { return score; }}

    float highScore;

    private void Start()
    {
        StartCoroutine(UpdateScore());
    }

    void Update()
    {
        DisplayScore();

        SaveHighScore();
    }

    IEnumerator UpdateScore()
    {
        while (!GameManager.Instance.IsGameOver)
        {
            score += 1;
            yield return new WaitForSeconds(1);
        }
    }

    void DisplayScore()
    {
        if (scoreText != null)
        {
            scoreText.SetText("Score: " + score);
        }
    }

    void SaveHighScore()
    {
        if (score > PlayerPrefs.GetFloat(HIGH_SCORE))
        {
            highScore = score;
            PlayerPrefs.SetFloat(HIGH_SCORE, highScore);
        }
    }

    public void RestartScore()
    {
        score = 0;
    }
}
