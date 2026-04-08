using UnityEngine;

public class DroppableScript : MonoBehaviour
{
    // Droppables variables
    [SerializeField] private GameObject repairKit;
    [SerializeField] private GameObject energyCell;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Droppables(Vector2 position)
    {
        Debug.Log(position);
        float result = Random.Range(0, 2);

        if(result == 0 && repairKit != null)
        {
            Instantiate(repairKit, position, Quaternion.identity);
        }
        else if(result == 1 && energyCell != null)
        {
            Instantiate(energyCell, position, Quaternion.identity);
        }
    }
}
