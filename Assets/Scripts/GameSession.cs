using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerScore = 0;
    [SerializeField] int playerLives = 3;
    [SerializeField] float levelLoadingTime = 2f;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;

    void Awake()
    {
        int numGameSessions = FindObjectsByType<GameSession>(FindObjectsSortMode.None).Length;
        if (numGameSessions > 1) {
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = playerScore.ToString();
    }

    public void ResetPlayerScore()
    {
        Destroy(gameObject);
    }
    
    public void UpdatePlayerScore(int valueToAdd) 
    {
        playerScore += valueToAdd;
        scoreText.text = playerScore.ToString();
    }

    public IEnumerator ProcessPlayerDeath() 
    {
        yield return new WaitForSecondsRealtime(levelLoadingTime);
        if (playerLives > 1) {
            TakeLife();
        } else {
            FindFirstObjectByType<ScenePersist>().ResetScenePersist();
            ResetGameSession();
        }
    }

    void TakeLife()
    {
       playerLives --;
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
       livesText.text = playerLives.ToString();
    }

    void ResetGameSession()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(0);
    }


}
