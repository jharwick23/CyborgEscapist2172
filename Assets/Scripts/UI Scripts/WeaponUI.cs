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
        if(playerCombat.GetIsEquippedFlag())
        {
            weapon.enabled = true;
            ammoText.enabled = true;
        }
        else
        {
            weapon.enabled = false;
            ammoText.enabled = false;
        }
    }

    public void UpdateAmmo(float ammo)
    {
        ammoText.text = ammo + " / ∞";
    }

    public void ShowReloading(bool state)
    {
        if(state)
        {
            reloadAnim = StartCoroutine(ReloadTextAnimation());
        }
        else
        {
            StopCoroutine(reloadAnim);
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
