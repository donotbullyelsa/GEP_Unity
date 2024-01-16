using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InventorySpace;

public class Slot
{
    public Item item;
    public int item_count;

    public Slot()
    {
        item = new Item(Item.ItemID.None);
        item_count = 0;
    }

    public void AddItem(Item item_)
    {
        item = item_;
        item_count++;
    }
}
