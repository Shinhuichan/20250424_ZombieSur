using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public int score = 0;
    public bool isGameOver {get; private set;}

    private void Start()
    {
        FindObjectOfType<PlayerHealth>().onDead += EndGame;
    }

    public void EndGame()
    {
        SetGame(true);
        // UI Update
        UIManager.instance.SetActive_GameOver(true);
        AddScore(-score);
    }
    public void SetGame(bool b)
    {
        isGameOver = b;
    }

    public void AddScore(int newScore = 10)
    {
        if (!isGameOver)
        {
            score += newScore;
            // UI Update
            UIManager.instance.Update_ScoreText(score);
        }
    }
} 