using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    // Health variables
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;

    // References 
    [SerializeField] private Sprite emptyHeart;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Image[] hearts;

    [SerializeField] private PlayerHealth playerHealth;

    void Awake()
    {
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!playerHealth)
        {
            playerHealth = FindFirstObjectByType<PlayerHealth>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = playerHealth.GetPlayerCurrentHealth();
        maxHealth = playerHealth.GetPlayerMaxHealth();
        for(int i =0; i < hearts.Length; i++)
        {
            if(i < currentHealth)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
            if(i < maxHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
