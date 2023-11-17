using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image _icon;
    Item _item;

    public void AddItem(Item newItem)
    {
        _item = newItem;

        _icon.sprite = _item.icon;
        _icon.enabled = true;
    }

    public void ClearSlot()
    {
        _item = null;
        _icon.sprite = null;
        _icon.enabled = false;
    }

    public void RemoveItem()
    {
        Inventory._instance.Remove(_item);
    }
}
