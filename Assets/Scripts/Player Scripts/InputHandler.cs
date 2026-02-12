using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    // References
    [Header("Player References")]
    [SerializeField] private PlayerMovement _playerMovementController;
    [SerializeField] private PlayerCombat _playerCombat;
    
    // Input System
    private PlayerInput _playerInput;
    private InputAction _moveAction, _firedownAction, _fireupAction, _fireleftAction, _firerightAction, _interactAction, _dropAction;

    // Runtime variables
    private Camera mainCam;

    void Awake()
    {
        if(_playerMovementController == null)
        {
            _playerMovementController = FindFirstObjectByType<PlayerMovement>();
        }
        if(_playerCombat == null)
        {
            _playerCombat = FindFirstObjectByType<PlayerCombat>();
        }
        _playerInput = GetComponent<PlayerInput>();
        mainCam = Camera.main;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_playerInput != null)
        {
            _moveAction = _playerInput.actions.FindAction("Move");
            _fireupAction = _playerInput.actions.FindAction("FireUp");
            _firedownAction = _playerInput.actions.FindAction("FireDown");
            _fireleftAction = _playerInput.actions.FindAction("FireLeft");
            _firerightAction = _playerInput.actions.FindAction("FireRight");
            _interactAction = _playerInput.actions.FindAction("Interact");
            _dropAction = _playerInput.actions.FindAction("Drop");
            _dropAction.performed += OnDropPerformed;
            _interactAction.performed += OnInteractPerformed;
        }
        else
        {
            Debug.LogWarning("PlayerInput component not found on GameObject.  Make sure an Input Actions asset or PlayerInput is present.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_moveAction != null){
            Vector2 movementVector = _moveAction.ReadValue<Vector2>();
            _playerMovementController.Move(movementVector);    
        }

        // Fire
        if(_playerCombat != null && (_fireupAction != null || _firedownAction != null || _fireleftAction != null || _firerightAction != null))
        {
            // Single shot per click
            if (_fireupAction.WasPressedThisFrame()){
                _playerCombat.Fire(PlayerCombat.FirePosition.Up);
            }
            if (_firedownAction.WasPressedThisFrame()){
                _playerCombat.Fire(PlayerCombat.FirePosition.Down);
            }
            if (_fireleftAction.WasPressedThisFrame()){
                _playerCombat.Fire(PlayerCombat.FirePosition.Left);
            }
            if (_firerightAction.WasPressedThisFrame()){
                _playerCombat.Fire(PlayerCombat.FirePosition.Right);
            }
        }
    }

    private void OnInteractPerformed(InputAction.CallbackContext context){
        _playerCombat.EquipWeapon();
    }

    private void OnDropPerformed(InputAction.CallbackContext context){
        _playerCombat.DropWeapon();
    }
}
