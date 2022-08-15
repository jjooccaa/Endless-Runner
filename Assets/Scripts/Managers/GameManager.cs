using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager>
{
    [Header("Player")]
    [SerializeField] GameObject player;

    [Header("UI")]
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject countDownText;

    //Gameplay
    float movementSpeed = 10;
    bool gameOver = false;
    bool isGamePaused = false;
    int startDelayer = 3;
    int numberOfLifes = 1;

    float difficultyIncreaser = 1;
    float increaseDifficultyAfter = 10;
    float scoreCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayStart(startDelayer));
    }

    // Update is called once per frame
    void Update()
    {
        IncreaseDifficultyOverTime(increaseDifficultyAfter);

        PauseOrUnpauseGame();
    }

    IEnumerator DelayStart(int startDelayer)
    {
        PauseMovement();

        countDownText.SetActive(true);
        for (int i = startDelayer; i >= 0; i--)
        {
            yield return new WaitForSecondsRealtime(1);

            if (i == 0)
            {
                countDownText.GetComponent<TextMeshProUGUI>().text = "GO!!!";
            }
            else
            {
                countDownText.GetComponent<TextMeshProUGUI>().text = i + "";
            }
        }

        UnpauseMovement();
    }

    void IncreaseDifficultyOverTime(float increaseDifficultyAfter)
    {
        // Increase difficulty for every value of increaseDifficultyAter
        if (ScoreManager.Instance.Score > scoreCounter)
        {
            movementSpeed += difficultyIncreaser;
            scoreCounter += increaseDifficultyAfter;
        }
    }

    public void RestartGame()
    {
        gameOver = true;
        ScoreManager.Instance.RestartScore();
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
        UnpauseMovement();
        pauseScreen.SetActive(false);
    }

    void PauseMovement()
    {
        Time.timeScale = 0;
    }

    void UnpauseMovement()
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
        movementSpeed = 0;
        gameOver = true;
        gameOverScreen.SetActive(true);
    }

    public void Death()
    {
        numberOfLifes--;
        if(numberOfLifes < 1)
        {
            EventManager.Instance.onGameOver?.Invoke();
        }
    }

    //Power Ups
    public void ActivatePowerUp(GameObject powerUp, int powerUpID)
    {
        switch(powerUpID)
        {
            case 1:
                ActivateInvisibilityPowerUp(5);
                break;
            case 2:
                ActivateExtraLifePowerUp();
                break;
            case 3:
                break;
        }

        powerUp.SetActive(false);
    }

    void ActivateExtraLifePowerUp()
    {
        numberOfLifes++;
    }

    void ActivateInvisibilityPowerUp(int duration)
    {
        Debug.Log("Invisibility activated");
        StartCoroutine(InvisibilityTimer(duration));
    }

    IEnumerator InvisibilityTimer(float time)
    {
        //Ignore collision between player and obstacles
        Physics.IgnoreLayerCollision(6, 7, true);

        yield return new WaitForSeconds(time);
        countDownText.SetActive(true);

        Physics.IgnoreLayerCollision(6, 7, false);
        Debug.Log("Invisibility deactivated");
    }

    public float MovementSpeed
    {
        get
        {
            return movementSpeed;
        }
    }
    public bool IsGameOver
    {
        get
        {
            return gameOver;
        }
    }
}
