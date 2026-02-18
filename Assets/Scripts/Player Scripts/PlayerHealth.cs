using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Health variables
    [SerializeField] private float maxHealth = 5f;
    private float currentHealth;

    // References
    [SerializeField] private PlayerMovement playerMov;
    [SerializeField] private SpriteRenderer playerSr;

    void Awake()
    {
        if(!playerMov)
        {
            playerMov = GetComponent<PlayerMovement>();
        }
        if(!playerSr)
        {
            playerSr = GetComponent<SpriteRenderer>();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = 5f;
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

        if(currentHealth <= 0f)
        {
            playerMov.enabled = false;
            playerSr.enabled = false;
        }
    }

    public float GetPlayerCurrentHealth(){
        return currentHealth;
    }

    public float GetPlayerMaxHealth(){
        return maxHealth;
    }
}
