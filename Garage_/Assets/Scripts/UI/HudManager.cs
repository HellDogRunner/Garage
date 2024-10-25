using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HudManager : MonoBehaviour
{
    [SerializeField] private List<Image> _slotImages = new List<Image>();
    [SerializeField] private TMP_Text _counterText;
    [SerializeField] private TMP_Text _pickupMessage;

    private PlayerInventory _playerInventory;
    private TrunkInventory _trunkInventory;
    private InputController _inputController;

    [Inject]
    private void Construct(PlayerInventory playerInventory, InputController inputController, TrunkInventory trunkInventory)
    {
        _playerInventory = playerInventory;
        _trunkInventory = trunkInventory;
    }
    private void OnEnable()
    {
        _playerInventory.OnItemsChanged += UpdateInventoryUI;
        _playerInventory.OnItemReadyToPickUp += ShowPickupMessage;
        _trunkInventory.OnItemsAdded += UpdateItemCounterText;
    }

    private void OnDisable()
    {
        _playerInventory.OnItemsChanged -= UpdateInventoryUI;
        _playerInventory.OnItemReadyToPickUp -= ShowPickupMessage;
        _trunkInventory.OnItemsAdded -= UpdateItemCounterText;
    }

    private void UpdateInventoryUI()
    {
        for (int i = 0; i < _slotImages.Count; i++)
        {
            _slotImages[i].sprite = null;
            _slotImages[i].gameObject.SetActive(false); 
        }

        for (int i = 0; i < _playerInventory.Slots.Count; i++)
        {
            if (i < _slotImages.Count)
            {
                if (_playerInventory.Slots[i] != null)
                {
                    _slotImages[i].sprite = _playerInventory.Slots[i].itemIcon;
                    _slotImages[i].color =Color.white;
                    _slotImages[i].gameObject.SetActive(true);
                }
            }
        }
    }

    private void UpdateItemCounterText(int counter)
    {
        _counterText.text =  $"{counter}/6 погружено";
    }

    private void ShowPickupMessage(string itemName)
    {
        if (itemName != null)
        {
            _pickupMessage.text = $"Нажмите E чтобы подобрать {itemName}";
            _pickupMessage.gameObject.SetActive(true);
        }
        else
        {
            ClosePickupMessage();
        }
    }

    public void ClosePickupMessage()
    {
        if (_pickupMessage != null)
            _pickupMessage.gameObject.SetActive(false);
    }
}
