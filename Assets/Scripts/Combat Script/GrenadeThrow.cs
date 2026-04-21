using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GrenadeThrow : MonoBehaviour
{
    // References
    [SerializeField] private GameObject explosionVFX;

    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _lifetime = 0.5f;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private float explosionDamage = 5f;

    private Rigidbody2D _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Throws grenade where player aims
    public void ThrowGrenade(Vector2 direction)
    {
        direction = direction.normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
        
        _rb.linearVelocity = direction * _speed;
        Invoke(nameof(Explode), _lifetime); // Explodes after certain time if nothing hit
    }

    // Enemy hit with grenade
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") || other.CompareTag("Droppables"))
            return;

        if(other.CompareTag("Enemy") || 
        other.CompareTag("Objects-Col") || 
        other.CompareTag("Walls"))
        {
            Explode();
        }
    }

    void Explode()
    {
        // Find all enemies in radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D hit in hits)
        {
            EnemyLogic enemy = hit.GetComponent<EnemyLogic>();
            if (enemy != null)
            {
                enemy.DoDamage(explosionDamage);
            }
        }

        if(explosionVFX != null)
        {
            Instantiate(explosionVFX, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
