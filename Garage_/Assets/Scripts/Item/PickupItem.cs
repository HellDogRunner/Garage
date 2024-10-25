using System;
using UnityEngine;
using Zenject;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private Item _item;

    private PlayerInventory _playerInventory;
    private InputController _inputController;
    private HudManager _hudManager;
    private bool _inRange;

    public Item Item => _item;
    public event Action<string> OnItemReadyToPickUp;

    [Inject]
    private void Construct(PlayerInventory playerInventory, InputController inputController, HudManager hudManager)
    {
        _playerInventory = playerInventory;
        _hudManager = hudManager;
        _inputController = inputController;
        _inputController.OnInteractionKeyPressed += AddToInventory;
    }

    private void OnDisable()
    {
        _inputController.OnInteractionKeyPressed -= AddToInventory;
        OnItemReadyToPickUp?.Invoke(null);
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerMovement>();
        if (player)
        {
            _inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<PlayerMovement>();
        if (player)
        {
            _inRange = false;
        }
    }

    private void OnDestroy()
    {
        _hudManager.ClosePickupMessage();
    }

    private void AddToInventory()
    {
        if (_inRange)
        {
            if (_playerInventory.AddItem(_item))
            {
                Destroy(gameObject);
            }
        }
    }

}
