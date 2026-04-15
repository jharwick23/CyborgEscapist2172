using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class WeaponUI : MonoBehaviour
{
    // References
    [SerializeField] private Image weapon;
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private TextMeshProUGUI ammoText;

    // Couroutine Variable
    private Coroutine reloadAnim;

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
        Weapon currentWeapon = playerCombat.GetCurrentWeapon();

        if(currentWeapon != null)
        {
            weapon.enabled = true;

            // Set weapon icon
            weapon.sprite = currentWeapon.GetWeaponIcon();

            // Handle ammo visibility
            if(currentWeapon.ShouldShowAmmo())
            {
                ammoText.enabled = true;

                // Handle reloading state
                if(currentWeapon.IsReloading())
                {
                    HandleReloadAnimation();
                }
                else
                {
                    StopReloadAnimation();
                    ammoText.text = currentWeapon.GetAmmoText();
                }
            }
            else
            {
                ammoText.enabled = false;
                StopReloadAnimation();
            }
        }
        else
        {
            weapon.enabled = false;
            ammoText.enabled = false;
            StopReloadAnimation();
        }
    }

    void HandleReloadAnimation()
    {
        if(reloadAnim == null)
        {
            reloadAnim = StartCoroutine(ReloadTextAnimation());
        }
    }

    void StopReloadAnimation()
    {
        if(reloadAnim != null)
        {
            StopCoroutine(reloadAnim);
            reloadAnim = null;
        }
    }

    IEnumerator ReloadTextAnimation()
    {
        string baseText = "RELOADING";
        int dots = 0;

        while(true)
        {
            dots = (dots + 1) % 4;
            ammoText.text = baseText + new string('.', dots);

            yield return new WaitForSeconds(0.1f);
        }
    }
}
