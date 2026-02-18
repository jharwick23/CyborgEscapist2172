using UnityEngine;

public class AlienAI : MonoBehaviour
{
    // Variables for Alien
    [Header("Alien Variables")]
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float detectionRange = 2f;
    [SerializeField] private float attackCooldown = 1f;
    private float attackTimer;
    private Vector2 moveDirection;
    private enum State {Idle, Attacking}
    private State currentState = State.Idle;

    // References
    private Rigidbody2D rb;
    private Transform player;
    private Animator animator;

    void Awake()
    {
        if(!rb){
            rb = GetComponent<Rigidbody2D>();
        }
        if(!animator)
        {
            animator = GetComponent<Animator>();
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
            //Debug.Log("Visible");
        }
        else
        {
            currentState = State.Idle;
            //Debug.Log("Not Visible");
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

    void HandleIdle()
    {
        moveDirection = Vector2.zero;
        animator.SetBool("IsAttacking", false);
    }

    void HandleAttacking()
    {
        moveDirection = (player.position - transform.position).normalized;
        animator.SetBool("IsAttacking", true);
    }

    bool PlayerVisible()
    {
        if(player == null) return false;

        float distance = Vector2.Distance(transform.position, player.position);
        //Debug.Log(distance);
        return distance <= detectionRange;
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
