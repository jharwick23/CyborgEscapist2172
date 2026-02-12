using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    // Movement Variables
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 _movementInput;
    // Assigned Controller Variables
    private Rigidbody2D _rb;
    private Animator _animator;

    void Awake()
    {
        if(!_rb){
            _rb = GetComponent<Rigidbody2D>();
        }
        if(!_animator){
            _animator = GetComponent<Animator>();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rb.linearVelocity = _movementInput * moveSpeed;
    }

    // Move the character
    public void Move(Vector2 movementVector)
    {
        _movementInput = movementVector;
        _animator.SetFloat("Speed", movementVector.sqrMagnitude);

        // Only update facing direction if moving
        if (movementVector != Vector2.zero)
        {
            _animator.SetFloat("DirectionX", movementVector.x);
            _animator.SetFloat("DirectionY", movementVector.y);
        }
    }
}
