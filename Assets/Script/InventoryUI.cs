using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    Inventory _inventory;
    // Start is called before the first frame update
    void Start()
    {
        _inventory = Inventory._instance;
        _inventory.onItemChangedCallback += UpdateUI;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateUI()
    {
        
    }
}
