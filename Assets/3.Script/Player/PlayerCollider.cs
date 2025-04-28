using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Gun gun;
    Item colItem;

    void OnTriggerEnter(Collider col)
    {
        if (!col.gameObject.TryGetComponent(out colItem)) return;

        if (colItem.data.name == "Coin")
        {
            int gainScore = colItem.data.gainScore;
            GameManager.instance.AddScore(gainScore);
            Destroy(col.transform.parent.gameObject);
        }
        else if (colItem.data.name == "Ammo")
        {
            int gainAmmo = colItem.data.gainAmmo;
            gun.ammoRemain += gainAmmo;
            Destroy(col.transform.parent.gameObject);
        }
        else if (colItem.data.name == "Heart")
        {
            float gainHealth = colItem.data.gainHealth;
            playerHealth.Healing(gainHealth);
            Destroy(col.transform.parent.gameObject);
        }
    }
}