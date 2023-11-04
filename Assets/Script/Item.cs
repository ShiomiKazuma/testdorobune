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

    public Sprite GetSprite()
    {
        switch(_itemtype)
        {
            default:
            case ItemType.Knife:
                return ItemAsset.Instance._knife;
        }
    }
}
