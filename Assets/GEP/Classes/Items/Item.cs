using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySpace
{
    // Add new item class here

    public class Item
    {
        public ItemID id;
        public int stack_limit = 1;
        public string prefab_link = "";
        public string image_link = "";
        public string description = "";

        public Item(ItemID id_)
        {
            id = id_;
        }

        public enum ItemID
        {
            None = 0,
            RedCube = 1,
            GreyCube = 2
        }
    }

    public class EmptyItem: Item
    {
        public EmptyItem(): base(ItemID.None)
        {
            
        }
    }

    public class RedCube: Item
    {
        public RedCube(): base(ItemID.RedCube)
        {
            stack_limit = 3;
            prefab_link = "Prefabs/Example Item";
            image_link = "red_cube";
            description = "\nRed Cube\n\nA red cube, bring it to the NPC.";
        }
    }

    public class GreyCube: Item
    {
        public GreyCube(): base(ItemID.GreyCube)
        {
            stack_limit = 2;
            prefab_link = "Prefabs/Grey Cube";
            image_link = "grey_cube";
            description = "\nGrey Cube\n\nA grey cube, bring it to the NPC.";
        }
    }
}