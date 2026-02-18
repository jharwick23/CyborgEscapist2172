using UnityEngine;

public class Gun : MonoBehaviour
{
    // References
    [Header("Weapon Setup")]
    [SerializeField] private Transform firePoint;

    [Header("Gun Animator")]
    [SerializeField] private Animator _gunanimator;

    [Header("Gun Sprite Renderer")]
    [SerializeField] private SpriteRenderer gunSr;

    [Header("UI Handler")] 
    [SerializeField] private WeaponUI weaponUi; 

    // Internal Variables
    private Vector3 _firePointDefaultLocalPos;
    private float lastShotTimer;
    private float lastShotDuration = 1f;

    // Ammo Variables
    [SerializeField] private float currentAmmo;
    [SerializeField] private float maxAmmo = 24f;

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
        if(!weaponUi)
        {
            weaponUi = GameObject.FindFirstObjectByType<WeaponUI>();
        }
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if(!transform.parent)
        {
            gunSr.enabled = true;
        }
        else
        {
            CheckLastShot();
        }

        if(currentAmmo <= 0)
        {
            gunSr.enabled = false;
        }
    }

    public Transform GetFirePoint(){
        return firePoint;
    }

    // Fire weapon
    public void Fire(GameObject bulletPrefab, PlayerCombat.FirePosition firePos)
    {
        // Checks for references
        if(bulletPrefab == null || firePoint == null) return;

        // Returns if ammo hits zero
        if(currentAmmo <= 0) return;

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

        // spawn bullet
        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        BulletScript bullet = bulletObj.GetComponent<BulletScript>();
        if(bullet != null){
            bullet.FireBullet(dir);
        }

        SetLastFire();
        currentAmmo--;
        
        if(weaponUi)
        {
            weaponUi.UpdateAmmo(currentAmmo);
        }
        Debug.Log(currentAmmo);
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
}
