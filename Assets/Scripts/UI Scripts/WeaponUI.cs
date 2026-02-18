using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    // References
    [SerializeField] private Image weapon;
    [SerializeField] private PlayerCombat playerCombat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!playerCombat)
        {
            playerCombat = GameObject.FindFirstObjectByType<PlayerCombat>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(playerCombat.GetIsEquippedFlag())
        {
            weapon.enabled = true;
        }
        else
        {
            weapon.enabled = false;
        }
    }
}
