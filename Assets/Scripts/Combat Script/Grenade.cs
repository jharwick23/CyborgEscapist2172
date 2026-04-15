using UnityEngine;

public class Grenade : Weapon
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    // Sets aiming position
    public override void OnAim(PlayerCombat.FirePosition firePos, Transform weaponHolder)
    {
        weaponHolder.localPosition = Vector3.zero;

        switch(firePos)
        {
            case PlayerCombat.FirePosition.Left:
                weaponHolder.localPosition += new Vector3(-0.25f, 0f, 0f);
                break;
            case PlayerCombat.FirePosition.Right:
                weaponHolder.localPosition += new Vector3(0.25f, 0f, 0f);
                break;
            case PlayerCombat.FirePosition.Up:
                weaponHolder.localPosition += new Vector3(0f, 0.5f, 0f);
                break;
            case PlayerCombat.FirePosition.Down:
                weaponHolder.localPosition += new Vector3(0f, -0.5f, 0f);
                break;
        }
    }

    // Throw Grenade
    public override void Use(PlayerCombat.FirePosition firePos)
    {
        Debug.Log("Throw Grenade");
        ResetUseTimer();
    }
}
