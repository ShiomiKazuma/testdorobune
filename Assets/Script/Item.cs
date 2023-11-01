using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType
    {
        Knife,
        Heal,
    }

    public ItemType _itemtype;
    public int _amount;
}
