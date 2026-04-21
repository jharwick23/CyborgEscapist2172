using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class EnemyDoorScript : MonoBehaviour
{
    // References
    [SerializeField] private Tilemap doorTilemap;
    [SerializeField] private List<EnemyLogic> enemies; 

    // Flags for performance purposes
    private bool wasRemoved;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Remove the enemy object from the list
    public void RemoveEnemy(EnemyLogic enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count == 0)
        {
            doorTilemap.gameObject.SetActive(false);
        }
    }
}
