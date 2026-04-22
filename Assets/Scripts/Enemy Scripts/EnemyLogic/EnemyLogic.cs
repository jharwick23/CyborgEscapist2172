using UnityEngine;
using System.Collections;

public class EnemyLogic : MonoBehaviour
{
    // Enemy variables
    [SerializeField] private float currentEnemyHealth;
    [SerializeField] private float maxEnemyHealth;
    
    // Default Enemy color
    private Color defaultColor;

    // References
    [SerializeField] private SpriteRenderer enemySr;
    [SerializeField] private DroppableScript dropService;
    
    // Only used in first level
    // TODO: Possible cleaner way to do this will look into more later
    [SerializeField] private EnemyDoorScript enemydoor;

    // Flags
    private bool isDead = false;

    protected virtual void Awake()
    {
        if(!enemySr)
        {
            enemySr = GetComponent<SpriteRenderer>();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        if(!dropService)
        {
            dropService = FindFirstObjectByType<DroppableScript>();
        }

        currentEnemyHealth = maxEnemyHealth;
        defaultColor = enemySr.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoDamage(float dmg)
    {
        if(isDead) return;

        currentEnemyHealth -= dmg;
        StartCoroutine(FlashRed());
        
        if(currentEnemyHealth <= 0)
        {
            DoDeath();
        }
    }

    protected virtual void DoDeath()
    {
        if (isDead) return;
        isDead = true;

        // TODO: make this more scalable
        // or maybe it is already?
        // Enemy object is removed from list on the enemy door script
        if(enemydoor != null)
        {
            enemydoor.RemoveEnemy(this);
        }

        Vector2 position = transform.position;
        Destroy(gameObject);
        dropService.Droppables(position);
    }

    private IEnumerator FlashRed()
    {
        enemySr.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        enemySr.color = defaultColor;
    }

    public float GetCurrentHealth()
    {
        return currentEnemyHealth;
    }
}
