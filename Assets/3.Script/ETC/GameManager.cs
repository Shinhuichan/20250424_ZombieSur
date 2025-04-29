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
    // private static GameManager Instance
    // {
    //     get 
    //     {
    //         if (instance == null)
    //             instance = FindAnyObjectByType<GameManager>();
    //         return instance; 
    //     }
    // }

    public int score = 0;
    public bool isGameOver {get; private set;}

    // private void Start()
    // {
    //     FindObjectOfType<PlayerHealth>().onDead += EndGame;
    // }

    public void EndGame()
    {
        // UI Update
        UIManager.instance.SetActive_GameOver(false);
        SetScore(-score, true);
        score = 0;
        // Debug.Log("END GAME");
    }
    public void SetGame(bool b)
    {
        isGameOver = b;
    }

    public void SetScore(int newScore = 10, bool isReset = false)
    {
        if (!isGameOver)
        {
            score = !isReset ? score += newScore : score = 0;

            // UI Update
            UIManager.instance.Update_ScoreText(score);
        }
    }
} 