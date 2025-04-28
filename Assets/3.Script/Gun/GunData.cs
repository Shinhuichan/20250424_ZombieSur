using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/GunData", fileName = "Gun_Data")]
public class GunData : ScriptableObject
{
    /*
    
    공격력 -> float
    연사력 -> float -> Couroutine
    재장전 시간 -> float
    최대 탄창 -> int
    발사 소리 -> audio clip
    재장전 소리 -> audio clip
    시작 탄창 -> int

    */

    public float damage = 25f; // 총의 공격력

    public float timeBetFire = 0.12f; // 연사 속도
    public float reloadTime = 1.8f; // 재장전 시간

    public int MAGCapacity = 30; // 총의 탄창 크기
    public int startAmmoRemain = 100; // 게임 시작 시, 주어지는 탄의 총량

    public AudioClip shot_Clip;
    public AudioClip reload_Clip;
}
