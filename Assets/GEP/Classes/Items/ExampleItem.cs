using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventorySpace;

public class ExampleItem : ItemMonoBase
{
    // Start is called before the first frame update
    void Start()
    {
        item = new RedCube();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// This is where you will want to add your own implementation for your own systems.
    /// </summary>
}
