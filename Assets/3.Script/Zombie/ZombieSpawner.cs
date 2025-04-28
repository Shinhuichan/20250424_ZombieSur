using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public ZombieData[] zombieDatas;
    public ZombieController zombie;

    [SerializeField] private Transform[] spawnPoint;

    private List<ZombieController> zombieList = new List<ZombieController>();

    private int wave;

    private void SetupSpawnPoint()
    {
        spawnPoint = new Transform[transform.childCount];
        for (int i = 0; i < spawnPoint.Count(); i++)
        {
            // GetChild(index) -> 자식 객체를 순서대로 가지고 올 때, 사용한다.
            spawnPoint[i] = transform.GetChild(i).transform;
        }
    }

    private void Awake()
    {
        SetupSpawnPoint();
    }

    private void Update()
    {
        // 게임 오버일 경우
        if (GameManager.instance != null && GameManager.instance.isGameOver)
            return;

        if (zombieList.Count <= 0)
            // Wave 증가 메서드
            SpawnWave();
        Update_UI();
    }

    private void Update_UI()
    {
        UIManager.instance.Update_WaveText(wave, zombieList.Count);
    }

    private void SpawnWave()
    {
        // wave 증가
        wave++;
        int count = Mathf.RoundToInt(wave * 2f);
        for (int i = 0; i < count; i++)
            CreateZombie();
    }

    private void CreateZombie()
    {
        /* zombie 랜덤 설정
        Spawn Point Random하게 설정

        zombie가 죽었을 때 Event 추가
        1. List 삭제
        2. Object 삭제
        3. 점수 계산 -> GameManager */
        ZombieData data = zombieDatas[Random.Range(0, zombieDatas.Length)];
        Transform point = spawnPoint[Random.Range(0, spawnPoint.Length)];
        ZombieController z = Instantiate(zombie, point.position, point.rotation);
        z.Setup(data);
        zombieList.Add(z);

        // Zombie Spawn

        // 익명 함수는 우리가 1회용으로 사용하는 메서드를 이야기한다.
        // 대표적으로 람다식이 있다.
        // ( 매개 변수 ) => { 구현 부분 }
        z.onDead += () => {zombieList.Remove(z);};
        z.onDead += () => {Destroy(z.gameObject, 10f);};
        z.onDead += () => {GameManager.instance.AddScore();};
    }
}