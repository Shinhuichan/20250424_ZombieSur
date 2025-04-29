using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    [SerializeField] Gun gun;
    [SerializeField] PlayerHealth playerHealth;
    void OnTriggerEnter(Collider col)
    {
        Item colItem = col.gameObject.GetComponent<Item>();
        if (colItem == null) return;

        if (colItem.data.name == "Coin")
        {
            int gainScore = colItem.data.gainScore;
            GameManager.instance.SetScore(gainScore);
            Destroy(col.transform.parent.gameObject);
        }
        else if (colItem.data.name == "Ammo")
        {
            if (gun == null) return;

            int gainAmmo = colItem.data.gainAmmo;
            gun.ammoRemain += gainAmmo;
            Destroy(col.transform.parent.gameObject);
        }
        else if (colItem.data.name == "Heart")
        {
            if (playerHealth == null) return;

            float gainHealth = colItem.data.gainHealth;
            playerHealth.Healing(gainHealth);
            Destroy(col.transform.parent.gameObject);
        }
    }
}