using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventorySpace;

public class UIController : MonoBehaviour
{
    public PlayerInventory player_inventory;
    public NPCsInventory npc_inventory;

    public bool getInvenOpenPermission()
    {
        return (((!getPlayerInvenOpen()) && (!getNPCInvenOpen())) && (!npc_inventory.getInvetoryLock()));
    }

    public bool getPlayerInvenOpen()
    {
        return player_inventory.transform.GetChild(0).gameObject.activeSelf;
    }

    public bool getNPCInvenOpen()
    {
        return npc_inventory.transform.GetChild(0).gameObject.activeSelf;
    }

    public void playerToNPC(int n)
    {
        Item.ItemID id = player_inventory.BaseDeleteItem(n);

        Item item = new EmptyItem();
        if (id == Item.ItemID.RedCube)
        {
            item = new RedCube();
        }
        else if (id == Item.ItemID.GreyCube)
        {
            item = new GreyCube();
        }

        npc_inventory.AddItem(item);
    }

    public void NPCToPlayer(int n)
    {
        Item.ItemID id = npc_inventory.DeleteItem(n);

        Item item = new EmptyItem();
        if (id == Item.ItemID.RedCube)
        {
            item = new RedCube();
        }
        else if (id == Item.ItemID.GreyCube)
        {
            item = new GreyCube();
        }

        player_inventory.AddItem(item);
    }
}
