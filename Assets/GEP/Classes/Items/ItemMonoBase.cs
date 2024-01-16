using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventorySpace;

public class ItemMonoBase : MonoBehaviour, IPickupable
{
    protected Item item;

    public InventoryUIControl inventory_controller_script;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// This is where you will want to add your own implementation for your own systems.
    /// </summary>
    public void Pickup()
    {
        bool added_flag = inventory_controller_script.AddItem(item);
        if (added_flag)
        {
            Destroy(gameObject);
        }
    }

    public int getStackLimit()
    {
        return item.stack_limit;
    }
}