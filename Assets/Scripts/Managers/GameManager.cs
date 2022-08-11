using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] GameObject player;
    
    [Header("UI")]
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] TextMeshProUGUI scoreText;

    bool gameOver = false;
    bool isGamePaused = false;
    float score;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        StartCoroutine(UpdateScore());
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.SetText("Score: " + score);

        if (!isGamePaused && (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)))
        {
            PauseGame();
        } 
        else if (Input.anyKeyDown)
        {
            UnpauseGame();
        }
    }

    IEnumerator UpdateScore()
    {
        while(!gameOver)
        {
            score += 1;
            yield return new WaitForSeconds(1);
        }
    }

    public void RestartGame()
    {
        gameOver = true;
        score = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        UnpauseGame(); 
    }

    public void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0;
        pauseScreen.SetActive(true);
    }

    public void UnpauseGame()
    {
        isGamePaused = false;
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
    }

    public void ExitGame()
    {
        gameOver = true;
        SceneManager.LoadScene(SceneName.Main_Menu);
    }

    public void GameOver()
    {
        player.GetComponent<PlayerController>().DisableMovement();
        gameOver = true;
        gameOverScreen.SetActive(true);
    }
}
