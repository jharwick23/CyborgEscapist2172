using UnityEngine;

public class ShieldAbility : MonoBehaviour
{
    // References
    [SerializeField] private PlayerEnergy playerEnergy;

    // Flags to check if shielded
    private bool isShielded = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!playerEnergy)
        {
            playerEnergy = FindFirstObjectByType<PlayerEnergy>();
        }
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
    }
}
