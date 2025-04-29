using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] List<GameObject> itemList = new List<GameObject>();
    public float spawnRange = 5f;
    public float spawnInterval = 5f;
    private void Start()
    {
        if (!transform.parent.gameObject.TryGetComponent(out playerHealth))
        {
            Debug.LogWarning($"ItemSpawner ] PlayerHealth 없음");
            return;
        }
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (playerHealth.isLive)
        {
            Vector2 spawnPoint = new Vector2(transform.position.x, transform.position.z) + spawnRange * Random.insideUnitCircle;
            GameObject spawnItem = itemList[Random.Range(0, itemList.Count)];
            Vector3 spawnPos = new Vector3(spawnPoint.x, transform.position.y, spawnPoint.y);
            
            if(Physics.Raycast(spawnPos, Vector3.down))
                Instantiate(spawnItem, spawnPos, Quaternion.identity);
            else 
                continue;
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}