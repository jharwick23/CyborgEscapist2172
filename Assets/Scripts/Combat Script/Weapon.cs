using UnityEngine;

public abstract class Weapon : MonoBehaviour, IInteractable
{
    [Header("Fire Rate Settings")]
    [SerializeField] protected float timeBetweenUses = 0.2f;
    protected float useTimer;

    [Header("WeaponUI")]
    [SerializeField] protected Sprite weaponIcon;

    public bool CanInteract()
    {
        return true;
    }

    // Interaction system for weapons
    public bool Interact(Interactor interactor)
    {
        PlayerCombat player = interactor.GetComponent<PlayerCombat>();
        if(player)
        {
            player.EquipWeapon(this);
            return true;
        }

        return false;
    }

    // Fire rates for weapons
    protected virtual void Update()
    {
        if(useTimer > 0f)
        {
            useTimer -= Time.deltaTime;
        }
    }

    public bool CanUse()
    {
        return useTimer <= 0f;
    }

    protected void ResetUseTimer()
    {
        useTimer = timeBetweenUses;
    }

    // Called when player aims
    public virtual void OnAim(PlayerCombat.FirePosition firePos, Transform weaponHolder)
    {
        
    }

    // UI methods
        public virtual Sprite GetWeaponIcon()
    {
        return weaponIcon;
    }

    public virtual bool ShouldShowAmmo()
    {
        return false;
    }

    public virtual string GetAmmoText()
    {
        return "";
    }

    public virtual bool IsReloading()
    {
        return false;
    }

    // Abstract method allows different weapons to be used
    public abstract void Use(PlayerCombat.FirePosition firePos);
}
