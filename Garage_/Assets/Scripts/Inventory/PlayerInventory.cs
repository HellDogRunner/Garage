using System;
using UnityEngine;
using Zenject;

public class PlayerInventory : Inventory
{
    [SerializeField] private Transform _hand;

    private Item _currentItem;
    private GameObject _currentGameObjectInHand;

    public Item CurrentItemInHand => _currentItem;
    public event Action<string> OnItemReadyToPickUp;

    [Inject]
    private void Construct(InputController inputController, DiContainer diContainer)
    {
        _diContainer = diContainer;
        _inputController = inputController;
        _inputController.OnFirstInventorySlotPressed += SelectItem;
        _inputController.OnSecoundInventorySlotPressed += SelectItem;
        _inputController.OnThirdInventorySlotPressed += SelectItem;
    }

    private void OnDisable()
    {
        _inputController.OnFirstInventorySlotPressed -= SelectItem;
        _inputController.OnSecoundInventorySlotPressed -= SelectItem;
        _inputController.OnThirdInventorySlotPressed -= SelectItem;
    }

    private void OnTriggerStay(Collider other)
    {
        if (_currentItem != null) return;
        var pickupItem = other.GetComponent<PickupItem>();
        if (pickupItem)
        {
            OnItemReadyToPickUp?.Invoke(pickupItem.Item.itemName);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var item = other.GetComponent<PickupItem>();
        if (item)
        {
            OnItemReadyToPickUp?.Invoke(null);
        }
    }

    private void SelectItem(int slotIndex)
    {
        Destroy(_currentGameObjectInHand);
        if (slotIndex + 1 > _slots.Count)
        {
            _currentItem = null;
            return;
        }
        if (_slots[slotIndex] != null)
        {
            _currentItem = _slots[slotIndex];
            CreateItemPrefab();
        }
    }

    private void CreateItemPrefab()
    {
        GameObject itemPrefab = _currentItem.itemPrefab;
        if (itemPrefab != null)
        {
            _currentGameObjectInHand = _diContainer.InstantiatePrefab(itemPrefab, _hand.position, _hand.rotation, _hand);
            var pickup = _currentGameObjectInHand.GetComponent<PickupItem>();
            pickup.enabled = false;
        }
        OnItemReadyToPickUp?.Invoke(null);
    }

    public override void RemoveItem(Item item)
    {
        base.RemoveItem(item);
        _currentItem = null;
        Destroy(_currentGameObjectInHand);
    }
}
