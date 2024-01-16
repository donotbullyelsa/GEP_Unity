using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InventorySpace;

public class Inventory
{
    public const int row = 4;
    public const int column = 8;
    public Slot[,] slots;

    public Inventory()
    {
        // Creating the slots
        slots = new Slot[column, row];
        for (int i = 0; i < column; i++)
        {
            for (int j = 0; j < row; j++)
            {
                slots[i, j] = new Slot();
            }
        }
    }

    private int[] SearchFreeSlot(Item item)
    {
        int[] ans = new int[2];
        ans[0] = -1;
        ans[1] = -1;

        for (int j = 0; j < row; j++)
        {
            for (int i = 0; i < column; i++)
            {
                // if slot item = search target item
                if (slots[i, j].item.id == item.id)
                {
                    // if slot not full
                    if (slots[i, j].item_count < item.stack_limit)
                    {
                        ans[0] = i;
                        ans[1] = j;
                        return ans;
                    }
                }
            }
        }

        return ans;
    }

    private int[] SpaceAssign(Item item)
    {
        int[] free_slot = new int[2];

        // if no free slot of item
        free_slot = SearchFreeSlot(item);
        if (free_slot[0] == -1)
        {

            // if no empty slot
            free_slot = SearchFreeSlot(new EmptyItem());
            if (free_slot[0] == -1)
            {
                return free_slot;
            }
        }

        return free_slot;
    }

    public int AddItem(Item item)
    {
        // Look for free space in inventory
        int[] free_slot = new int[2];

        // if no free slot of item
        free_slot = SpaceAssign(item);

        // if no free space
        if (free_slot[0] == -1)
        {
            return -1;
        }
        
        else
        {
            // Add item to slot
            slots[free_slot[0], free_slot[1]].AddItem(item);
            
            return V2ToIndex(new Vector2(free_slot[0], free_slot[1]));
        }
    }

    // Calculate the correct index of the slot by the slot's 2D coordinates
    public int V2ToIndex(Vector2 v2)
    {
        return (int) (v2.x + v2.y * column);
    }

    // Calculate the correct 2D coordinates of the slot by the slot's index
    public Vector2Int IndexToV2(int n)
    {
        return new Vector2Int(n % column, n / column);
    }
}
