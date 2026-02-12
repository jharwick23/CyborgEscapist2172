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

    // Runtime References
    private Gun weaponInRange;
    private Gun currentWeapon;
    private Camera _mainCamera;

    // Internal State
    private float _firetimer;
    private Vector2 aimWorldPos;


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
    public void Fire()
    {
        if(_firetimer > 0f) return;

        if(_bulletPrefab == null || currentWeapon == null) return;

        // spawn bullet 
        currentWeapon.Fire(_bulletPrefab);

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
}
