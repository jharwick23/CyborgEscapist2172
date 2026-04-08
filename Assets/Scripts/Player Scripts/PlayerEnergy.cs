using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    // Energy variables
    [SerializeField] private float currentEnergy;
    [SerializeField] private float maxEnergy = 100f;

    // References
    [SerializeField] private ShieldAbility shieldAbility;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!shieldAbility)
        {
            shieldAbility = FindFirstObjectByType<ShieldAbility>();
        }
        currentEnergy = maxEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        if(shieldAbility.GetIsShielded())
        {
            DecreaseEnergy(0.25f);
        }

        if(currentEnergy == 0f)
        {
            shieldAbility.ToggleShield();
        }
    }

    // Decreases player energy if shielded was performed;
    void DecreaseEnergy(float amount)
    {
        currentEnergy -= amount;
    }

    // Increases player energy 
    public void IncreaseEnergy(float amount)
    {
        currentEnergy += amount;
    }

    public float GetCurrentEnergy()
    {
        return currentEnergy;
    }

    public float GetMaxEnergy()
    {
        return maxEnergy;
    }
}
