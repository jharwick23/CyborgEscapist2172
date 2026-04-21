using UnityEngine;

public class Grenade : Weapon
{
    // References
    [Header("Grenade")]
    [SerializeField] private GameObject _grenadePrefab;

    [Header("Weapon Setup")]
    [SerializeField] private Transform firePoint;

    [Header("Grenade Sprite Renderer")]
    [SerializeField] private SpriteRenderer grenadeSr;


    // Internal Variables
    private Vector3 _firePointDefaultLocalPos;
    private float lastThrowTimer;
    private float lastThrowDuration = 0.1f;

    void Awake()
    {
        if(firePoint == null){
            firePoint = transform.Find("FirePointGrenade");
            _firePointDefaultLocalPos = firePoint.localPosition;
        }
        if(!grenadeSr){
            grenadeSr = GetComponent<SpriteRenderer>();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if(!transform.parent)
        {
            grenadeSr.enabled = true;
        }
        else
        {
            CheckLastThrow();
        }
    }

    // Sets aiming position
    public override void OnAim(PlayerCombat.FirePosition firePos, Transform weaponHolder)
    {
        weaponHolder.localPosition = Vector3.zero;

        switch(firePos)
        {
            case PlayerCombat.FirePosition.Left:
                weaponHolder.localPosition += new Vector3(-0.25f, 0f, 0f);
                break;
            case PlayerCombat.FirePosition.Right:
                weaponHolder.localPosition += new Vector3(0.25f, 0f, 0f);
                break;
            case PlayerCombat.FirePosition.Up:
                weaponHolder.localPosition += new Vector3(0f, 0.5f, 0f);
                break;
            case PlayerCombat.FirePosition.Down:
                weaponHolder.localPosition += new Vector3(0f, -0.5f, 0f);
                break;
        }
    }

    // Throw Grenade
    public override void Use(PlayerCombat.FirePosition firePos)
    {
        Debug.Log("Throw Grenade");
        
        // Checks for references
        if(_grenadePrefab == null || firePoint == null) return;

        firePoint.localRotation = Quaternion.identity;
        firePoint.localPosition = _firePointDefaultLocalPos;

        // Sets sprite based on which direction player is aiming
        // Rotates fire point so player fires in correct direction (Right is zero by default so no need to rotate)
        // Changes fire point pos relative to the direction player is facing
        switch(firePos){
            case PlayerCombat.FirePosition.Up:
                firePoint.localRotation = Quaternion.Euler(0f, 0f, 90f);
                firePoint.localPosition += new Vector3(0f, 0.2f, 0f);
                break;
            case PlayerCombat.FirePosition.Down:
                firePoint.localRotation = Quaternion.Euler(0f, 0f, -90f);
                firePoint.localPosition += new Vector3(0f, -0.2f, 0f);
                break;
            case PlayerCombat.FirePosition.Left:
                firePoint.localRotation = Quaternion.Euler(0f, 0f, 180f);
                firePoint.localPosition += new Vector3(-0.2f, 0f, 0f);
                break;
            case PlayerCombat.FirePosition.Right:
                firePoint.localPosition += new Vector3(0.2f, 0f, 0f);
                break;
        }

        Vector2 dir = firePoint.right;

        GameObject grenadeObj = Instantiate(_grenadePrefab, firePoint.position, Quaternion.identity);
        
        GrenadeThrow grenade = grenadeObj.GetComponent<GrenadeThrow>();
        if(grenade != null)
        {
            Debug.Log("Grenade being thrown");
            grenade.ThrowGrenade(dir);
        }

        SetLastThrow();
        ResetUseTimer();
    }

    // Resets the duration of the last throw
    void SetLastThrow()
    {
        lastThrowTimer = lastThrowDuration;
    }

    // Checks last throw 
    // If grenade has not been thrown in allocated time
    // Do not render the sprite
    void CheckLastThrow()
    {
        if(lastThrowTimer > 0)
        {
            lastThrowTimer -= Time.deltaTime;
            grenadeSr.enabled = true;
        }
        else
        {
            grenadeSr.enabled = false;
        }
    }
}
