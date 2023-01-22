using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyGame;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Canvas inventoryCanvas;
    [SerializeField] private TMP_Text mainBagText;
    [SerializeField] private TMP_Text bagsCountText;

    [Tooltip("Drag from player")] [SerializeField]
    private InventoryObject inventoryOfBags;

    private PlayerInputActions _playerInput;
    private InputAction _inventory;
    private bool _openToggle = false;
    private int maxMainBag = 1;
    private int maxVaccineBags = 9;

    private void Awake()
    {
        _playerInput = new PlayerInputActions();
    }

    private void Start()
    {
        if (inventoryOfBags == null)
        {
            inventoryOfBags = ScriptableObject.CreateInstance<InventoryObject>();
        }

        inventoryCanvas.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _inventory = _playerInput.Player.Inventory;
        _inventory.Enable();
        _inventory.performed += OpenInventory;
    }

    private void OpenInventory(InputAction.CallbackContext obj)
    {
        _openToggle = !_openToggle;

        Debug.Log("Open inventory");
        inventoryCanvas.gameObject.SetActive(_openToggle);
        mainBagText.text =
            $"{inventoryOfBags.Container.Count(slot => slot.item.itemType == ItemType.MainVaccineBag)}/{maxMainBag}";
        bagsCountText.text =
            $"{inventoryOfBags.Container.Count(slot => slot.item.itemType == ItemType.VaccineBag)}/{maxVaccineBags}";
    }

    private void OnDisable()
    {
        _inventory.Disable();
    }

    private void OnApplicationQuit()
    {
        inventoryOfBags.Container.Clear();
    }
}