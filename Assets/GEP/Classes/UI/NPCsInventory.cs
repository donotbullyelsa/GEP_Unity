using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using InventorySpace;

public class NPCsInventory : InventoryUIBase
{
    public PlayerInventory player_inventory;
    private UIController ui_controller;

    //Prevent any inventory UI to be opened in one frame
    private bool inventory_lock;

    protected override void Start()
    {
        base.Start();
    }
    
    protected override void Awake()
    {
        base.Awake();
        ui_controller = GameObject.FindGameObjectsWithTag("UI Controller")[0].GetComponent<UIController>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        input.Inventory.Open.started += ToggleInventory;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        input.Inventory.Open.started -= ToggleInventory;
    }

    protected override void OnMouseClick(InputAction.CallbackContext context)
    {
        base.OnMouseClick(context);

        // if clicked on the 'change inventory' button (the right arrow appear when player is near to npc)
        if (RectTransformToScreenSpace(transform.GetChild(2).GetChild(1).GetComponent<Image>().rectTransform).Contains(Mouse.current.position.ReadValue()))
        {
            // Changing to player's inventory
            panel.SetActive(false);
            player_inventory.OpenInventory(true);
        }

        int x = PointsOnSlotCheck(Mouse.current.position.ReadValue());

        // Item transfering
        if (x >= 0)
        {
            ui_controller.NPCToPlayer(x);
        }
    }

    private void ToggleInventory(InputAction.CallbackContext value)
    {
        if (panel.activeSelf)
        {
            panel.SetActive(false);
            inventory_lock = true;
        }
    }

    protected override void Update()
    {
        inventory_lock = false;

        base.Update();
    }

    public bool getInvetoryLock()
    {
        return inventory_lock;
    }
}
