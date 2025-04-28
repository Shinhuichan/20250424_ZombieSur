using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/ZombieData", fileName = "ZombieData")]
public class ZombieData : ScriptableObject
{
    /*
        체력
        이동속도
        공격력
        피부색
    */
    public float health = 100f;
    public float damage = 20f;
    public float moveSpeed = 2f;
    public Color skinColor = Color.white;
}