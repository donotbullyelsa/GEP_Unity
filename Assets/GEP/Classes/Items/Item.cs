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
            RedCube = 1
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
            image_link = "red_cube1";
            description = "\nRed Cube\n\nA red cube, do nothing.";
        }
    }
}