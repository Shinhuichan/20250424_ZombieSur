using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/ItemData", fileName = "ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public float gainHealth;
    public int gainScore;
    public int gainAmmo;
    public GameObject prefab;
}
