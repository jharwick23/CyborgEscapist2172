using UnityEngine;

public class AlienUFO : MonoBehaviour
{
    // Variables for Alien
    [Header("Alien Variables")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float fireRate = 1.5f;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private float stoppingDistance = 4f;
    private Vector2 moveDirection;
    private enum State {Idle, Attacking}
    private State currentState = State.Idle;
    private float fireCooldown;

    // References
    [SerializeField] private GameObject laserProjectile;
    private Rigidbody2D rb;
    private Transform player;

    void Awake()
    {
        if(!rb){
            rb = GetComponent<Rigidbody2D>();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!player){
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerVisible())
        {
            currentState = State.Attacking;
        }
        else
        {
            currentState = State.Idle;
        }
    }

    void FixedUpdate()
    {
        switch(currentState)
        {
            case State.Idle:
                HandleIdle();
                break;
            case State.Attacking:
                HandleAttacking();
                break;
        }
        rb.linearVelocity = moveDirection.normalized * moveSpeed;  
    }

    // Handle the idle state
    void HandleIdle()
    {
        moveDirection = Vector2.zero;
    }

    // Handle the attacking state
    void HandleAttacking()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        
        if(distance <= stoppingDistance)
        {
            moveDirection = Vector2.zero;
        }
        else
        {
            moveDirection = (player.position - transform.position).normalized;
        }

        fireCooldown -= Time.deltaTime;
        if(fireCooldown <= 0)
        {
            Attack();
            fireCooldown = fireRate;
        }
    }

    // Check if player is visible
    bool PlayerVisible()
    {
        if(player == null) return false;

        float distance = Vector2.Distance(transform.position, player.position);
        return distance <= detectionRange;
    }

    // Shoot at player
    void Attack()
    {
        if(!laserProjectile) return;

        Vector2 direction = (player.position - transform.position).normalized;
        GameObject projectile = Instantiate(laserProjectile, transform.position, Quaternion.identity);
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        projectileRb.linearVelocity = direction * projectileSpeed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
