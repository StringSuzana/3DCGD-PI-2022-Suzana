using System;
using System.Collections;
using System.Collections.Generic;
using MyGame;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Canvas inventoryCanvas;
    [SerializeField] private TMP_Text mainBagText;
    [SerializeField] private TMP_Text bagsCountText;

    private PlayerInputActions _playerInput;
    private InputAction _inventory;


    private void Start()
    {
        _playerInput = new PlayerInputActions();
        inventoryCanvas.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _inventory = _playerInput.Player.Jump;
        _inventory.Enable();
        _inventory.performed += OpenInventory;

    }

    private void OpenInventory(InputAction.CallbackContext obj)
    {
        inventoryCanvas.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        _inventory.Disable();
    }
}
