using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiInventory : MonoBehaviour
{
    Inventory _inventory;

    public void SetInventory(Inventory inventory)
    {
        this._inventory = inventory;
    }
}
