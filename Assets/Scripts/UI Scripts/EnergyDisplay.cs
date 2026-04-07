using UnityEngine;
using UnityEngine.UI;

public class EnergyDisplay : MonoBehaviour
{
    // UI Variables
    [SerializeField] private Image energyBar;

    // Energy Variables
    [SerializeField] private float currentEnergy;
    [SerializeField] private float maxEnergy;

    // References
    [SerializeField] private PlayerEnergy playerEnergy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!playerEnergy)
        {
            playerEnergy = FindFirstObjectByType<PlayerEnergy>();
        }
    }

    // Update is called once per frame
    // Updates the players energy bar
    void Update()
    {
        currentEnergy = playerEnergy.GetCurrentEnergy();
        maxEnergy = playerEnergy.GetMaxEnergy();
        if(currentEnergy < 0) currentEnergy = 0;
        energyBar.fillAmount = currentEnergy / maxEnergy;
    }
}
