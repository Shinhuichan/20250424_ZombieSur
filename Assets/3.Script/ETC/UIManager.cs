using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    // private static UIManager Instance
    // {
    //     get 
    //     {
    //         if (instance == null)
    //             instance = FindAnyObjectByType<UIManager>();
    //         return instance; 
    //     }
    // }

    /* 
        탄약 표시 텍스트
        점수 표시 텍스트
        적 웨이트 텍스트
        게임 오버 오브젝트 활성화
        게임 재시작 -> Button Event
    */
    [SerializeField] private Text ammoText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text waveText;

    [SerializeField] private GameObject gameOver;

    private void Start()
    {
        gameOver.SetActive(false);
    }

    // Ammo Update
    public void Update_AmmoText(int magAmmo, int remain)
    {
        ammoText.text = string.Format($"{magAmmo} / {remain}");
    }

    // Score Update
    public void Update_ScoreText(int score)
    {
        scoreText.text = string.Format($"Score : {score}");
    }

    // Wave Update
    public void Update_WaveText(int wave, int Count)
    {
        waveText.text = string.Format($"Wave : {wave} \n Zombie Left : {Count}");
    }

    // GameOver UI Controller
    public void SetActive_GameOver(bool isActive)
    {
        gameOver.SetActive(isActive);
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SetActive_GameOver(false);
        GameManager.instance.SetGame(false);
        GameManager.instance.EndGame();
    }
}