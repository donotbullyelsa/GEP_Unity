using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using InventorySpace;

// Inventory UI class shared between player and NPC
public class InventoryUIBase : MonoBehaviour
{
    protected UIControls input = null;

    protected Inventory inventory;

    protected GameObject panel;
    protected GameObject ref_slot;
    protected ItemDescriptionControl description_text_script;

    protected virtual void Start()
    {
        panel = transform.GetChild(0).gameObject;
        ref_slot = panel.transform.GetChild(0).gameObject;
        panel.SetActive(false);
        description_text_script = transform.GetChild(1).GetComponent<ItemDescriptionControl>();

        StartInventory();
    }

    protected void StartInventory()
    {
        // Initialize the inventory
        inventory = new Inventory();

        // Set up the slots' position of inventory
        GameObject slot;
        RectTransform slot_transform;
        for (int j = 0; j < Inventory.row; j++)
        {
            for (int i = 0; i < Inventory.column; i++)
            {
                if (!(i == 0) || !(j == 0))
                {
                    slot = Instantiate(ref_slot, panel.transform);
                    slot_transform = slot.GetComponent<RectTransform>();

                    Vector2 moveVector = new Vector2(slot_transform.rect.width * i + 40f * i, -slot_transform.rect.height * j - 40f * j);
                    slot_transform.anchoredPosition += moveVector;
                }
            }
        }

        
    }

    protected virtual void Awake()
    {
        input = new UIControls();
    }

    protected virtual void OnEnable()
    {
        input.Enable();
        input.Inventory.Click.performed += OnMouseClick;
    }

    protected virtual void OnDisable()
    {
        input.Disable();
        input.Inventory.Click.performed -= OnMouseClick;
    }

    protected virtual void OnMouseClick(InputAction.CallbackContext context)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OpenInventory(bool flag)
    {
        panel.SetActive(flag);
    }

    protected virtual void Update()
    {
        // Update the position of the item description text
        description_text_script.UpdatePosition(Mouse.current.position.ReadValue());

        // Check which slot is the cursor currently in on
        int on_slot = PointsOnSlotCheck(Mouse.current.position.ReadValue());

        // When inventory is opened up and cursor is on any non-empty slot, show text
        if ((panel.activeSelf) && (on_slot >= 0))
        {
            description_text_script.SetEnabled(inventory.slots[inventory.IndexToV2(on_slot).x, inventory.IndexToV2(on_slot).y].item.id != Item.ItemID.None);
            description_text_script.SetString(inventory.slots[inventory.IndexToV2(on_slot).x, inventory.IndexToV2(on_slot).y].item.description);
        }

        else
        {
            description_text_script.SetEnabled(false);
        }

        // if inventory is off, other objects on panel are off
        GameObject fixed_objects = transform.GetChild(2).gameObject;
        fixed_objects.SetActive(panel.activeSelf);
    }

    public bool AddItem(Item item)
    {
        // Add the item to the inventory
        int n = inventory.AddItem(item);

        // if no item is added (inventory full)
        if (n < 0)
        {
            return false;
        }

        // Get the assigned slot
        GameObject slot = transform.GetChild(0).GetChild(n).gameObject;

        // Update the inventory item count
        slot.transform.GetChild(1).GetComponent<ItemCount>().SetCount(inventory.slots[inventory.IndexToV2(n).x, inventory.IndexToV2(n).y].item_count);

        // Change the sprite of the slot
        Image temp = slot.transform.GetChild(0).GetComponent<Image>();
        temp.sprite = Resources.Load<Sprite>(item.image_link);

        // Update the color of the slot
        temp.color = new Color(1f, 1f, 1f, 1f);

        return true;
    }

    public virtual Item.ItemID DeleteItem(int n)
    {
        Item.ItemID id = Item.ItemID.None;

        // if out of bound
        if ((n >= Inventory.column * Inventory.row) || (n < 0))
        {
            return id;
        }

        Vector2Int slot_pos = inventory.IndexToV2(n);

        if (SlotIsNotEmpty(n))
        {
            // Slot item count decrease 1
            inventory.slots[slot_pos.x, slot_pos.y].item_count--;
            id = inventory.slots[slot_pos.x, slot_pos.y].item.id;

            // Get the assigned slot
            GameObject slot = transform.GetChild(0).GetChild(n).gameObject;

            // Update the inventory item count
            slot.transform.GetChild(1).GetComponent<ItemCount>().SetCount(inventory.slots[inventory.IndexToV2(n).x, inventory.IndexToV2(n).y].item_count);
            
            // if no more item left, remove the record
            if (inventory.slots[slot_pos.x, slot_pos.y].item_count <= 0)
            {
                inventory.slots[slot_pos.x, slot_pos.y].item.id = Item.ItemID.None;

                // Delete the slot image sprite
                Image temp = slot.transform.GetChild(0).GetComponent<Image>();
                temp.sprite = null;

                // Update the color of the slot
                temp.color = new Color(1f, 1f, 1f, 0.38f);
            }
        }

        return id;
    }

    protected bool SlotIsNotEmpty(int n)
    {
        Vector2Int v2 = inventory.IndexToV2(n);

        // if slot is not empty
        if (inventory.slots[v2.x, v2.y].item.id == Item.ItemID.None)
        {
            return false;
        }

        return true;
    }

    // return the index of slot the mouse has clicked on
    protected int PointsOnSlotCheck(Vector2 point)
    {
        if (panel.activeSelf)
        {
            int i = 0;
            GameObject inventory_panel = transform.GetChild(0).gameObject;
            int slot_count = inventory_panel.transform.childCount;

            // Search for the slot that is clicked on
            while (i < slot_count)
            {
                Image temp_image = inventory_panel.transform.GetChild(i).GetComponent<Image>();
                if (RectTransformToScreenSpace(temp_image.rectTransform).Contains(point))
                {
                    return i;
                }
                i++;
            }
        }
        return -1;
    }

    // Transform RectTransform to a Rect in global space    
    protected Rect RectTransformToScreenSpace(RectTransform rectTransform)
    {
        Vector2 size = Vector2.Scale(rectTransform.rect.size, rectTransform.lossyScale);
        Rect rect = new Rect(rectTransform.position.x, rectTransform.position.y, size.x, size.y);
        rect.x -= rectTransform.pivot.x * size.x;
        rect.y -= rectTransform.pivot.y * size.y;
        return rect;
    }

    protected float DistanceCheck(Transform transform1, Transform transform2)
    {
        return Vector3.Distance(transform1.position, transform2.position);
    }
}
