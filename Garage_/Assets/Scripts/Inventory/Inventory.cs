using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class Inventory : MonoBehaviour
{
    [SerializeField] protected float _inventorySize = 3;

    protected List<Item> _slots = new List<Item>();
    protected DiContainer _diContainer;
    protected InputController _inputController;

    public List<Item> Slots => _slots;

    public event Action OnItemsChanged;

    [Inject]
    private void Construct(InputController inputController, DiContainer diContainer)
    {
        _diContainer = diContainer;
        _inputController = inputController;
    }

    public virtual bool AddItem(Item newItem)
    {
        if (_slots.Count < _inventorySize)
        {
            _slots.Add(newItem);
            OnItemsChanged?.Invoke();
            return true;
        }
        return false;
    }

    public virtual void RemoveItem(Item item)
    {
        if (item == null) return;
        _slots.Remove(item);
        OnItemsChanged?.Invoke();
    }
}
