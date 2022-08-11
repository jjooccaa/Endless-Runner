using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] TextMeshProUGUI scoreText;

    float score = 0;
    float highScore;
    const string High_Score = "highScore";

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(UpdateScore());
    }

    // Update is called once per frame
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
        scoreText.SetText("Score: " + score);
    }

    void SaveHighScore()
    {
        if (score > PlayerPrefs.GetFloat(High_Score))
        {
            highScore = score;
            PlayerPrefs.SetFloat(High_Score, highScore);
        }
    }

    public void RestartScore()
    {
        score = 0;
    }

    public float Score
    {
        get
        {
            return score;
        }
    }
}
