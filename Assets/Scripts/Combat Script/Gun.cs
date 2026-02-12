using UnityEngine;

public class Gun : MonoBehaviour
{
    // References
    [Header("Weapon Setup")]
    [SerializeField] private Transform firePoint;

    // Runtime References
    private SpriteRenderer weaponSprite;

    void Awake(){
        if(weaponSprite == null){
            weaponSprite = GetComponentInChildren<SpriteRenderer>();
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
    public void Fire(GameObject bulletPrefab)
    {
        if(bulletPrefab == null || firePoint == null) return;

        Vector2 dir = firePoint.right;

        // spawn bullet
        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        BulletScript bullet = bulletObj.GetComponent<BulletScript>();
        if(bullet != null){
            bullet.FireBullet(dir);
        }
    }
}
