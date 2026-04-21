using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerCombat : MonoBehaviour
{
    // References
    [Header("Weapon Setup")]
    [SerializeField] private Transform weaponHolder;

    [Header("Player Animator")]
    [SerializeField] private Animator _animator;

    [Header("Player Movement")]
    [SerializeField] private PlayerMovement playerMov;

    // Runtime References
    private Weapon weaponInRange;
    List<Weapon> weapons = new List<Weapon>();
    int currentIndex = 0;
    private Camera _mainCamera;

    // Internal State
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

    }

    void Update()
    {
        
    }
    
    // InputHandler calls this when "Attack" is pressed
    public void UseWeapon(FirePosition firePos)
    {
        if(weapons.Count == 0) return;

        Weapon currentWeapon = weapons[currentIndex];

        if(!currentWeapon.CanUse()) return;

        // Positions weapon holder object based on where player is shooting
        if(_currentFacing != firePos){
            weaponHolder.localPosition = _weaponHolderDefaultLocalPos;
            currentWeapon.OnAim(firePos, weaponHolder);
            _currentFacing = firePos;
        }

        // Uses current weapon
        currentWeapon.Use(firePos);

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
        Weapon weapon = other.GetComponent<Weapon>();
        if(weapon != null)
        {
            weaponInRange = weapon;
        }
    }

    // Lets Player object know object is no longer in range
    private void OnTriggerExit2D(Collider2D other)
    {
        Weapon weapon = other.GetComponent<Weapon>();
        if(weapon != null && weaponInRange == weapon)
        {
            weaponInRange = null;
        }
    }

    // Try to equip the weapon
    public void EquipWeapon(Weapon weapon)
    {
        if(weapon == null) return;

        if(weapons.Contains(weapon)) return;

        weapons.Add(weapon); // Add weapon to list and update parent
        weapon.transform.SetParent(weaponHolder);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;

        weapon.GetComponent<CircleCollider2D>().enabled = false;

        weaponInRange = null;

        if(weapons.Count == 1)
        {
            currentIndex = 0;
            weapon.gameObject.SetActive(true);
        }
        else{
            weapon.gameObject.SetActive(false);
        }
    }

    public void CycleWeapon()
    {
        if (weapons.Count <= 1) return;

        weapons[currentIndex].gameObject.SetActive(false);

        currentIndex++;
        if (currentIndex >= weapons.Count)
            currentIndex = 0;

        weapons[currentIndex].gameObject.SetActive(true);
    }

    // Drop the weapon
    public void DropWeapon()
    {
        if (weapons.Count == 0) return;

        Weapon currentWeapon = weapons[currentIndex];

        weapons.RemoveAt(currentIndex);

        currentWeapon.transform.SetParent(null);
        currentWeapon.transform.position = transform.position + transform.right * 0.5f;

        currentWeapon.GetComponent<CircleCollider2D>().enabled = true;

        if (weapons.Count == 0)
        {
            currentIndex = 0;
            return;
        }

        if (currentIndex >= weapons.Count)
            currentIndex = 0;

        weapons[currentIndex].gameObject.SetActive(true);
    }

    public Weapon GetCurrentWeapon()
    {
        if (weapons.Count == 0) return null;
        return weapons[currentIndex];
    }
}
