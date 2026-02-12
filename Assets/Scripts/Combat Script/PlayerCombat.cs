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

    // Runtime References
    private Gun weaponInRange;
    private Gun currentWeapon;
    private Camera _mainCamera;

    // Internal State
    private float _firetimer;
    private FirePosition _currentFacing = FirePosition.None;
    private Vector3 _weaponHolderDefaultLocalPos;

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
        if(weaponInRange == null){
            weaponInRange = FindFirstObjectByType<Gun>();
        }
        if(weaponHolder == null){
            weaponHolder = transform.Find("WeaponHolder");
            _weaponHolderDefaultLocalPos = weaponHolder.localPosition;
        }
        if(!_animator){
            _animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        if(_firetimer > 0f)
        {
            _firetimer -= Time.deltaTime;
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
                weaponHolder.localPosition += new Vector3(0f, 0.25f, 0f);
            }
            if(firePos == FirePosition.Down){
                weaponHolder.localPosition += new Vector3(0f, -0.25f, 0f);
            }

            _currentFacing = firePos;
        }

        // Spawns bullets and sets timer so you cannot fire too fast
        currentWeapon.Fire(_bulletPrefab, firePos);

        _firetimer = _timeBetweenFiring;
    }

    // Check if weapon is in range
    private void OnTriggerEnter2D(Collider2D other)
    {
        Gun gun = other.GetComponent<Gun>();
        if(gun != null)
        {
            weaponInRange = gun;
            Debug.Log("Weapon in range");
        }
    }

    // Lets Player object know object is no longer in range
    private void OnTriggerExit2D(Collider2D other)
    {
        Gun gun = other.GetComponent<Gun>();
        if(gun != null && weaponInRange == gun)
        {
            weaponInRange = null;
            Debug.Log("Weapon left range");
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
            Debug.Log("Already holding weapon");
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
}
