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
    [SerializeField] GameObject[] countDownUI;

    [Header("Gameplay")]
    public float movementSpeed = 10;
    bool gameOver = false;
    bool isGamePaused = false;
    float score;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayStart());

        score = 0;
        StartCoroutine(UpdateScore());
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.SetText("Score: " + score);

        PauseOrUnpauseGame();
    }

    IEnumerator DelayStart()
    {
        PauseMovement();
        countDownUI[0].SetActive(true);
        for (int i = 1; i < countDownUI.Length; i++)
        {
            yield return new WaitForSecondsRealtime(1);
            countDownUI[i - 1].SetActive(false);
            countDownUI[i].SetActive(true);
        }
        ResumeMovement();
    }

    IEnumerator UpdateScore()
    {
        while (!gameOver)
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

    void PauseOrUnpauseGame()
    {
        if (!isGamePaused && (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)))
        {
            PauseGame();
        }
        else if (isGamePaused && Input.anyKeyDown && !Input.GetMouseButtonDown(0))
        {
            UnpauseGame();
        }
    }
    public void PauseGame()
    {
        isGamePaused = true;
        PauseMovement();
        pauseScreen.SetActive(true);
    }

    public void UnpauseGame()
    {
        isGamePaused = false;
        ResumeMovement();
        pauseScreen.SetActive(false);
    }

    void PauseMovement()
    {
        Time.timeScale = 0;
    }

    void ResumeMovement()
    {
        Time.timeScale = 1;
    }

    public void ExitGame()
    {
        gameOver = true;
        SceneManager.LoadScene(SceneName.Main_Menu);

    }

    public void GameOver()
    {
        player.GetComponent<PlayerController>().DisableMovement();
        movementSpeed = 0;
        gameOver = true;
        gameOverScreen.SetActive(true);

    }
}
