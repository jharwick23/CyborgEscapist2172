using UnityEngine;

public class ShieldAbility : MonoBehaviour
{
    // References
    [SerializeField] private PlayerEnergy playerEnergy;
    [SerializeField] private SpriteRenderer playerSr;

    // Default color for player
    private Color defaultColor;

    // Flags to check if shielded
    private bool isShielded = false;

    void Awake()
    {
        if(!playerSr)
        {
            playerSr = GetComponent<SpriteRenderer>();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!playerEnergy)
        {
            playerEnergy = FindFirstObjectByType<PlayerEnergy>();
        }

        defaultColor = playerSr.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool GetIsShielded()
    {
        return isShielded;
    }

    public void ToggleShield()
    {
        if(playerEnergy.GetCurrentEnergy() <= 0 && !isShielded) return;

        isShielded = !isShielded;

        if(isShielded)
        {
            playerSr.color = Color.yellow;
        }
        else
        {
            playerSr.color = defaultColor;
        }
    }
}
