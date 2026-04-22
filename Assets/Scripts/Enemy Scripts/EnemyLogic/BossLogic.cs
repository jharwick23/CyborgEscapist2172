using UnityEngine;
using System.Collections.Generic;

public class BossLogic : EnemyLogic
{
    // Boss state machine
    private enum BossState
    {
        Phase1_Ranged,
        Phase2_Chase
    }

    private BossState currentState;

    // Phase Settings
    [Header("Phase Settings")]
    [SerializeField] private float phase2Threshold = 25f; // HP phase 2 threshold

    // (Phase 1)
    [Header("Ranged Attack")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private float fireRate = 1.5f;
    [SerializeField] private float projectileSpeed = 6f;
    private float fireCooldown;

    // Spawn Settings (Phase 1)
    [SerializeField] private GameObject alienPrefab;
    [SerializeField] private GameObject ufoPrefab;

    // Limit spawn
    private bool hasSpawned = false;
    [SerializeField] private int maxPerSpawner = 2;

    // (Phase 2)
    [Header("Chase Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackCooldown = 1f;
    private float attackTimer;
    // Phase 2 Buffs
    [SerializeField] private float phase2FireRate = 0.5f;       // faster shooting
    [SerializeField] private float phase2ProjectileSpeed = 7.5f; // slightly faster bullets
    [SerializeField] private float phase2MoveSpeed = 4f;         // slightly faster movement

    // References
    private Transform player;
    private Rigidbody2D rb;

    // Spawners
    private GameObject ufoSpawner;
    private GameObject alienSpawner;

    // Drops
    [SerializeField] private GameObject scrapPrefab;

    protected override void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Start()
    {
        base.Start();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Find spawners by tag
        ufoSpawner = GameObject.FindGameObjectWithTag("UFOSpawner");
        alienSpawner = GameObject.FindGameObjectWithTag("AlienSpawner");

        currentState = BossState.Phase1_Ranged;

        fireCooldown = fireRate;

        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        float currentHealth = GetCurrentHealth();

        if (currentHealth <= phase2Threshold && currentState == BossState.Phase1_Ranged)
        {
            EnterPhase2();
        }

        // Run state
        switch (currentState)
        {
            case BossState.Phase1_Ranged:
                HandlePhase1();
                break;

            case BossState.Phase2_Chase:
                HandlePhase2();
                break;
        }
    }

    void FixedUpdate()
    {
        if (currentState == BossState.Phase2_Chase)
        {
            HandleMovement();
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    // Shoot Projectiles 
    // Phase 1
    void HandlePhase1()
    {
        fireCooldown -= Time.deltaTime;

        // Shooting
        if (fireCooldown <= 0f)
        {
            Shoot();
            fireCooldown = fireRate;
        }

        // Spawning
        if (!hasSpawned)
        {
            SpawnEnemies();
            hasSpawned = true;
        }
    }

    // Shooting logic
    void Shoot()
    {
        if (!projectile || player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;

        GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity);

        Rigidbody2D projRb = proj.GetComponent<Rigidbody2D>();
        projRb.linearVelocity = direction * projectileSpeed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        proj.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // Chase and regenerate health
    void HandlePhase2()
    {
        // Still shoot in phase 2
        fireCooldown -= Time.deltaTime;

        if (fireCooldown <= 0f)
        {
            Shoot();
            fireCooldown = fireRate;
        }
    }

    // Handle the bosses movement
    void HandleMovement()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    // Phase Transition
    void EnterPhase2()
    {
        currentState = BossState.Phase2_Chase;

        // Disable spawners
        if (ufoSpawner) ufoSpawner.SetActive(false);
        if (alienSpawner) alienSpawner.SetActive(false);

        fireRate = phase2FireRate;
        projectileSpeed = phase2ProjectileSpeed;
        moveSpeed = phase2MoveSpeed;

        // Reset cooldown so it feels immediate
        fireCooldown = 0f;

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void SpawnEnemies()
    {
        // Spawn 3 aliens
        for (int i = 0; i < maxPerSpawner; i++)
        {
            if (alienSpawner && alienPrefab)
            {
                Instantiate(alienPrefab, alienSpawner.transform.position, Quaternion.identity);
            }
        }

        // Spawn 3 UFOs
        for (int i = 0; i < maxPerSpawner; i++)
        {
            if (ufoSpawner && ufoPrefab)
            {
                Instantiate(ufoPrefab, ufoSpawner.transform.position, Quaternion.identity);
            }
        }
    }

    protected override void DoDeath()
    {
        Vector2 position = transform.position;

        // Drop scrap
        if (scrapPrefab != null)
        {
            Instantiate(scrapPrefab, position, Quaternion.identity);
        }

        base.DoDeath();
    }

    // Player hit 
    private void OnTriggerStay2D(Collider2D other)
    {
        PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();

        attackTimer -= Time.deltaTime;
        
        if(playerHealth && attackTimer <= 0)
        {
            playerHealth.ChangeHealth(-1);
            attackTimer = attackCooldown;
        }
    }
}