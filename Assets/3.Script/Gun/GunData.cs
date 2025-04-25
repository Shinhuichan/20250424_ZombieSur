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

    public float damage = 25f; // ���ݷ�

    public float timeBetFire = 0.12f; // �����
    public float reloadTime = 1.8f; // ������ �ð�

    public int MAGCapacity = 30;
    public int startAmmoRemain = 100;

    public AudioClip shot_Clip;
    public AudioClip reload_Clip;
}
