using UnityEngine;

public class Gun : MonoBehaviour
{
    // References
    [Header("Weapon Setup")]
    [SerializeField] private Transform firePoint;

    [Header("Gun Animator")]
    [SerializeField] private Animator _gunanimator;

    // Internal Variables
    private Vector3 _firePointDefaultLocalPos;

    void Awake(){
        if(!_gunanimator){
            _gunanimator = GetComponent<Animator>();
        }
        if(firePoint == null){
            firePoint = transform.Find("FirePoint");
            _firePointDefaultLocalPos = firePoint.localPosition;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform GetFirePoint(){
        return firePoint;
    }

    // Fire weapon
    public void Fire(GameObject bulletPrefab, PlayerCombat.FirePosition firePos)
    {
        if(bulletPrefab == null || firePoint == null) return;

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
    }
}
