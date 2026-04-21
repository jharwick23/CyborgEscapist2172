using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    // References
    [Header("Player References")]
    [SerializeField] private PlayerMovement _playerMovementController;
    [SerializeField] private PlayerCombat _playerCombat;
    [SerializeField] private Interactor _interactor;
    [SerializeField] private ShieldAbility _shieldAbility;

    // Weapon References
    [Header("Weapon References")]
    [SerializeField] private Gun _gun;

    // UI References
    [Header("UI References")]
    [SerializeField] private PauseMenu _pauseMenu;

    
    // Input System
    private PlayerInput _playerInput;
    private InputAction _moveAction, _firedownAction, _fireupAction,
                        _fireleftAction, _firerightAction, _interactAction, 
                        _dropAction, _reloadAction, _pauseAction, _shieldAction,
                        _switchAction;

    // Runtime variables
    private Camera mainCam;

    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        mainCam = Camera.main;
        if(_playerMovementController == null)
        {
            _playerMovementController = GetComponent<PlayerMovement>();
        }
        if(_playerCombat == null)
        {
            _playerCombat = GetComponent<PlayerCombat>();
        }
        if(_interactor == null)
        {
            _interactor = GetComponent<Interactor>();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!_gun)
        {
            _gun = FindFirstObjectByType<Gun>();
        }
        if(!_pauseMenu)
        {
            _pauseMenu = FindFirstObjectByType<PauseMenu>();
        }
        if(!_shieldAbility)
        {
            _shieldAbility = FindFirstObjectByType<ShieldAbility>();
        }
        if (_playerInput != null)
        {
            _moveAction = _playerInput.actions.FindAction("Move");
            _fireupAction = _playerInput.actions.FindAction("FireUp");
            _firedownAction = _playerInput.actions.FindAction("FireDown");
            _fireleftAction = _playerInput.actions.FindAction("FireLeft");
            _firerightAction = _playerInput.actions.FindAction("FireRight");
            _interactAction = _playerInput.actions.FindAction("Interact");
            _dropAction = _playerInput.actions.FindAction("Drop");
            _reloadAction = _playerInput.actions.FindAction("Reload");
            _pauseAction = _playerInput.actions.FindAction("Pause");
            _shieldAction = _playerInput.actions.FindAction("Shield");
            _switchAction = _playerInput.actions.FindAction("SwitchWeapon");

            _pauseAction.performed += OnPausePerformed;
            EnableInputs();
        }
        else
        {
            Debug.LogWarning("PlayerInput component not found on GameObject.  Make sure an Input Actions asset or PlayerInput is present.");
        }


        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Fire
        if(_playerCombat != null && (_fireupAction != null || _firedownAction != null || _fireleftAction != null || _firerightAction != null))
        {
            // Single shot per click
            if (_fireupAction.WasPressedThisFrame()){
                _playerCombat.UseWeapon(PlayerCombat.FirePosition.Up);
            }
            if (_firedownAction.WasPressedThisFrame()){
                _playerCombat.UseWeapon(PlayerCombat.FirePosition.Down);
            }
            if (_fireleftAction.WasPressedThisFrame()){
                _playerCombat.UseWeapon(PlayerCombat.FirePosition.Left);
            }
            if (_firerightAction.WasPressedThisFrame()){
                _playerCombat.UseWeapon(PlayerCombat.FirePosition.Right);
            }
        }
    }

    void FixedUpdate()
    {
        if(_moveAction != null){
            Vector2 movementVector = _moveAction.ReadValue<Vector2>();
            _playerMovementController.Move(movementVector);    
        }
    }

    public void EnableInputs()
    {
        _dropAction.performed += OnDropPerformed;
        _interactAction.performed += OnInteractPerformed;
        _reloadAction.performed += OnReloadPerformed;
        _shieldAction.performed += OnShieldPerformed;
        _switchAction.performed += OnSwitchPerformed;
        _moveAction?.Enable();
        _fireupAction?.Enable();
        _firedownAction?.Enable();
        _fireleftAction?.Enable();
        _firerightAction?.Enable();
    }

    public void DisableInputs()
    {
        _dropAction.performed -= OnDropPerformed;
        _interactAction.performed -= OnInteractPerformed;
        _reloadAction.performed -= OnReloadPerformed;
        _shieldAction.performed -= OnShieldPerformed;
        _switchAction.performed -= OnSwitchPerformed;
        _moveAction?.Disable();
        _fireupAction?.Disable();
        _firedownAction?.Disable();
        _fireleftAction?.Disable();
        _firerightAction?.Disable();
    }

    private void OnInteractPerformed(InputAction.CallbackContext context){
        _interactor.TryInteract();
    }

    private void OnDropPerformed(InputAction.CallbackContext context){
        _playerCombat.DropWeapon();
    }

    private void OnReloadPerformed(InputAction.CallbackContext context)
    {
        _gun.Reload();
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        Cursor.lockState = CursorLockMode.None;
        _pauseMenu.PerformPause();
    }

    private void OnShieldPerformed(InputAction.CallbackContext context)
    {
        _shieldAbility.ToggleShield();
    }

    private void OnSwitchPerformed(InputAction.CallbackContext context)
    {
        _playerCombat.CycleWeapon();
    }
}
