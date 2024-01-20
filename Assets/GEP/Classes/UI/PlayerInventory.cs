using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using InventorySpace;

public class PlayerInventory : InventoryUIBase
{
    public GameObject player;
    public GameObject object_spawn;
    public NPCsInventory npc_inventory;

    public GameObject npc;
    private bool near_npc_flag;
    private UIController ui_controller;

    protected override void Start()
    {
        base.Start();
        near_npc_flag = false;
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

        // Near npc
        if (near_npc_flag)
        {
            // if clicked on the 'change inventory' button (the right arrow appear when player is near to npc)
            if (RectTransformToScreenSpace(transform.GetChild(2).GetChild(1).GetComponent<Image>().rectTransform).Contains(Mouse.current.position.ReadValue()))
            {
                // Changing to NPC's inventory
                panel.SetActive(false);
                npc_inventory.OpenInventory(true);
            }

            int x = PointsOnSlotCheck(Mouse.current.position.ReadValue());

            // Item transfering
            if (x >= 0)
            {
                ui_controller.playerToNPC(x);
            }
        }

        else
        {    
            // Item deleting
            DeleteItem(PointsOnSlotCheck(Mouse.current.position.ReadValue()));
        }
    }

    private void ToggleInventory(InputAction.CallbackContext value)
    {
        if (!panel.activeSelf)
        {
            // Ask UI controller for permission to open the inventory UI
            // No other inventory can be opened when one is on
            if (ui_controller.getInvenOpenPermission())
            {
                panel.SetActive(true);
            }
        }

        else
        {
            panel.SetActive(false);
        }
    }

    protected override void Update()
    {
        base.Update();

        // Is near the npc?
        near_npc_flag = (DistanceCheck(player.transform, npc.transform) < 5.0f);

        // Show button if near the npc
        transform.GetChild(2).GetChild(1).GetComponent<Image>().enabled = near_npc_flag;
    }

    public override Item.ItemID DeleteItem(int n)
    {
        // if out of bound
        if ((n >= Inventory.column * Inventory.row) || (n < 0))
        {
            return Item.ItemID.None;
        }

        Vector2Int slot_pos = inventory.IndexToV2(n);
        if (SlotIsNotEmpty(n))
        {
            Item delete_item = inventory.slots[slot_pos.x, slot_pos.y].item;
            CreateItem(delete_item);
        }   

        // Do the other thing
        return base.DeleteItem(n);
    }

    public Item.ItemID BaseDeleteItem(int n)
    {
        return base.DeleteItem(n);
    }

    // Create a game object to the world
    private void CreateItem(Item item)
    {
        GameObject prefab = Resources.Load<GameObject>(item.prefab_link);
        GameObject new_object = Instantiate(prefab);
        new_object.GetComponent<ItemMonoBase>().player_inventory = this;

        // Set position
        new_object.transform.SetParent(player.transform, true);
        new_object.transform.localPosition = new Vector3(0f, 0f, 1f);
        new_object.transform.SetParent(object_spawn.transform, true);
        new_object.transform.localPosition = new Vector3(new_object.transform.localPosition.x, player.transform.localPosition.y, new_object.transform.localPosition.z);
    }
}
