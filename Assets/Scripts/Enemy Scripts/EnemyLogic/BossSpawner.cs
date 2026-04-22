using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    // Boss Reference
    [SerializeField] private GameObject bossPrefab;

    // Spawner
    private GameObject bossSpawn;

    // Flags
    private bool hasSpawned = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bossSpawn = GameObject.FindGameObjectWithTag("BossSpawn");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasSpawned && other.CompareTag("Player"))
        {
            if (bossSpawn && bossPrefab)
            {
                Instantiate(bossPrefab, bossSpawn.transform.position, Quaternion.identity);
                hasSpawned = true;
            }
        }
    }
}
