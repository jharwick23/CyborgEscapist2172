using UnityEngine;

public class RepairKit : MonoBehaviour
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
        // Do not pick up repair kit if health is full
        if(other.GetComponent<PlayerHealth>().GetPlayerCurrentHealth() == other.GetComponent<PlayerHealth>().GetPlayerMaxHealth()) return;

        other.GetComponent<PlayerHealth>().ChangeHealth(1f);
        Destroy(gameObject);
    }
}
