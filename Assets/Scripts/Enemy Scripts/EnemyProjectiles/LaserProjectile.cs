using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    [Header("Projectile Variables")]
    [SerializeField] private float force = 12f;
    [SerializeField] private float damage = 1f;
    private Transform player;
    private Rigidbody2D rb;

    void Awake()
    {
        if(!rb)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!player)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        Vector3 direction = player.position - transform.position;
        rb.linearVelocity = new Vector2(direction.x, direction.y).normalized * force;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();

        if(playerHealth)
        {
            playerHealth.ChangeHealth(-damage);
            Destroy(gameObject);
        }
    }
}
