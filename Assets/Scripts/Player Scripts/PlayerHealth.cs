using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Health variables
    [SerializeField] private float maxHealth = 5f;
    private float currentHealth;

    // References
    [SerializeField] HealthBarUI healthBar;

    void Awake()
    {
        if(!healthBar)
        {
            healthBar = FindFirstObjectByType<HealthBarUI>();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = 5f;
        healthBar.SetHealth(currentHealth, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Change the player's health
    public void ChangeHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        //Debug.Log(playerHealth);
        healthBar.SetHealth(currentHealth, maxHealth);

        if(currentHealth <= 0f)
        {
            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
