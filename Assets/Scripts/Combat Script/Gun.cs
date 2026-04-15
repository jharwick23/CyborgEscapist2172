using UnityEngine;
using System.Collections;

public class Gun : Weapon
{
    // References
    [Header("Bullet")]
    [SerializeField] private GameObject _bulletPrefab;

    [Header("Weapon Setup")]
    [SerializeField] private Transform firePoint;

    [Header("Gun Animator")]
    [SerializeField] private Animator _gunanimator;

    [Header("Gun Sprite Renderer")]
    [SerializeField] private SpriteRenderer gunSr;

    // Internal Variables
    private Vector3 _firePointDefaultLocalPos;
    private float lastShotTimer;
    private float lastShotDuration = 1f;

    // Ammo Variables
    [SerializeField] private float currentAmmo;
    [SerializeField] private float maxAmmo = 24f;

    // Flags
    private bool isReloading = false;

    void Awake(){
        if(!_gunanimator){
            _gunanimator = GetComponent<Animator>();
        }
        if(firePoint == null){
            firePoint = transform.Find("FirePoint");
            _firePointDefaultLocalPos = firePoint.localPosition;
        }
        if(!gunSr){
            gunSr = GetComponentInChildren<SpriteRenderer>();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if(!transform.parent)
        {
            gunSr.enabled = true;
        }
        else
        {
            CheckLastShot();
        }
    }

    public Transform GetFirePoint(){
        return firePoint;
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

    // Fire weapon
    public override void Use(PlayerCombat.FirePosition firePos)
    {
        // Checks for references
        if(_bulletPrefab == null || firePoint == null) return;

        firePoint.localRotation = Quaternion.identity;
        firePoint.localPosition = _firePointDefaultLocalPos;

        // Sets sprite based on which direction player is shooting
        // Rotates fire point so player fires in correct direction (Right is zero by default so no need to rotate)
        // Changes fire point pos relative to the direction player is facing
        switch(firePos){
            case PlayerCombat.FirePosition.Up:
                _gunanimator.SetFloat("DirectionX", 0);
                _gunanimator.SetFloat("DirectionY", 1);
                firePoint.localRotation = Quaternion.Euler(0f, 0f, 90f);
                firePoint.localPosition += new Vector3(0f, 0.2f, 0f);
                break;
            case PlayerCombat.FirePosition.Down:
                _gunanimator.SetFloat("DirectionX", 0);
                _gunanimator.SetFloat("DirectionY", -1);
                firePoint.localRotation = Quaternion.Euler(0f, 0f, -90f);
                firePoint.localPosition += new Vector3(0f, -0.2f, 0f);
                break;
            case PlayerCombat.FirePosition.Left:
                _gunanimator.SetFloat("DirectionX", -1);
                _gunanimator.SetFloat("DirectionY", 0);
                firePoint.localRotation = Quaternion.Euler(0f, 0f, 180f);
                firePoint.localPosition += new Vector3(-0.2f, 0f, 0f);
                break;
            case PlayerCombat.FirePosition.Right:
                _gunanimator.SetFloat("DirectionX", 1);
                _gunanimator.SetFloat("DirectionY", 0);
                firePoint.localPosition += new Vector3(0.2f, 0f, 0f);
                break;
        }

        Vector2 dir = firePoint.right;

        // Returns if ammo hits zero
        // Or if reloading
        // Sets last fire to make sure the gun still appears but it doesn't 
        // Actually shoot bullets
        if(currentAmmo <= 0 || isReloading)
        {
            SetLastFire();
            return;
        }

        // spawn bullet
        GameObject bulletObj = Instantiate(_bulletPrefab, firePoint.position, Quaternion.identity);

        BulletScript bullet = bulletObj.GetComponent<BulletScript>();
        if(bullet != null){
            bullet.FireBullet(dir);
        }

        SetLastFire();
        currentAmmo--;

        ResetUseTimer();
    }

    // Resets the duration of the last shot fired
    void SetLastFire()
    {
        lastShotTimer = lastShotDuration;
    }

    // Checks last shot 
    // If gun has not been shot in allocated time
    // Do not render the sprite
    void CheckLastShot()
    {
        if(lastShotTimer > 0)
        {
            lastShotTimer -= Time.deltaTime;
            gunSr.enabled = true;
        }
        else
        {
            gunSr.enabled = false;
        }
    }

    public void Reload()
    {
        if(!isReloading && currentAmmo != maxAmmo)
        {
            StartCoroutine(ReloadRoutine());
        }
        else
        {
            return;
        }
    }

    IEnumerator ReloadRoutine()
    {
        isReloading = true;

        yield return new WaitForSeconds(1.5f);

        currentAmmo = maxAmmo;

        isReloading = false;
    }

    public override bool ShouldShowAmmo()
    {
        return true;
    }

    public override string GetAmmoText()
    {
        return currentAmmo + " / ∞";
    }

    public override bool IsReloading()
    {
        return isReloading;
    }
}
