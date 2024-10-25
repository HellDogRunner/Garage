using System;
using System.Collections.Generic;
using UnityEngine;

public class TrunkInventory : Inventory
{
    [SerializeField] List<Transform> _transforms = new List<Transform>();
    [SerializeField] private Collider _triggerForPlayer;

    private PlayerInventory _playerInventory;
    private int _itemCounter;

    public int ItemCounter => _itemCounter;
    public event Action<int> OnItemsAdded;

    private void OnTriggerEnter(Collider other)
    {
        var inventory = GetComponent<PlayerInventory>();
        if (inventory)
        {
            _playerInventory = inventory;
            if (_playerInventory.CurrentItemInHand != null)
            {
                var item = _playerInventory.CurrentItemInHand;
                AddItem(item);
                PlaceItem(item);
                _itemCounter++;
                _playerInventory.RemoveItem(item);
                OnItemsAdded?.Invoke(_itemCounter);
            }
        }
    }

    private void PlaceItem(Item item)
    {
        for (int i = 0; i < _transforms.Count; i++)
        {
            bool isOccupaid = _transforms[i].GetComponentInChildren<PickupItem>();
            if (!isOccupaid) 
            {
                var spawnpoint = _transforms[i];
                GameObject itemPrefab = item.itemPrefab;
                if (itemPrefab != null)
                {
                    var prefab = _diContainer.InstantiatePrefab(itemPrefab, spawnpoint.position, spawnpoint.rotation, spawnpoint);
                    var pickup = prefab.GetComponent<PickupItem>();
                    pickup.enabled = false;
                }
                break;
            }
        }
    }
}
