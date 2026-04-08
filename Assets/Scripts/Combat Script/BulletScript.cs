using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletScript : MonoBehaviour
{
    [SerializeField] private float _speed = 15f;
    [SerializeField] private float _lifetime = 3f;

    private Rigidbody2D _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {

    }

    // shoots bullet out of gun direction facing wherever gun is facing
    public void FireBullet(Vector2 direction)
    {
        direction = direction.normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
        
        _rb.linearVelocity = direction * _speed;
        Destroy(gameObject, _lifetime);
    }

    // Enemy hit with bullet
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Droppables")){
            return;
        }
        if(other.gameObject.CompareTag("Enemy")){
            other.gameObject.GetComponent<EnemyLogic>().DoDamage(1f);
        }

        Destroy(gameObject);
    }
}
