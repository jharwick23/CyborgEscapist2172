using UnityEngine;

public class EnergyCell : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Do not increase energy if full
        if(other.GetComponent<PlayerEnergy>().GetCurrentEnergy() == other.GetComponent<PlayerEnergy>().GetMaxEnergy()) return;

        other.GetComponent<PlayerEnergy>().IncreaseEnergy(25f);
        Destroy(gameObject);
    }
}
