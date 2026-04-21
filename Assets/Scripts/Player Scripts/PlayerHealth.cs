using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    // Health variables
    [SerializeField] private float maxHealth = 5f;
    private float currentHealth;

    // References
    [SerializeField] private PlayerMovement playerMov;
    [SerializeField] private SpriteRenderer playerSr;
    [SerializeField] private Deathmenu deathMenu;
    [SerializeField] private ShieldAbility shieldAbility;

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
        if(!deathMenu)
        {
            deathMenu = FindFirstObjectByType<Deathmenu>();
        }
        if(!shieldAbility)
        {
            shieldAbility = FindFirstObjectByType<ShieldAbility>();
        }
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Change the player's health
    public void ChangeHealth(float amount)
    {
        if(shieldAbility.GetIsShielded()) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if(currentHealth <= 0f)
        {
            playerMov.enabled = false;
            playerSr.enabled = false;

            deathMenu.EnableDeathMenu();
        }
    }

    public void Spawn()
    {
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("Spawnpoint");

        if(spawnPoint != null)
        {
            transform.position = spawnPoint.transform.position;

            if(playerMov.enabled == false)
            {
                playerMov.enabled = true;
            }

            if(playerSr.enabled == false)
            {
                playerSr.enabled = true;
            }
        }

        currentHealth = maxHealth;
    }

    public float GetPlayerCurrentHealth(){
        return currentHealth;
    }

    public float GetPlayerMaxHealth(){
        return maxHealth;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject spawn = GameObject.FindWithTag("Spawnpoint");

        if (spawn != null)
        {
            transform.position = spawn.transform.position;
        }
        else
        {
            Debug.LogWarning("No spawn point found in scene!");
        }
    }
}
