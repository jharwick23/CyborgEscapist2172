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

    void Awake()
    {
        if(!enemySr)
        {
            enemySr = GetComponent<SpriteRenderer>();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
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
        currentEnemyHealth -= dmg;
        StartCoroutine(FlashRed());
        
        if(currentEnemyHealth <= 0)
        {
            DoDeath();
        }
    }

    void DoDeath()
    {
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
}
