using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    private GameControls _playerControls;
    private Vector3 _moveDirection;
    private Vector2 _mousePosition;
    public Vector3 MoveDirection => _moveDirection;
    public Vector2 MousePosition => _mousePosition;

    public event Action OnInteractionKeyPressed;
    public event Action<int> OnFirstInventorySlotPressed;
    public event Action<int> OnSecoundInventorySlotPressed;
    public event Action<int> OnThirdInventorySlotPressed;
    public event Action<bool> OnJumpKeyPressed;

    private void Awake()
    {
        _playerControls = new GameControls();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
        SubscribeToActions();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
        UnsubscribeToActions();
        _moveDirection = Vector3.zero;
    }

    private void ChangeDirection(InputAction.CallbackContext context)
    {
        var direction = _playerControls.Gameplay.WASD.ReadValue<Vector3>();
        _moveDirection = new Vector3(direction.x, direction.z, direction.y);
    }

    private void ChangeMousePosition(InputAction.CallbackContext context) 
    {
        _mousePosition = _playerControls.Gameplay.MouseMovement.ReadValue<Vector2>();
    }

    private void Jump(InputAction.CallbackContext context)
    {
        var isPressed = _playerControls.Gameplay.Jump.IsPressed();
        OnJumpKeyPressed?.Invoke(isPressed);
    }

    private void Interact(InputAction.CallbackContext context)
    {
        OnInteractionKeyPressed?.Invoke();
    }

    private void FirstInvetorySlot(InputAction.CallbackContext context)
    {
        OnFirstInventorySlotPressed?.Invoke(0);
    }

    private void SecoundInvetorySlot(InputAction.CallbackContext context)
    {
        OnSecoundInventorySlotPressed?.Invoke(1);
    }

    private void ThirdInvetorySlot(InputAction.CallbackContext context)
    {
        OnThirdInventorySlotPressed?.Invoke(2);
    }

    private void SubscribeToActions()
    {
        _playerControls.Gameplay.WASD.performed += ChangeDirection;
        _playerControls.Gameplay.MouseMovement.performed += ChangeMousePosition;
        _playerControls.Gameplay.Jump.performed += Jump;
        _playerControls.Gameplay.Interact.performed += Interact;
        _playerControls.Gameplay.InventorySlot1.performed += FirstInvetorySlot;
        _playerControls.Gameplay.InventorySlot2.performed += SecoundInvetorySlot;
        _playerControls.Gameplay.InventorySlot3.performed += ThirdInvetorySlot;
    }

    private void UnsubscribeToActions()
    {
        _playerControls.Gameplay.WASD.performed -= ChangeDirection;
        _playerControls.Gameplay.MouseMovement.performed -= ChangeMousePosition;
        _playerControls.Gameplay.Jump.performed -= Jump;
        _playerControls.Gameplay.Interact.performed -= Interact;
        _playerControls.Gameplay.InventorySlot1.performed -= FirstInvetorySlot;
        _playerControls.Gameplay.InventorySlot2.performed -= SecoundInvetorySlot;
        _playerControls.Gameplay.InventorySlot3.performed -= ThirdInvetorySlot;
    }
}
