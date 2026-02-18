using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    // References
    [Header("Projectile")]
    [SerializeField] private GameObject _bulletPrefab;

    [Header("Weapon Setup")]
    [SerializeField] private Transform weaponHolder;

    [Header("Firing Settings")]
    [SerializeField] private float _timeBetweenFiring = 0.15f;

    [Header("Player Animator")]
    [SerializeField] private Animator _animator;

    [Header("Player Movement")]
    [SerializeField] private PlayerMovement playerMov;

    // Runtime References
    private Gun weaponInRange;
    private Gun currentWeapon;
    private Camera _mainCamera;

    // Internal State
    private float _firetimer;
    private FirePosition _currentFacing = FirePosition.None;
    private Vector3 _weaponHolderDefaultLocalPos;

    // Flags
    private bool isEquipped = false;

    // Enum for firing position
    public enum FirePosition{
        None,
        Up,
        Down,
        Left,
        Right
    };

    void Awake()
    {
        if(_mainCamera == null)
        {
            _mainCamera = Camera.main;
        }
        if(weaponHolder == null){
            weaponHolder = transform.Find("WeaponHolder");
            _weaponHolderDefaultLocalPos = weaponHolder.localPosition;
        }
        if(!_animator){
            _animator = GetComponent<Animator>();
        }
        if(!playerMov)
        {
            playerMov = GetComponent<PlayerMovement>();
        }
    }

    void Start(){
        if(weaponInRange == null){
            weaponInRange = FindFirstObjectByType<Gun>();
        }
    }

    void Update()
    {
        if(_firetimer > 0f)
        {
            _firetimer -= Time.deltaTime;
        } 

        if(currentWeapon)
        {
            isEquipped = true;
        }
        else
        {
            isEquipped = false;
        }
    }
    
    // InputHandler calls this when Fire is pressed
    public void Fire(FirePosition firePos)
    {
        if(_firetimer > 0f) return;

        if(_bulletPrefab == null || currentWeapon == null) return;

        Transform weaponHolder = transform.Find("WeaponHolder");

        // Positions weapon holder object based on where player is shooting
        if(_currentFacing != firePos){
            weaponHolder.localPosition = _weaponHolderDefaultLocalPos;
            if(firePos == FirePosition.Left){
                weaponHolder.localPosition += new Vector3(-0.25f, 0f, 0f);
            }
            if(firePos == FirePosition.Right){
                weaponHolder.localPosition += new Vector3(0.25f, 0f, 0f);
            }
            if(firePos == FirePosition.Up){
                weaponHolder.localPosition += new Vector3(0f, 0.5f, 0f);
            }
            if(firePos == FirePosition.Down){
                weaponHolder.localPosition += new Vector3(0f, -0.5f, 0f);
            }

            _currentFacing = firePos;
        }

        // Spawns bullets and sets timer so you cannot fire too fast
        currentWeapon.Fire(_bulletPrefab, firePos);

        _firetimer = _timeBetweenFiring;

        Vector2 faceDir = Vector2.zero;

        switch(firePos){
            case FirePosition.Left:
                faceDir = Vector2.left;
                break;
            case FirePosition.Right:
                faceDir = Vector2.right;
                break;
            case FirePosition.Up:
                faceDir = Vector2.up;
                break;
            case FirePosition.Down:
                faceDir = Vector2.down;
                break;
        }

        playerMov.SetFacingDirection(faceDir);
    }

    // Check if weapon is in range
    private void OnTriggerEnter2D(Collider2D other)
    {
        Gun gun = other.GetComponent<Gun>();
        if(gun != null)
        {
            weaponInRange = gun;
        }
    }

    // Lets Player object know object is no longer in range
    private void OnTriggerExit2D(Collider2D other)
    {
        Gun gun = other.GetComponent<Gun>();
        if(gun != null && weaponInRange == gun)
        {
            weaponInRange = null;
        }
    }

    // Try to equip the weapon
    public void EquipWeapon()
    {
        if(weaponInRange == null) return;

        Transform weaponHolder = transform.Find("WeaponHolder");
        if(weaponHolder == null) return;

        if(weaponHolder.childCount > 0)
        {
            return;
        }

        currentWeapon = weaponInRange;

        weaponInRange.transform.SetParent(weaponHolder);
        weaponInRange.transform.localPosition = Vector3.zero;
        weaponInRange.transform.localRotation = Quaternion.identity;

        weaponInRange.GetComponent<Collider2D>().enabled = false;

        weaponInRange = null;
    }

    // Drop the weapon
    public void DropWeapon()
    {
        if(currentWeapon == null) return;

        currentWeapon.transform.SetParent(null);
        currentWeapon.transform.position = transform.position + transform.right * 0.5f;

        currentWeapon.GetComponent<Collider2D>().enabled = true;

        currentWeapon = null;
    }

    // Return current fire position
    public FirePosition GetCurrentFirePosition()
    {
        return _currentFacing;
    }

    // Set current fire position
    public void SetCurrentFirePosition(FirePosition firePos)
    {
        _currentFacing = firePos;
    }

    public bool GetIsEquippedFlag()
    {
        return isEquipped;
    }
}
