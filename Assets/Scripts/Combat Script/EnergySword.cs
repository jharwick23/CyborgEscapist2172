using UnityEngine;

public class EnergySword : Weapon
{
    // References
    [Header("Sword Animator")]
    [SerializeField] private Animator _swordanimator;

    [Header("Sword Sprite Renderer")]
    [SerializeField] private SpriteRenderer swordSr;

    [Header("Capsule Collider")]
    [SerializeField] private CapsuleCollider2D col;

    [Header("Player Combat")]
    [SerializeField] private PlayerCombat playerCombat;

    // Internal Variables
    private float lastSwingTimer;
    private float lastSwingDuration = 1f;

    void Awake()
    {
        if(!_swordanimator){
            _swordanimator = GetComponent<Animator>();
        }
        if(!swordSr){
            swordSr = GetComponentInChildren<SpriteRenderer>();
        }
        if(!col)
        {
            col = GetComponent<CapsuleCollider2D>();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!playerCombat)
        {
            playerCombat = FindFirstObjectByType<PlayerCombat>();
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        // Enables the sword sprite if "swung"
        if(!transform.parent)
        {
            swordSr.enabled = true;
        }
        else
        {
            CheckLastSwing();
        }
    }

    // Sets aiming position
    public override void OnAim(PlayerCombat.FirePosition firePos, Transform weaponHolder)
    {
        weaponHolder.localPosition = Vector3.zero;

        switch(firePos)
        {
            case PlayerCombat.FirePosition.Left:
                weaponHolder.localPosition += new Vector3(-0.5f, 0f, 0f);
                break;
            case PlayerCombat.FirePosition.Right:
                weaponHolder.localPosition += new Vector3(0.5f, 0f, 0f);
                break;
            case PlayerCombat.FirePosition.Up:
                weaponHolder.localPosition += new Vector3(0f, 0.75f, 0f);
                break;
            case PlayerCombat.FirePosition.Down:
                weaponHolder.localPosition += new Vector3(0f, -0.75f, 0f);
                break;
        }
    }

    // Stab Sword
    public override void Use(PlayerCombat.FirePosition firePos)
    {
        Debug.Log("StabSword");

        // Sets sprite based on which direction player is shooting
        switch(firePos){
            case PlayerCombat.FirePosition.Up:
                _swordanimator.SetFloat("DirectionX", 0);
                _swordanimator.SetFloat("DirectionY", 1);
                break;
            case PlayerCombat.FirePosition.Down:
                _swordanimator.SetFloat("DirectionX", 0);
                _swordanimator.SetFloat("DirectionY", -1);
                break;
            case PlayerCombat.FirePosition.Left:
                _swordanimator.SetFloat("DirectionX", -1);
                _swordanimator.SetFloat("DirectionY", 0);
                break;
            case PlayerCombat.FirePosition.Right:
                _swordanimator.SetFloat("DirectionX", 1);
                _swordanimator.SetFloat("DirectionY", 0);
                break;
        }
        SetLastSwing();
        ResetUseTimer();
    }

    // Resets the duration of the last swing
    void SetLastSwing()
    {
        lastSwingTimer = lastSwingDuration;
    }

    // Checks last swing 
    // If sword has not been swung in allocated time
    // Do not render the sprite
    void CheckLastSwing()
    {
        if(lastSwingTimer > 0)
        {
            lastSwingTimer -= Time.deltaTime;
            swordSr.enabled = true;
            col.enabled = true; // enable hitbox
        }
        else
        {
            swordSr.enabled = false; 
            col.enabled = false; // disable hitbox
        }
    }

    // Logic to hurt enemies 
    private void OnTriggerStay2D(Collider2D other)
    {
        if(playerCombat == null)
        {
            Debug.Log("PlayerCombat does not exist!");
            return;
        }

        // Enemy only takes damage when the player is swinging the sword
        if(other.gameObject.CompareTag("Enemy") && col.enabled == true){
            Debug.Log("Enemy stabbed");
            other.gameObject.GetComponent<EnemyLogic>().DoDamage(1f);
        }
    }
}
